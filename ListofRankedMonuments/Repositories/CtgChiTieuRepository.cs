﻿using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace QUANLYVANHOA.Repositories
{
    public class CtgChiTieuRepository : ICtgChiTieuRepository
    {
        private readonly string _connectionString;

        public CtgChiTieuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<CtgChiTieu>, int)> GetAll(string? name, int pageNumber, int pageSize)
        {
            var chiTieuList = new List<CtgChiTieu>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("CT_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 120;
                    command.Parameters.AddWithValue("@TenChiTieu", name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var chiTieu = new CtgChiTieu
                            {
                                ChiTieuID = reader.GetInt32(reader.GetOrdinal("ChiTieuID")),
                                MaChiTieu = reader["MaChiTieu"].ToString(),
                                TenChiTieu = reader["TenChiTieu"].ToString(),
                                ChiTieuChaID = reader.IsDBNull(reader.GetOrdinal("ChiTieuChaID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ChiTieuChaID")),
                                GhiChu = reader["GhiChu"].ToString(),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                LoaiMauPhieuID = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuID"))
                            };

                            chiTieuList.Add(chiTieu);
                        }

                        await reader.NextResultAsync();
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
            }

            // After fetching the data, build the hierarchy
            var chiTieuHierarchy = BuildHierarchy(chiTieuList);

            return (chiTieuHierarchy, totalRecords);
        }

        public async Task<CtgChiTieu> GetByID(int id)
        {
            CtgChiTieu chiTieu = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("CT_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ChiTieuID", id);
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            chiTieu = new CtgChiTieu
                            {
                                ChiTieuID = reader.GetInt32(reader.GetOrdinal("ChiTieuID")),
                                MaChiTieu = reader["MaChiTieu"].ToString(),
                                TenChiTieu = reader["TenChiTieu"].ToString(),
                                ChiTieuChaID = reader.IsDBNull(reader.GetOrdinal("ChiTieuChaID")) ? 0 : reader.GetInt32(reader.GetOrdinal("ChiTieuChaID")),
                                GhiChu = reader["GhiChu"].ToString(),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                LoaiMauPhieuID = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuID"))
                            };
                        }
                    }
                }
            }

            return chiTieu;
        }

        public async Task<int> Insert(CtgChiTieuModelInsert chiTieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("CT_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MaChiTieu", chiTieu.MaChiTieu);
                    command.Parameters.AddWithValue("@TenChiTieu", chiTieu.TenChiTieu);
                    command.Parameters.AddWithValue("@ChiTieuChaID", chiTieu.ChiTieuChaID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GhiChu", chiTieu.GhiChu ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LoaiMauPhieuID", chiTieu.LoaiMauPhieuID); // Sửa lỗi kiểu dữ liệu

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(CtgChiTieuModelUpdate chiTieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("CT_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ChiTieuID", chiTieu.ChiTieuID);
                    command.Parameters.AddWithValue("@MaChiTieu", chiTieu.MaChiTieu);
                    command.Parameters.AddWithValue("@TenChiTieu", chiTieu.TenChiTieu);
                    command.Parameters.AddWithValue("@ChiTieuChaID", chiTieu.ChiTieuChaID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GhiChu", chiTieu.GhiChu ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LoaiMauPhieuID", chiTieu.LoaiMauPhieuID); // Sửa lỗi kiểu dữ liệu

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("CT_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ChiTieuID", id);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        private List<CtgChiTieu> BuildHierarchy(List<CtgChiTieu> chiTieuList)
        {
            var lookup = chiTieuList.ToLookup(c => c.ChiTieuChaID);
            var rootItems = lookup[0].ToList();

            // Để đảm bảo tất cả các cấp độ của cây đều được bao gồm
            foreach (var item in chiTieuList)
            {
                var parent = chiTieuList.FirstOrDefault(c => c.ChiTieuID == item.ChiTieuChaID);
                if (parent != null)
                {
                    parent.Children.Add(item);
                }
            }

            return rootItems;
        }
    }
}
