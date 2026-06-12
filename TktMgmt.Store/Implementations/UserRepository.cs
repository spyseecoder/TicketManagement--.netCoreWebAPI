using System.Data;
using Microsoft.Data.SqlClient;
using TktMgmt.Common.DTOs.User;
using Microsoft.Extensions.Configuration;
using TktMgmt.Common.Constants;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<(string, string)> CreateUser(UserCreateDto dto, string passwordHash)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(SqlConstants.CreateUser, connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FirstName", dto.FirstName);
            command.Parameters.AddWithValue("@LastName", dto.LastName);
            command.Parameters.AddWithValue("@Email", dto.Email);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);

            await connection.OpenAsync();

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            string userGuid = string.Empty;
            string message = string.Empty;

            if (await reader.ReadAsync())
            {
                userGuid = reader["UserGuid"]?.ToString() ?? "";
                message = reader["Message"]?.ToString() ?? "";
            }

            return (userGuid, message);

        }
        catch (Exception ex)
        {
            await LogErrorAsync(ex, "CreateUser");
            throw;
        }
    }

    public async Task<string> UpdateUserRole(string userGuid, string roleName)
    {
        try
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(SqlConstants.UpdateUserRole, con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserGuid", userGuid);
            cmd.Parameters.AddWithValue("@RoleName", roleName);

            await con.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return result?.ToString() ?? "Error updating role";
        }
        catch (Exception ex)
        {
            await LogErrorAsync(ex, "UpdateUserRole");
            throw;
        }
    }

    public async Task<List<UserDetailsDto>> GetUserDetails(string? roleName)
    {
        try
        {
            List<UserDetailsDto> list = new();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(SqlConstants.GetUserDetails, con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleName", (object?)roleName ?? DBNull.Value);

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UserDetailsDto
                {
                    UserGuid = reader["UserGuid"].ToString()!,
                    FullName = reader["FullName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    RoleName = reader["RoleName"].ToString()!
                });
            }

            return list;
        }
        catch (Exception ex)
        {
            await LogErrorAsync(ex, "GetUserDetails");
            throw;
        }
    }

    public async Task<string> DeleteUser(string userGuid)
    {
        try
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(SqlConstants.DeleteUser, con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserGuid", userGuid);

            await con.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return result?.ToString() ?? "Error";
        }
        catch (Exception ex)
        {
            await LogErrorAsync(ex, "DeleteUser");
            throw;
        }
    }

    public async Task<UserDetailsDto?> GetUserByGuid(string userGuid)
    {
        try
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(SqlConstants.GetUserByGuid, con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserGuid", userGuid);

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new UserDetailsDto
                {
                    UserGuid = reader["UserGuid"].ToString()!,
                    FullName = reader["FullName"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    RoleName = reader["RoleName"].ToString()!
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            await LogErrorAsync(ex, "GetUserByGuid");
            throw;
        }
    }
    private async Task LogErrorAsync(Exception ex, string source)
    {
        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand(SqlConstants.LogError, con);

        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Message", ex.Message);
        cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace ?? "");
        cmd.Parameters.AddWithValue("@Source", source);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}