using QUANLYVANHOA.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Repositories
{
    public class CtgLoaiDiTichRepository : ICtgLoaiDiTichRepository
    {
        private readonly string _connectionString;

        public CtgLoaiDiTichRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<CtgLoaiDiTich>, int)> GetAll(string name, int pageNumber, int pageSize)
        {
            var loaiDiTichList = new List<CtgLoaiDiTich>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("LDT_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenLoaiDiTich", name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            loaiDiTichList.Add(new CtgLoaiDiTich
                            {
                                LoaiDiTichID = reader.GetInt32(reader.GetOrdinal("LoaiDiTichID")),
                                LoaiDiTichChaID = reader.GetInt32(reader.GetOrdinal("LoaiDiTichChaID")),
                                TenLoaiDiTich = reader.GetString(reader.GetOrdinal("TenLoaiDiTich")),
                                MaLoaiDiTich = reader.GetString(reader.GetOrdinal("MaLoaiDiTich")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                Loai = reader.GetInt32(reader.GetOrdinal("Loai"))
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

            return (loaiDiTichList, totalRecords);
        }

        public async Task<CtgLoaiDiTich> GetByID(int id)
        {
            CtgLoaiDiTich loaiDiTich = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LDT_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoaiDiTichID", id);
                    await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            loaiDiTich = new CtgLoaiDiTich
                            {
                                LoaiDiTichID = reader.GetInt32(reader.GetOrdinal("LoaiDiTichID")),
                                LoaiDiTichChaID = reader.GetInt32(reader.GetOrdinal("LoaiDiTichChaID")),
                                TenLoaiDiTich = reader.GetString(reader.GetOrdinal("TenLoaiDiTich")),
                                MaLoaiDiTich = reader.GetString(reader.GetOrdinal("MaLoaiDiTich")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                Loai = reader.GetInt32(reader.GetOrdinal("Loai"))
                            };
                        }
                    }
                }
            }

            return loaiDiTich;
        }

        public async Task<int> Insert(CtgLoaiDiTichModelInsert loaiDiTich)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LDT_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenLoaiDiTich", loaiDiTich.TenLoaiDiTich);
                    cmd.Parameters.AddWithValue("@GhiChu", loaiDiTich.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(CtgLoaiDiTichModelUpdate loaiDiTich)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LDT_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoaiDiTichID", loaiDiTich.LoaiDiTichID);
                    cmd.Parameters.AddWithValue("@TenLoaiDiTich", loaiDiTich.TenLoaiDiTich);
                    cmd.Parameters.AddWithValue("@GhiChu", loaiDiTich.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LDT_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoaiDiTichID", id);

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
