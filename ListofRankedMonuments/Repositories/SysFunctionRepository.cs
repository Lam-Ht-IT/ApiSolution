using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace QUANLYVANHOA.Repositories
{
    public class SysFunctionRepository : ISysFunctionRepository
    {
        private readonly string _connectionString;

        public SysFunctionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(IEnumerable<SysFunction>, int)> GetAll(string? functionName, int pageNumber, int pageSize)
        {
            var functionList = new List<SysFunction>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("FMS_GetListPaging", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionName", functionName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            functionList.Add(new SysFunction
                            {
                                FunctionID = reader.GetInt32(reader.GetOrdinal("FunctionID")),
                                FunctionName = reader.GetString(reader.GetOrdinal("FunctionName")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            });
                        }

                        await reader.NextResultAsync(); // Di chuyển đến kết quả thứ hai
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
            }

            return (functionList, totalRecords);
        }

        public async Task<SysFunction> GetByID(int functionId)
        {
            SysFunction function = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("FMS_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionID", functionId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            function = new SysFunction
                            {
                                FunctionID = reader.GetInt32(reader.GetOrdinal("FunctionID")),
                                FunctionName = reader.GetString(reader.GetOrdinal("FunctionName")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            };
                        }
                    }
                }
            }

            return function;
        }

        public async Task<int> Create(SysFunctionInsertModel function)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("FMS_Create", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionName", function.FunctionName);
                    command.Parameters.AddWithValue("@Description", function.Description);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(SysFunctionUpdateModel function)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("FMS_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionID", function.FunctionID);
                    command.Parameters.AddWithValue("@FunctionName", function.FunctionName);
                    command.Parameters.AddWithValue("@Description", function.Description);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int functionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("FMS_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionID", functionId);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}