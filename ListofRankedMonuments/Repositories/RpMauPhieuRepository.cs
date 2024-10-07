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
            var rpMauPhieuList = new List<RpMauPhieu>();
            int totalRecords = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("MP_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenMauPhieu", name);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            rpMauPhieuList.Add(new RpMauPhieu
                            {
                                MauPhieuID = reader.GetInt32(reader.GetOrdinal("MauPhieuID")),
                                TenMauPhieu = reader.GetString(reader.GetOrdinal("TenMauPhieu")),
                                LoaiMauPhieuID = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuID")),
                                MaMauPhieu = reader.GetString(reader.GetOrdinal("MaMauPhieu")),
                                NgayTao = reader.GetDateTime(reader.GetOrdinal("NgayTao")),
                                NguoiTao = reader.GetString(reader.GetOrdinal("NguoiTao"))
                            });

                            
                        }
                        await reader.NextResultAsync();
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
            }
            return (rpMauPhieuList, totalRecords);
        }

        public async Task<RpMauPhieu> GetMauPhieuByID(int id)
        {
            RpMauPhieu mauPhieu = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("MP_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MauPhieuID", id);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Đọc thông tin mẫu phiếu chính
                        if (await reader.ReadAsync())
                        {
                            mauPhieu = new RpMauPhieu
                            {
                                MauPhieuID = reader.GetInt32(reader.GetOrdinal("MauPhieuID")),
                                TenMauPhieu = reader.GetString(reader.GetOrdinal("TenMauPhieu")),
                                MaMauPhieu = reader.GetString(reader.GetOrdinal("MaMauPhieu")),
                                LoaiMauPhieuID = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuID")),
                                NgayTao = reader.GetDateTime(reader.GetOrdinal("NgayTao")),
                                NguoiTao = reader.GetString(reader.GetOrdinal("NguoiTao")),

                                // Lấy trực tiếp chuỗi JSON
                                ChiTieuS = reader.IsDBNull(reader.GetOrdinal("ChiTieuS")) ? null : reader.GetString(reader.GetOrdinal("ChiTieuS")),
                                TieuChiS = reader.IsDBNull(reader.GetOrdinal("TieuChiS")) ? null : reader.GetString(reader.GetOrdinal("TieuChiS"))
                            };
                        }

                        // Đọc danh sách các chi tiết mẫu phiếu nếu có
                        if (await reader.NextResultAsync())
                        {
                            mauPhieu.ChiTietMauPhieus = new List<RpChiTietMauPhieu>();
                            while (await reader.ReadAsync())
                            {
                                mauPhieu.ChiTietMauPhieus.Add(new RpChiTietMauPhieu
                                {
                                    ChiTietMauPhieuID = reader.GetInt32(reader.GetOrdinal("ChiTietMauPhieuID")),
                                    MauPhieuID = reader.GetInt32(reader.GetOrdinal("MauPhieuID")),
                                    ChiTieuID = reader.GetInt32(reader.GetOrdinal("ChiTieuID")),
                                    TieuChiIDs = JsonConvert.DeserializeObject<List<int>>(reader.GetString(reader.GetOrdinal("TieuChiIDs"))), // Deserialize từ chuỗi thành List<int>
                                    NoiDung = reader.IsDBNull(reader.GetOrdinal("NoiDung")) ? null : reader.GetString(reader.GetOrdinal("NoiDung")),
                                    GopCot = reader.GetBoolean(reader.GetOrdinal("GopCot")),
                                    GopTuCot = reader.GetInt32(reader.GetOrdinal("GopTuCot")),
                                    GopDenCot = reader.GetInt32(reader.GetOrdinal("GopDenCot")),
                                    SoCotGop = reader.GetInt32(reader.GetOrdinal("SoCotGop")),
                                    GhiChu = reader.IsDBNull(reader.GetOrdinal("GhiChu")) ? null : reader.GetString(reader.GetOrdinal("GhiChu"))
                                });
                            }
                        }
                    }
                }
            }
            return mauPhieu;
        }

        // Thêm mới mẫu phiếu
        public async Task<int> CreateMauPhieu(RpMauPhieuInsertModel mauPhieu)
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

                    // Truyền xuống dữ liệu JSON string
                    command.Parameters.AddWithValue("@ChiTieus", mauPhieu.ChiTieuS);
                    command.Parameters.AddWithValue("@TieuChis", mauPhieu.TieuChiS);

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
                    string chiTieusJson = JsonConvert.SerializeObject(mauPhieu.ChiTieuS); 
                    string tieuChisJson = JsonConvert.SerializeObject(mauPhieu.TieuChiS);
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

    }
}
