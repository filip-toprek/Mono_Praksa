using Npgsql;
using SportzHunter.Repository.Common;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class InviteStatusRepository : IInviteStatusRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<Guid> GetInviteStatus(string status)
        {
            Guid returnValue = Guid.Empty;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string inviteStatusQuery = "Select \"Id\" FROM \"InviteStatus\" WHERE \"StatusName\" = @status";
                NpgsqlCommand command = new NpgsqlCommand(inviteStatusQuery, connection);

                command.Parameters.AddWithValue("@status", status);
                connection.Open();

                NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read() && reader.HasRows)
                {
                    returnValue = (Guid)reader["Id"];

                }

                return returnValue;
            }
        }
    }
}
