using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TktMgmt.Store.Abstractions;
using TktMgmt.Common.Constants;


namespace TktMgmt.Store.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(int, string, string, string, string)> GetUserByEmail(string email)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                using SqlCommand command = new SqlCommand(SqlConstants.Login, connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);

                await connection.OpenAsync();

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return (
                        Convert.ToInt32(reader["UserId"]),
                        reader["UserGuid"].ToString()!,
                        reader["Email"].ToString()!,
                        reader["PasswordHash"].ToString()!,
                        reader["RoleName"].ToString()!
    );
                }

                return (0, "", "", "", "");
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "Login");
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


}
