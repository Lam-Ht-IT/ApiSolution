using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Controllers;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace QUANLYVANHOA.Repositories
{
    public class DiTichXepHangRepository : IDiTichXepHangRepository
    {
        private readonly string _connectionString;

        public DiTichXepHangRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<(IEnumerable<DiTichXepHang>, int)> GetAll(string name, int pageNumber, int pageSize)
        {
            var ditichlist = new List<DiTichXepHang>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Lấy dữ liệu phân trang
                using (var command = new SqlCommand("DTXH_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenDiTich", name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ditichlist.Add(new DiTichXepHang
                            {
                                DitichXepHangID = reader.GetInt32("DiTichXepHangID"),
                                TenDiTich = reader["TenDiTich"].ToString(),
                                ThuTuXepHang = reader.GetInt32("ThuTuXepHang")
                            });
                        }

                        // Đọc tổng số bản ghi từ truy vấn riêng biệt
                        await reader.NextResultAsync(); // Di chuyển đến kết quả thứ hai
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32("TotalRecords");
                        }
                    }
                }
            }

            return (ditichlist, totalRecords);
        }

        public async Task<DiTichXepHang> GetByID(int id)
        {
            DiTichXepHang ditichXepHang = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiTichXepHangID", id);
                    await connection.OpenAsync();
                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ditichXepHang = new DiTichXepHang
                            {
                                DitichXepHangID = reader.GetInt32("DiTichXepHangID"),
                                TenDiTich = reader["TenDiTich"].ToString(),
                                ThuTuXepHang = reader.GetInt32("ThuTuXepHang")
                            };
                        }
                    }
                }
            }

            return ditichXepHang;
        }

        public async Task<int> Insert(DiTichXepHang diTichXepHang)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDiTich", diTichXepHang.TenDiTich);
                    cmd.Parameters.AddWithValue("@ThuTuXepHang",diTichXepHang.ThuTuXepHang);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            } 
        }


        public async Task<int> Update(DiTichXepHang ditichxephang)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiTichXepHangID", ditichxephang.DitichXepHangID);
                    cmd.Parameters.AddWithValue("@TenDiTich", ditichxephang.TenDiTich);
                    cmd.Parameters.AddWithValue("ThuTuXepHang", ditichxephang.ThuTuXepHang);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_Delete", connection))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiTichXepHangID", id);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                    
                }
               
            }
        }
    }
}
