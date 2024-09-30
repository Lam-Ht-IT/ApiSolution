using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Controllers;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QUANLYVANHOA.Repositories
{
    public class CtgDiTichXepHangRepository : ICtgDiTichXepHangRepository
    {
        private readonly string _connectionString;

        public CtgDiTichXepHangRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<CtgDiTichXepHang>, int)> GetAll(string name, int pageNumber, int pageSize)
        {
            var ditichList = new List<CtgDiTichXepHang>();
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
                            ditichList.Add(new CtgDiTichXepHang
                            {
                                DiTichXepHangID = reader.GetInt32(reader.GetOrdinal("DiTichXepHangID")),
                                TenDiTich = reader.GetString(reader.GetOrdinal("TenDiTich")),
                                MaDiTich = reader.GetString(reader.GetOrdinal("MaDiTich")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                Loai = reader.GetInt32(reader.GetOrdinal("Loai"))
                            });
                        }

                        // Đọc tổng số bản ghi từ truy vấn riêng biệt
                        await reader.NextResultAsync(); // Di chuyển đến kết quả thứ hai
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
            }

            return (ditichList, totalRecords);
        }

        public async Task<CtgDiTichXepHang> GetByID(int id)
        {
            CtgDiTichXepHang ditichXepHang = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiTichXepHangID", id);
                    await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            ditichXepHang = new CtgDiTichXepHang
                            {
                                DiTichXepHangID = reader.GetInt32(reader.GetOrdinal("DiTichXepHangID")),
                                DiTichXepHangChaID = reader.GetInt32(reader.GetOrdinal("DiTichXepHangChaID")),
                                TenDiTich = reader.GetString(reader.GetOrdinal("TenDiTich")),
                                MaDiTich = reader.GetString(reader.GetOrdinal("MaDiTich")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                Loai = reader.GetInt32(reader.GetOrdinal("Loai"))
                            };
                        }
                    }
                }
            }
            return ditichXepHang;
        }

        public async Task<int> Insert(CtgDiTichXepHangModelInsert diTichXepHang)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDiTich", diTichXepHang.TenDiTich);
                    cmd.Parameters.AddWithValue("@GhiChu", diTichXepHang.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(CtgDiTichXepHangModelUpdate diTichXepHang)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DTXH_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiTichXepHangID", diTichXepHang.DiTichXepHangID);
                    cmd.Parameters.AddWithValue("@TenDiTich", diTichXepHang.TenDiTich);
                    cmd.Parameters.AddWithValue("@GhiChu", diTichXepHang.GhiChu);
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
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DiTichXepHangID", id);

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
