using QUANLYVANHOA.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<User>, int)> GetAll(string? userName, int pageNumber, int pageSize)
        {
            var userList = new List<User>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Lấy dữ liệu phân trang
                using (var command = new SqlCommand("GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            userList.Add(new User
                            {
                                UserID = reader.GetInt32("UserID"),
                                UserName = reader["UserName"].ToString(),
                                Password = reader["Password"].ToString(),
                                RoleID = reader.GetInt32("RoleID")
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

            return (userList, totalRecords);
        }

        public async Task<User> GetByID(int userId)
        {
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("GetUserByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserID = reader.GetInt32("UserID"),
                                UserName = reader["UserName"].ToString(),
                                Password = reader["Password"].ToString(),
                                RoleID = reader.GetInt32("RoleID")
                            };
                        }
                    }
                }
            }

            return user;
        }

        public async Task<int> Insert(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("InsertUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("UpdateUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@RoleID", user.RoleID);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("DeleteUser", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<User> VerifyLoginAsync(string userName, string password)
        {
            User user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("VerifyLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserID = reader.GetInt32("UserID"),
                                UserName = reader["UserName"].ToString(),
                                Password = reader["Password"].ToString(),
                                RoleID = reader.GetInt32("RoleID")
                            };
                        }
                    }
                }
            }

            return user;
        }

    }
}
