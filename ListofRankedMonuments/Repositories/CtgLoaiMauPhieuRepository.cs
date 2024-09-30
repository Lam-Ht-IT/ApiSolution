using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Controllers;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QUANLYVANHOA.Repositories
{
    public class CtgLoaiMauPhieuRepository : ICtgLoaiMauPhieuRepository
    {
        private readonly string _connectionString;

        public CtgLoaiMauPhieuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<CtgLoaiMauPhieu>, int)> GetAll(string name, int pageNumber, int pageSize)
        {
            var loaiMauPhieuList = new List<CtgLoaiMauPhieu>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("LMP_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TenLoaiMauPhieu", name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            loaiMauPhieuList.Add(new CtgLoaiMauPhieu
                            {
                                LoaiMauPhieuID = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuID")),
                                TenLoaiMauPhieu = reader.GetString(reader.GetOrdinal("TenLoaiMauPhieu")),
                                MaLoaiMauPhieu = reader.GetString(reader.GetOrdinal("MaLoaiMauPhieu")),
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

            return (loaiMauPhieuList, totalRecords);
        }

        public async Task<CtgLoaiMauPhieu> GetByID(int id)
        {
            CtgLoaiMauPhieu loaiMauPhieu = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LMP_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoaiMauPhieuID", id);
                    await connection.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            loaiMauPhieu = new CtgLoaiMauPhieu
                            {
                                LoaiMauPhieuID = reader.GetInt32(reader.GetOrdinal("LoaiMauPhieuID")),
                                TenLoaiMauPhieu = reader.GetString(reader.GetOrdinal("TenLoaiMauPhieu")),
                                MaLoaiMauPhieu = reader.GetString(reader.GetOrdinal("MaLoaiMauPhieu")),
                                TrangThai = reader.GetBoolean(reader.GetOrdinal("TrangThai")),
                                GhiChu = reader.GetString(reader.GetOrdinal("GhiChu")),
                                Loai = reader.GetInt32(reader.GetOrdinal("Loai"))
                            };
                        }
                    }
                }
            }

            return loaiMauPhieu;
        }

        public async Task<int> Insert(CtgLoaiMauPhieuModelInsert loaiMauPhieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LMP_Insert", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenLoaiMauPhieu", loaiMauPhieu.TenLoaiMauPhieu);
                    cmd.Parameters.AddWithValue("@MaLoaiMauPhieu", loaiMauPhieu.MaLoaiMauPhieu);
                    cmd.Parameters.AddWithValue("@GhiChu", loaiMauPhieu.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(CtgLoaiMauPhieuModelUpdate loaiMauPhieu)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LMP_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoaiMauPhieuID", loaiMauPhieu.LoaiMauPhieuID);
                    cmd.Parameters.AddWithValue("@TenLoaiMauPhieu", loaiMauPhieu.TenLoaiMauPhieu);
                    cmd.Parameters.AddWithValue("@MaLoaiMauPhieu", loaiMauPhieu.MaLoaiMauPhieu);
                    cmd.Parameters.AddWithValue("@GhiChu", loaiMauPhieu.GhiChu);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("LMP_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LoaiMauPhieuID", id);

                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
