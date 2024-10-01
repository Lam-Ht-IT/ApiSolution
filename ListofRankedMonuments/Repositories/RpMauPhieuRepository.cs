using QUANLYVANHOA.Interfaces;
using Newtonsoft.Json;
using QUANLYVANHOA.Controllers;
using System.Data.SqlClient;
using System.Data;
using QUANLYVANHOA.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QUANLYVANHOA.Repositories
{
    public class RpMauPhieuRepository : IRpMauPhieuRepository
    {
        private readonly string _connectionString;

        public RpMauPhieuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<RpMauPhieu>, int)> GetAll(string? name, int pageNumber, int pageSize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("MP_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenMauPhieu", pageNumber);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    await connection.OpenAsync();
                    var result = new List<RpMauPhieu>();
                    int totalRecords = 0;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var mauPhieu = new RpMauPhieu
                            {
                                MauPhieuID = reader.GetInt32(0),
                                TenMauPhieu = reader.GetString(1),
                                LoaiMauPhieuID = reader.GetInt32(2),
                                MaMauPhieu = reader.GetString(3)
                            };

                            // Lấy danh sách chỉ tiêu và tiêu chí theo phân cấp
                            mauPhieu.ChiTieus = await GetChiTieusHierarchyByMauPhieuID(mauPhieu.MauPhieuID);
                            mauPhieu.TieuChis = await GetTieuChisHierarchyByMauPhieuID(mauPhieu.MauPhieuID);
                            result.Add(mauPhieu);
                        }
                        await reader.NextResultAsync();
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                    return (result, totalRecords);
                }
            }
        }


        public async Task<RpMauPhieu> GetByID(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("MP_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", id);

                    await connection.OpenAsync();
                    RpMauPhieu mauPhieu = null;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            mauPhieu = new RpMauPhieu
                            {
                                MauPhieuID = reader.GetInt32(0),
                                TenMauPhieu = reader.GetString(1),
                                LoaiMauPhieuID = reader.GetInt32(2),
                                MaMauPhieu =reader.GetString(3)
                            };
                        }
                    }

                    if (mauPhieu != null)
                    {
                        mauPhieu.ChiTieus = await GetChiTieusHierarchyByMauPhieuID(id);
                        mauPhieu.TieuChis = await GetTieuChisHierarchyByMauPhieuID(id);
                        mauPhieu.ChiTietMauPhieus = await GetChiTietMauPhieuByMauPhieuID(id);
                    }

                    return mauPhieu;
                }
            }
        }


        // Thêm mới mẫu phiếu
        public async Task<int> InsertMauPhieu(RpMauPhieuInsertModel mauPhieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Thêm Mẫu Phiếu
                        using (var command = new SqlCommand("MP_Insert", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@TenMauPhieu", mauPhieu.TenMauPhieu);
                            command.Parameters.AddWithValue("@LoaiMauPhieu", mauPhieu.LoaiMauPhieuID);
                            command.Parameters.AddWithValue("@MaMauPhieu", mauPhieu.MaMauPhieu);
                            command.Parameters.AddWithValue("@NguoiTao", mauPhieu.NguoiTao);

                            var id = (int)await command.ExecuteScalarAsync(); // Lấy ID mẫu phiếu vừa được thêm

                            // 2. Thêm Chỉ Tiêu nếu có
                            if (mauPhieu.ChiTieus != null && mauPhieu.ChiTieus.Count > 0)
                            {
                                foreach (var chiTieu in mauPhieu.ChiTieus)
                                {
                                    await AddChiTieuMauPhieu(mauPhieu.MauPhieuID,chiTieu.ChiTieuID);
                                }
                            }

                            // 3. Thêm Tiêu Chí nếu có
                            if (mauPhieu.TieuChis != null && mauPhieu.TieuChis.Count > 0)
                            {
                                foreach (var tieuChi in mauPhieu.TieuChis)
                                {
                                    await AddTieuChiMauPhieu(mauPhieu.MauPhieuID,tieuChi.TieuChiID);
                                }
                            }

                            // 4. Thêm Chi Tiết Mẫu Phiếu nếu có
                            if (mauPhieu.ChiTietMauPhieus != null && mauPhieu.ChiTietMauPhieus.Count > 0)
                            {
                                foreach (var chiTiet in mauPhieu.ChiTietMauPhieus)
                                {
                                    await AddChiTietMauPhieu(chiTiet);
                                }
                            }

                            // Commit transaction
                            transaction.Commit();
                            return id; // Trả về ID mẫu phiếu vừa được thêm
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction nếu có lỗi
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<int> Update(RpMauPhieuUpdateModel obj)
        {
            int result = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("MP_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", obj.MauPhieuID);
                    command.Parameters.AddWithValue("@TenMauPhieu", obj.TenMauPhieu);
                    command.Parameters.AddWithValue("@MaMauPhieu", obj.MaMauPhieu);
                    command.Parameters.AddWithValue("@LoaiMauPhieuId", obj.LoaiMauPhieuID);
                    command.Parameters.AddWithValue("@NguoiTao", obj.NguoiTao);

                    result = await command.ExecuteNonQueryAsync();
                }
            }

            return result;
        }

        public async Task<int> Delete(int id)
        {
            int result = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("MP_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", id);

                    result = await command.ExecuteNonQueryAsync();
                }
            }

            return result;
        }

        // Lấy thông tin mẫu phiếu
        // Lấy các chỉ tiêu liên quan đến mẫu phiếu
        public async Task<List<CtgChiTieu>> GetChiTieusHierarchyByMauPhieuID(int mauPhieuId)
        {
            var chiTieus = new List<CtgChiTieu>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("Rp_GetReport", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Skip mẫu phiếu data
                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            chiTieus.Add(new CtgChiTieu
                            {
                                ChiTieuID = reader.GetInt32("ChiTieuID"),
                                TenChiTieu = reader.GetString("TenChiTieu"),
                                ChiTieuChaID = reader.IsDBNull(reader.GetOrdinal("ChiTieuChaID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ChiTieuChaID")),
                                GhiChu = reader.GetString("GhiChu")
                            });
                        }
                    }
                }
            }

            return chiTieus;
        }

        // Lấy các tiêu chí liên quan đến mẫu phiếu
        public async Task<List<CtgTieuChi>> GetTieuChisHierarchyByMauPhieuID(int mauPhieuId)
        {
            var tieuChis = new List<CtgTieuChi>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("MP_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Skip mẫu phiếu và chỉ tiêu data
                        await reader.NextResultAsync();
                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            tieuChis.Add(new CtgTieuChi
                            {
                                TieuChiID = reader.GetInt32("TieuChiID"),
                                TenTieuChi = reader.GetString("TenTieuChi"),
                                TieuChiChaID = reader.IsDBNull(reader.GetOrdinal("TieuChiChaID")) ? (int?)null : reader.GetInt32("TieuChiChaID"),
                                GhiChu = reader.GetString("GhiChu"),
                                KieuDuLieuCot = reader.GetInt32("KieuDuLieuCot")
                            });
                        }
                    }
                }
            }

            return tieuChis;
        }

        // Lấy chi tiết mẫu phiếu (bao gồm thông tin gộp cột và nội dung)
        public async Task<List<RpChiTietMauPhieu>> GetChiTietMauPhieuByMauPhieuID(int mauPhieuId)
        {
            var chiTietMauPhieus = new List<RpChiTietMauPhieu>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("MP_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Skip các resultset không cần thiết
                        await reader.NextResultAsync();
                        await reader.NextResultAsync();
                        await reader.NextResultAsync();

                        while (await reader.ReadAsync())
                        {
                            chiTietMauPhieus.Add(new RpChiTietMauPhieu
                            {
                                ChiTietMauPhieuID = reader.GetInt32(reader.GetOrdinal("ChiTietMauPhieuID")),
                                MauPhieuID = reader.GetInt32(reader.GetOrdinal("MauPhieuID")),
                                // Sử dụng JsonConvert để phân tích chuỗi JSON thành List<int>
                                TieuChiIDs = JsonConvert.DeserializeObject<List<int>>(reader.GetString(reader.GetOrdinal("TieuChiIDs"))),
                                ChiTieuID = reader.GetInt32(reader.GetOrdinal("ChiTieuID")),
                                NoiDung = reader.GetString(reader.GetOrdinal("NoiDung")),
                                GopCot = reader.GetBoolean(reader.GetOrdinal("GopCot")),
                                GopTuCot = reader.GetInt32(reader.GetOrdinal("GopTuCot")),
                                GopDenCot = reader.GetInt32(reader.GetOrdinal("GopDenCot")),
                                SoCotGop = reader.GetInt32(reader.GetOrdinal("SoCotGop")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu"))
                            });
                        }
                    }
                }
            }

            return chiTietMauPhieus;
        }

        // Thêm tiêu chí vào mẫu phiếu
        public async Task AddTieuChiMauPhieu(int mauPhieuId, int tieuChiId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCTC_AddTieuChiBaoCao", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);
                    command.Parameters.AddWithValue("@TieuChiID", tieuChiId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Xóa tiêu chí khỏi mẫu phiếu
        public async Task DeleteTieuChiMauPhieu(int mauPhieuId, int tieuChiId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCTC_DeleteTieuChiBaoCao", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);
                    command.Parameters.AddWithValue("@TieuChiID", tieuChiId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Thêm chỉ tiêu vào mẫu phiếu
        public async Task AddChiTieuMauPhieu(int mauPhieuId, int chiTieuId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCCT_AddChiTieuBaoCao", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);
                    command.Parameters.AddWithValue("@ChiTieuID", chiTieuId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Xóa chỉ tiêu khỏi mẫu phiếu
        public async Task DeleteChiTieuMauPhieu(int mauPhieuId, int chiTieuId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCCT_DeleteChiTieuBaoCao", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieuId);
                    command.Parameters.AddWithValue("@ChiTieuID", chiTieuId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Thêm mới ChiTietMauPhieu
        public async Task AddChiTietMauPhieu(RpChiTietMauPhieu chiTietMauPhieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCCTMMP_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", chiTietMauPhieu.MauPhieuID);
                    command.Parameters.AddWithValue("@TieuChiIDs", JsonConvert.SerializeObject(chiTietMauPhieu.TieuChiIDs));
                    command.Parameters.AddWithValue("@ChiTieuID", chiTietMauPhieu.ChiTieuID);
                    command.Parameters.AddWithValue("@NoiDung", chiTietMauPhieu.NoiDung ?? string.Empty);
                    command.Parameters.AddWithValue("@GopCot", chiTietMauPhieu.GopCot != null ? (object)chiTietMauPhieu.GopCot : DBNull.Value);
                    command.Parameters.AddWithValue("@GopTuCot", chiTietMauPhieu.GopTuCot != null ? (object)chiTietMauPhieu.GopTuCot : DBNull.Value);
                    command.Parameters.AddWithValue("@GopDenCot", chiTietMauPhieu.GopDenCot != null ? (object)chiTietMauPhieu.GopDenCot : DBNull.Value);
                    command.Parameters.AddWithValue("@SoCotGop", chiTietMauPhieu.SoCotGop != null ? (object)chiTietMauPhieu.SoCotGop : DBNull.Value);
                    command.Parameters.AddWithValue("@GhiChu", chiTietMauPhieu.GhiChu ?? string.Empty);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Sửa ChiTietMauPhieu
        public async Task UpdateChiTietMauPhieu(RpChiTietMauPhieuUpdateModel chiTietMauPhieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCCTMP_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ChiTietMauPhieuID", chiTietMauPhieu.ChiTietMauPhieuID);
                    command.Parameters.AddWithValue("@TieuChiIDs", chiTietMauPhieu.TieuChiIDs);
                    command.Parameters.AddWithValue("@ChiTieuID", chiTietMauPhieu.ChiTieuID);
                    command.Parameters.AddWithValue("@NoiDung", chiTietMauPhieu.NoiDung ?? string.Empty);
                    command.Parameters.AddWithValue("@GopCot", chiTietMauPhieu.GopCot != null ? (object)chiTietMauPhieu.GopCot : DBNull.Value);
                    command.Parameters.AddWithValue("@GopTuCot", chiTietMauPhieu.GopTuCot != null ? (object)chiTietMauPhieu.GopTuCot : DBNull.Value);
                    command.Parameters.AddWithValue("@GopDenCot", chiTietMauPhieu.GopDenCot != null ? (object)chiTietMauPhieu.GopDenCot : DBNull.Value);
                    command.Parameters.AddWithValue("@SoCotGop", chiTietMauPhieu.SoCotGop != null ? (object)chiTietMauPhieu.SoCotGop : DBNull.Value);
                    command.Parameters.AddWithValue("@GhiChu", chiTietMauPhieu.GhiChu ?? string.Empty);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Xóa ChiTietMauPhieu
        public async Task DeleteChiTietMauPhieu(int chiTietMauPhieuId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BCCTMP_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ChiTietMauPhieuID", chiTietMauPhieuId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
