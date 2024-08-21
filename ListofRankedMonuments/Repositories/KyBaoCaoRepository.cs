﻿using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models; // Thay thế bằng không gian tên chứa lớp KyBaoCao
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QUANLYVANHOA.Repositories
{
    public class KyBaoCaoRepository : IKyBaoCaoRepository
    {
        private readonly string _connectionString;

        public KyBaoCaoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<KyBaoCao>, int)> GetAll(string? name, int pageNumber, int pageSize)
        {
            var kyBaoCaoList = new List<KyBaoCao>();
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
                            kyBaoCaoList.Add(new KyBaoCao
                            {
                                KyBaoCaoID = reader.GetInt32("KyBaoCaoID"),
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

        public async Task<KyBaoCao> GetByID(int id)
        {
            KyBaoCao kyBaoCao = null;

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
                            kyBaoCao = new KyBaoCao
                            {
                                KyBaoCaoID = reader.GetInt32("KyBaoCaoID"),
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

        public async Task<int> Insert(KyBaoCao kyBaoCao)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("KBC_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenKyBaoCao", kyBaoCao.TenKyBaoCao);
                    command.Parameters.AddWithValue("@TrangThai", kyBaoCao.TrangThai);
                    command.Parameters.AddWithValue("@GhiChu", kyBaoCao.GhiChu);
                    command.Parameters.AddWithValue("@LoaiKyBaoCao", kyBaoCao.LoaiKyBaoCao);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(KyBaoCao kyBaoCao)
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
                    command.Parameters.AddWithValue("@LoaiKyBaoCao", kyBaoCao.LoaiKyBaoCao);

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
