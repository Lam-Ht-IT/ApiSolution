using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Controllers;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QUANLYVANHOA.Repositories
{
    public class CtgDonViTinhRepository : ICtgDonViTinhRepository
    {
        private readonly string _connectionString;

        public CtgDonViTinhRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<CtgDonViTinh>, int)> GetAll(string name, int pageNumber, int pageSize)
        {
            var donViTinhList = new List<CtgDonViTinh>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DVT_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenDonViTinh", name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            donViTinhList.Add(new CtgDonViTinh
                            {
                                DonViTinhID = reader.GetInt32(reader.GetOrdinal("DonViTinhID")),
                                TenDonViTinh = reader.GetString(reader.GetOrdinal("TenDonViTinh")),
                                MaDonViTinh = reader.GetString(reader.GetOrdinal("MaDonViTinh")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu"))
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

            return (donViTinhList, totalRecords);
        }

        public async Task<CtgDonViTinh> GetByID(int id)
        {
            CtgDonViTinh donViTinh = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DVT_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DonViTinhID", id);
                    await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            donViTinh = new CtgDonViTinh
                            {
                                DonViTinhID = reader.GetInt32(reader.GetOrdinal("DonViTinhID")),
                                TenDonViTinh = reader.GetString(reader.GetOrdinal("TenDonViTinh")),
                                MaDonViTinh = reader.GetString(reader.GetOrdinal("MaDonViTinh")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu"))
                            };
                        }
                    }
                }
            }

            return donViTinh;
        }

        public async Task<int> Insert(CtgDonViTinhModelInsert donViTinh)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DVT_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDonViTinh", donViTinh.TenDonViTinh);
                    cmd.Parameters.AddWithValue("@MaDonViTinh", donViTinh.MaDonViTinh);
                    cmd.Parameters.AddWithValue("@TrangThai", donViTinh.TrangThai);
                    cmd.Parameters.AddWithValue("@GhiChu", donViTinh.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(CtgDonViTinhModelUpdate donViTinh)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DVT_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DonViTinhID", donViTinh.DonViTinhID);
                    cmd.Parameters.AddWithValue("@TenDonViTinh", donViTinh.TenDonViTinh);
                    cmd.Parameters.AddWithValue("@MaDonViTinh", donViTinh.MaDonViTinh);
                    cmd.Parameters.AddWithValue("@TrangThai", donViTinh.TrangThai);
                    cmd.Parameters.AddWithValue("@GhiChu", donViTinh.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DVT_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DonViTinhID", id);

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
