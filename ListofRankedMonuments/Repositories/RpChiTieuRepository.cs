//using QUANLYVANHOA.Interfaces;
//using QUANLYVANHOA.Models;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//namespace QUANLYVANHOA.Repositories
//{
//    public class RpChiTieuRepository
//    {
//        private readonly string _connectionString;

//        public RpChiTieuRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task<(IEnumerable<RpChiTieu>, int)> GetAll()
//        {
//            var chiTieuList = new List<RpChiTieu>();
//            int totalRecords = 0;

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("BCCT_GetAll", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            chiTieuList.Add(new RpChiTieu
//                            {
//                                ChiTieuId = reader.GetInt32(reader.GetOrdinal("ChiTieuId")),
//                                TenChiTieu = reader.GetString(reader.GetOrdinal("TenChiTieu")),
//                                MaChiTieu = reader.GetString(reader.GetOrdinal("MaChiTieu")),
//                                LoaiChiTieuId = reader.GetInt32(reader.GetOrdinal("LoaiChiTieuId")),
//                                NguoiTao = reader.IsDBNull(reader.GetOrdinal("NguoiTao")) ? null : reader.GetString(reader.GetOrdinal("NguoiTao"))
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

//            return (chiTieuList, totalRecords);
//        }

//        public async Task<RpChiTieu> GetByID(int id)
//        {
//            RpChiTieu chiTieu = new RpChiTieu();

//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();

//                using (var command = new SqlCommand("CT_GetByID", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@ChiTieuId", id);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        if (await reader.ReadAsync())
//                        {
//                            chiTieu.ChiTieuId = reader.GetInt32(reader.GetOrdinal("ChiTieuId"));
//                            chiTieu.TenChiTieu = reader.GetString(reader.GetOrdinal("TenChiTieu"));
//                            chiTieu.MaChiTieu = reader.GetString(reader.GetOrdinal("MaChiTieu"));
//                            chiTieu.LoaiChiTieuId = reader.GetInt32(reader
//    }
//}
