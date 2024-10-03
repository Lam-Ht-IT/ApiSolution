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

        public async Task<(IEnumerable<RpMauPhieu>, int)> GetAllMauPhieu(string? name, int pageNumber, int pageSize)
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

        public async Task<RpMauPhieu> GetMauPhieuByID(int id)
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
                using (var command = new SqlCommand("MP_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenMauPhieu", mauPhieu.TenMauPhieu);
                    command.Parameters.AddWithValue("@LoaiMauPhieuID", mauPhieu.LoaiMauPhieuID);
                    command.Parameters.AddWithValue("@MaMauPhieu", mauPhieu.MaMauPhieu);
                    command.Parameters.AddWithValue("@NguoiTao", mauPhieu.NguoiTao);
                    command.Parameters.AddWithValue("@ChiTieus", mauPhieu.ChiTieus != null ? JsonConvert.SerializeObject(mauPhieu.ChiTieus) : DBNull.Value);
                    command.Parameters.AddWithValue("@TieuChis", mauPhieu.TieuChis != null ? JsonConvert.SerializeObject(mauPhieu.TieuChis) : DBNull.Value);
                    command.Parameters.AddWithValue("@ChiTietMauPhieus", mauPhieu.ChiTietMauPhieus != null ? JsonConvert.SerializeObject(mauPhieu.ChiTietMauPhieus) : DBNull.Value);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> UpdateMauPhieu(RpMauPhieuUpdateModel mauPhieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UpdateMauPhieu", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm các parameter cho mẫu phiếu chính
                    command.Parameters.AddWithValue("@MauPhieuID", mauPhieu.MauPhieuID);
                    command.Parameters.AddWithValue("@TenMauPhieu", mauPhieu.TenMauPhieu);
                    command.Parameters.AddWithValue("@MaMauPhieu", mauPhieu.MaMauPhieu);
                    command.Parameters.AddWithValue("@LoaiMauPhieuID", mauPhieu.LoaiMauPhieuID);
                    command.Parameters.AddWithValue("@NguoiTao", mauPhieu.NguoiTao);

                    // Chuyển danh sách Chỉ Tiêu, Tiêu Chí, Chi Tiết Mẫu Phiếu sang JSON
                    string chiTieusJson = JsonConvert.SerializeObject(mauPhieu.ChiTieus); 
                    string tieuChisJson = JsonConvert.SerializeObject(mauPhieu.TieuChis);
                    string chiTietMauPhieusJson = JsonConvert.SerializeObject(mauPhieu.ChiTietMauPhieus);

                    // Thêm các parameter cho Chỉ Tiêu, Tiêu Chí và Chi Tiết Mẫu Phiếu
                    command.Parameters.AddWithValue("@ChiTieus", (object)chiTieusJson ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TieuChis", (object)tieuChisJson ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ChiTietMauPhieus", (object)chiTietMauPhieusJson ?? DBNull.Value);

                    // Mở kết nối
                    await connection.OpenAsync();

                    // Thực thi Stored Procedure và trả về số bản ghi bị ảnh hưởng
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> DeleteMauPhieu(int id)
        {
            int result = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new SqlCommand("MP_Delete", connection, transaction))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@MauPhieuID", id);

                            result = await command.ExecuteNonQueryAsync();
                        }

                        // Nếu mọi thứ thành công thì commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi thì rollback transaction
                        transaction.Rollback();
                        throw;
                    }
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
                using (var command = new SqlCommand("BCCT_GetByID", connection))
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
                using (var command = new SqlCommand("BCTC_GetAllTieuChiByMauPhieuID", connection))
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
        public async Task AddTieuChiBaoCao(int mauPhieuId, int tieuChiId)
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
        public async Task DeleteTieuChiBaoCao(int mauPhieuId, int tieuChiId)
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
        public async Task AddChiTieuBaoCao(int mauPhieuId, int chiTieuId)
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
        public async Task DeleteChiTieuBaoCao(int mauPhieuId, int chiTieuId)
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
        public async Task AddChiTietBaoCao(RpChiTietMauPhieu chiTietMauPhieu)
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
        public async Task UpdateChiTietBaoCao(RpChiTietMauPhieuUpdateModel chiTietMauPhieu)
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
        public async Task DeleteChiTietBaoCao(int chiTietMauPhieuId)
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
