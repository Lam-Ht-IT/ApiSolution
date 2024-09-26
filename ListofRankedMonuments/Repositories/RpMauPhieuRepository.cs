//using QUANLYVANHOA.Interfaces;
//using QUANLYVANHOA.Controllers;
//using System.Data.SqlClient;
//using System.Data;
//using QUANLYVANHOA.Models;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace QUANLYVANHOA.Repositories
//{
//    public class RpMauPhieuRepository : IRpMauPhieuRepository
//    {
//        private readonly string _connectionString;

//        public RpMauPhieuRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task<(IEnumerable<RpMauPhieu>, int)> GetAll(string name)
//        {
//            var mauPhieuList = new List<RpMauPhieu>();
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
//                            mauPhieuList.Add(new RpMauPhieu
//                            {
//                                MauPhieuId = reader.GetInt32(reader.GetOrdinal("MauPhieuId")),
//                                TenMauPhieu = reader.GetString(reader.GetOrdinal("TenMauPhieu")),
//                                MaMauPhieu = reader.GetString(reader.GetOrdinal("MaMauPhieu")),
//                                LoaiMauPhieuId = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuId")),
//                                // If NgayTao is null, use DateTime.Now
//                                Ngaytao = !reader.IsDBNull(reader.GetOrdinal("NgayTao"))
//                                    ? reader.GetDateTime(reader.GetOrdinal("NgayTao")).ToString("yyyy-MM-dd HH:mm:ss")
//                                    : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                                NguoiTao = reader.GetString(reader.GetOrdinal("NguoiTao"))
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


//        public async Task<RpMauPhieu> GetByID(int id)
//        {
//            RpMauPhieu mauPhieu = null;

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_GetByID", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@MauPhieuID", id);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        if (await reader.ReadAsync())
//                        {
//                            mauPhieu = new RpMauPhieu
//                            {
//                                MauPhieuId = reader.GetInt32(reader.GetOrdinal("MauPhieuID")),
//                                TenMauPhieu = reader.GetString(reader.GetOrdinal("TenMauPhieu")),
//                                MaMauPhieu = reader.GetString(reader.GetOrdinal("MaMauPhieu")),
//                                LoaiMauPhieuId = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuId")),
//                                // If NgayTao is null, use DateTime.Now
//                                Ngaytao = !reader.IsDBNull(reader.GetOrdinal("NgayTao"))
//                                    ? reader.GetDateTime(reader.GetOrdinal("NgayTao")).ToString("yyyy-MM-dd HH:mm:ss")
//                                    : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
//                                NguoiTao = reader.GetString(reader.GetOrdinal("NguoiTao"))
//                            };
//                        }
//                    }
//                }
//            }

//            return mauPhieu;
//        }

//        public async Task<int> Insert(RpMauPhieu obj)
//        {
//            int result = 0;

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_Insert", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@TenMauPhieu", obj.TenMauPhieu);
//                    command.Parameters.AddWithValue("@MaMauPhieu", obj.MaMauPhieu);
//                    command.Parameters.AddWithValue("@LoaiMauPhieuId", obj.LoaiMauPhieuId);
//                    command.Parameters.AddWithValue("@NgayTao", obj.Ngaytao);
//                    command.Parameters.AddWithValue("@NguoiTao", obj.NguoiTao);

//                    result = await command.ExecuteNonQueryAsync();
//                }
//            }

//            return result;
//        }

//        public async Task<int> Update(RpMauPhieu obj)
//        {
//            int result = 0;

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_Update", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@MauPhieuID", obj.MauPhieuId);
//                    command.Parameters.AddWithValue("@TenMauPhieu", obj.TenMauPhieu);
//                    command.Parameters.AddWithValue("@MaMauPhieu", obj.MaMauPhieu);
//                    command.Parameters.AddWithValue("@LoaiMauPhieuId", obj.LoaiMauPhieuId);
//                    command.Parameters.AddWithValue("@NgayTao", obj.Ngaytao);
//                    command.Parameters.AddWithValue("@NguoiTao", obj.NguoiTao);

//                    result = await command.ExecuteNonQueryAsync();
//                }
//            }

//            return result;
//        }

//        public async Task<int> Delete(int id)
//        {
//            int result = 0;

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("MP_Delete", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@MauPhieuID", id);

//                    result = await command.ExecuteNonQueryAsync();
//                }
//            }

//            return result;
//        }
//    }
//}
