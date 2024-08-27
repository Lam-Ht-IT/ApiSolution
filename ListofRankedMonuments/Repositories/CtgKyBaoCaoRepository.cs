using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models; // Thay thế bằng không gian tên chứa lớp KyBaoCao
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QUANLYVANHOA.Repositories
{
    public class CtgKyBaoCaoRepository : ICtgKyBaoCaoRepository
    {
        private readonly string _connectionString;

        public CtgKyBaoCaoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<CtgKyBaoCao>, int)> GetAll(string? name, int pageNumber, int pageSize)
        {
            var kyBaoCaoList = new List<CtgKyBaoCao>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("KBC_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenKyBaoCao", name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            kyBaoCaoList.Add(new CtgKyBaoCao
                            {
                                KyBaoCaoID = reader.GetInt32("KyBaoCaoID"),
                                KyBaoCaoChaID = reader.GetInt32("KyBaoCaoChaID"),
                                TenKyBaoCao = reader.GetString(reader.GetOrdinal("TenKyBaoCao")),
                                TrangThai = reader.GetBoolean("TrangThai"),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                LoaiKyBaoCao = reader.GetInt32("LoaiKyBaoCao")
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

            return (kyBaoCaoList, totalRecords);
        }

        public async Task<CtgKyBaoCao> GetByID(int id)
        {
            CtgKyBaoCao kyBaoCao = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("KBC_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@KyBaoCaoID", id);   

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            kyBaoCao = new CtgKyBaoCao
                            {
                                KyBaoCaoID = reader.GetInt32("KyBaoCaoID"),
                                KyBaoCaoChaID = reader.GetInt32("KyBaoCaoChaID"),
                                TenKyBaoCao = reader.GetString(reader.GetOrdinal("TenKyBaoCao")),
                                TrangThai = reader.GetBoolean("TrangThai"),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                LoaiKyBaoCao = reader.GetInt32("LoaiKyBaoCao")
                            };
                        }
                    }
                }
            }

            return kyBaoCao;
        }

        public async Task<int> Insert(CtgKyBaoCaoModelInsert kyBaoCao)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("KBC_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenKyBaoCao", kyBaoCao.TenKyBaoCao);
                    command.Parameters.AddWithValue("@TrangThai", kyBaoCao.TrangThai);
                    command.Parameters.AddWithValue("@GhiChu", kyBaoCao.GhiChu);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(CtgKyBaoCaoModelUpdate kyBaoCao)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("KBC_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@KyBaoCaoID", kyBaoCao.KyBaoCaoID);
                    command.Parameters.AddWithValue("@TenKyBaoCao", kyBaoCao.TenKyBaoCao);
                    command.Parameters.AddWithValue("@TrangThai", kyBaoCao.TrangThai);
                    command.Parameters.AddWithValue("@GhiChu", kyBaoCao.GhiChu);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("KBC_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@KyBaoCaoID", id);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
