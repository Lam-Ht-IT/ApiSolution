//using QUANLYVANHOA.Interfaces;
//using QUANLYVANHOA.Controllers;
//using System.Data.SqlClient;
//using System.Data;
//using QUANLYVANHOA.Models;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace QUANLYVANHOA.Repositories
//{
//    public class RpChiTietMauPhieuRepository
//    {
//        private readonly string _connectionString;

//        public RpChiTietMauPhieuRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task<(IEnumerable<RpChiTietMauPhieu>, int)> GetAll(string name)
//        {
//            var mauPhieuList = new List<RpChiTietMauPhieu>();
//            int totalRecords = 0;

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_GetAll", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@TenMauPhieu", name ?? (object)DBNull.Value);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            mauPhieuList.Add(new RpChiTietMauPhieu
//                            {
//                                ChiTietMauPhieuId = reader.GetInt32(reader.GetOrdinal("ChiTietMauPhieuId")),
//                                MauPhieuId = reader.GetInt32(reader.GetOrdinal("MauPhieuId")),
//                                TieuChiIDs = reader.GetString(reader.GetOrdinal("TieuChiIDs")),
//                                ChitieuID = reader.GetInt32(reader.GetOrdinal("ChitieuIDs")),
//                                NoiDung = reader.GetString(reader.GetOrdinal("NoiDung")),
//                                GopCot = reader.GetInt32(reader.GetOrdinal("GopCot")),
//                                GoptuCot = reader.GetInt32(reader.GetOrdinal("GoptuCot")),
//                                GopDenCot = reader.GetInt32(reader.GetOrdinal("GopDenCot")),
//                                SoCotGop = reader.GetInt32(reader.GetOrdinal("SoCotGop")),
//                                GhiChu = reader.GetString(reader.GetOrdinal("GhiCchu"))
//                            });
//                        }

//                        await reader.NextResultAsync();
//                        if (await reader.ReadAsync())
//                        {
//                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
//                        }
//                    }
//                }
//            }

//            return (mauPhieuList, totalRecords);
//        }

//        public async Task<RpChiTietMauPhieu> GetByID(int id)
//        {
//            RpChiTietMauPhieu mauPhieu = new RpChiTietMauPhieu();

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_GetByID", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@ChiTietMauPhieuId", id);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            mauPhieu = new RpChiTietMauPhieu
//                            {
//                                ChiTietMauPhieuId = reader.GetInt32(reader.GetOrdinal("ChiTietMauPhieuId")),
//                                MauPhieuId = reader.GetInt32(reader.GetOrdinal("MauPhieuId")),
//                                TieuChiIDs = reader.GetString(reader.GetOrdinal("TieuChiIDs")),
//                                ChitieuID = reader.GetInt32(reader.GetOrdinal("ChitieuIDs")),
//                                NoiDung = reader.GetString(reader.GetOrdinal("NoiDung")),
//                                GopCot = reader.GetInt32(reader.GetOrdinal("GopCot")),
//                                GoptuCot = reader.GetInt32(reader.GetOrdinal("GoptuCot")),
//                                GopDenCot = reader.GetInt32(reader.GetOrdinal("GopDenCot")),
//                                SoCotGop = reader.GetInt32(reader.GetOrdinal("SoCotGop")),
//                                GhiChu = reader.GetString(reader.GetOrdinal("GhiCchu"))
//                            };
//                        }
//                    }
//                }
//            }

//            return mauPhieu;
//        }

//        public async Task<int> Insert(RpChiTietMauPhieu obj)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_Insert", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@MauPhieuId", obj.MauPhieuId);
//                    command.Parameters.AddWithValue("@TieuChiIDs", obj.TieuChiIDs);
//                    command.Parameters.AddWithValue("@ChitieuIDs", obj.ChitieuID);
//                    command.Parameters.AddWithValue("@NoiDung", obj.NoiDung);
//                    command.Parameters.AddWithValue("@GopCot", obj.GopCot);
//                    command.Parameters.AddWithValue("@GoptuCot", obj.GoptuCot);
//                    command.Parameters.AddWithValue("@GopDenCot", obj.GopDenCot);
//                    command.Parameters.AddWithValue("@SoCotGop", obj.SoCotGop);
//                    command.Parameters.AddWithValue("@GhiChu", obj.GhiChu);

//                    return await command.ExecuteNonQueryAsync();
//                }
//            }
//        }

//        public async Task<int> Update(RpChiTietMauPhieu obj)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_Update", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@ChiTietMauPhieuId", obj.ChiTietMauPhieuId);
//                    command.Parameters.AddWithValue("@MauPhieuId", obj.MauPhieuId);
//                    command.Parameters.AddWithValue("@TieuChiIDs", obj.TieuChiIDs);
//                    command.Parameters.AddWithValue("@ChitieuIDs", obj.ChitieuID);
//                    command.Parameters.AddWithValue("@NoiDung", obj.NoiDung);
//                    command.Parameters.AddWithValue("@GopCot", obj.GopCot);
//                    command.Parameters.AddWithValue("@GoptuCot", obj.GoptuCot);
//                    command.Parameters.AddWithValue("@GopDenCot", obj.GopDenCot);
//                    command.Parameters.AddWithValue("@SoCotGop", obj.SoCotGop);
//                    command.Parameters.AddWithValue("@GhiChu", obj.GhiChu);

//                    return await command.ExecuteNonQueryAsync();
//                }
//            }
//        }

//        public async Task<int> Delete(int id)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_Delete", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@ChiTietMauPhieuId", id);

//                    return await command.ExecuteNonQueryAsync();
//                }
//            }
//        }
//    }
//}
