﻿using QUANLYVANHOA.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using QUANLYVANHOA.Controllers;

namespace QUANLYVANHOA.Repositories
{
    public class SysUserRepository : ISysUserRepository
    {
        private readonly string _connectionString;

        public SysUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<SysUser>, int)> GetAll(string? userName, int pageNumber, int pageSize)
        {
            var userList = new List<SysUser>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("UMS_GetListPaging", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Đọc dữ liệu người dùng
                        while (await reader.ReadAsync())
                        {
                            userList.Add(new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = reader.GetString(reader.GetOrdinal("Note"))
                            });
                        }

                        // Đọc tổng số bản ghi từ truy vấn riêng biệt
                        await reader.NextResultAsync(); // Di chuyển đến kết quả thứ hai
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
            }

            return (userList, totalRecords);
        }

        public async Task<SysUser> GetByID(int userId)
        {
            SysUser user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("UMS_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = reader.GetString(reader.GetOrdinal("Note"))
                            };
                        }
                    }
                }
            }

            return user;
        }

        public async Task<int> Create(SysUser user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("UMS_Create", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Status", user.Status);
                    cmd.Parameters.AddWithValue("@Note", user.Note);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(SysUser user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("UMS_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Status", user.Status);
                    cmd.Parameters.AddWithValue("@Note", user.Note);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("UMS_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<SysUser> GetByRefreshToken(string refreshToken)
        {
            SysUser user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetUserByRefreshToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                RefreshToken = reader.GetString(reader.GetOrdinal("RefreshToken")),
                                RefreshTokenExpiryTime = reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiryTime"))
                            };
                        }
                    }
                }
            }
            return user;
        }

        public async Task<SysUser> VerifyLogin(string userName, string password)
        {
            SysUser user = null;

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
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = reader.GetString(reader.GetOrdinal("Note"))
                            };
                        }
                    }
                }
            }

            return user;
        }
    }
}
