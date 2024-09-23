using QUANLYVANHOA.Interfaces;
using QUANLYVANHOA.Models;
using System.Data;
using System.Data.SqlClient;

public class SysUserInGroupRepository : ISysUserInGroupRepository
{
    private readonly string _connectionString;

    public SysUserInGroupRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<SysUserInGroup>> GetAll()
    {
        var userInGroupList = new List<SysUserInGroup>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_GetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userInGroupList.Add(new SysUserInGroup
                        {
                            UserInGroupID = reader.GetInt32(reader.GetOrdinal("UserInGroupID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            GroupID = reader.GetInt32(reader.GetOrdinal("GroupID"))
                        });
                    }
                }
            }
        }

        return userInGroupList;
    }

    public async Task<IEnumerable<SysUserInGroup>> GetByGroupID(int groupID)
    {
        var userInGroupList = new List<SysUserInGroup>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_GetByGroupID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@GroupID", groupID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userInGroupList.Add(new SysUserInGroup
                        {
                            UserInGroupID = reader.GetInt32(reader.GetOrdinal("UserInGroupID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            GroupID = reader.GetInt32(reader.GetOrdinal("GroupID"))
                        });
                    }
                }
            }
        }

        return userInGroupList;
    }

    public async Task<IEnumerable<SysUserInGroup>> GetByUserID(int userID)
    {
        var userInGroupList = new List<SysUserInGroup>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_GetByUserID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userInGroupList.Add(new SysUserInGroup
                        {
                            UserInGroupID = reader.GetInt32(reader.GetOrdinal("UserInGroupID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            GroupID = reader.GetInt32(reader.GetOrdinal("GroupID"))
                        });
                    }
                }
            }
        }

        return userInGroupList;
    }

    public async Task<SysUserInGroup> GetByID(int userInGroupID)
    {
        SysUserInGroup userInGroup = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_GetByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserInGroupID", userInGroupID);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userInGroup = new SysUserInGroup
                        {
                            UserInGroupID = reader.GetInt32(reader.GetOrdinal("UserInGroupID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            GroupID = reader.GetInt32(reader.GetOrdinal("GroupID"))
                        };
                    }
                }
            }
        }

        return userInGroup;
    }

    public async Task<int> Create(SysUserInGroupCreateModel userInGroup)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_Create", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userInGroup.UserID);
                command.Parameters.AddWithValue("@GroupID", userInGroup.GroupID);
                return await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<int> Update(SysUserInGroupUpdateModel userInGroup)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserInGroupID", userInGroup.UserInGroupID);
                command.Parameters.AddWithValue("@UserID", userInGroup.UserID);
                command.Parameters.AddWithValue("@GroupID", userInGroup.GroupID);

                return await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<int> Delete(int userInGroupID)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UIG_Delete", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserInGroupID", userInGroupID);

                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}