using Npgsql;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class TeamLeaderRepository : ITeamLeaderRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        protected ITeamRepository TeamRepository;
        public TeamLeaderRepository(ITeamRepository teamRepository)
        {
            TeamRepository = teamRepository;
        }
        public async Task<int> KickPlayerFromTeamAsync(Guid playerId, Guid kickId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE \"Player\" SET \"TeamId\" = NULL, \"DateUpdated\" = @dateUpdated, \"UpdatedBy\" = @updatedBy WHERE \"Id\" = @id";
                command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                command.Parameters.AddWithValue("@updatedBy", kickId);
                command.Parameters.AddWithValue("@id", playerId);
                connection.Open();
                if (await command.ExecuteNonQueryAsync() >= 0)
                {
                    connection.Close();
                    return 1;
                }
                connection.Close();
                return 0;
            }
        }

        public async Task<TeamLeader> GetTeamLeaderByUserId(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT \"TeamLeader\".*, \"User\".* FROM \"TeamLeader\" LEFT JOIN \"User\" ON \"User\".\"Id\" = \"TeamLeader\".\"UserId\" WHERE \"TeamLeader\".\"UserId\" = @id AND \"TeamLeader\".\"IsActive\" = True";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                TeamLeader teamleaderToReturn = null;
                while (await reader.ReadAsync())
                {
                    teamleaderToReturn = new TeamLeader
                    {
                        Id = (Guid)reader["Id"],
                        UserId = (Guid)reader["UserId"],
                        Team = await TeamRepository.GetTeamByTeamLeaderAsync((Guid)reader["Id"]),
                        SportCategoryId = (Guid)reader["SportCategoryId"],
                        Description = (reader["Description"] as string) ?? reader["Description"]?.ToString(),
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"],
                        IsAccepted = (bool)reader["IsAccepted"]
                    };
                }
                connection.Close();
                return teamleaderToReturn;
            }
        }

        public async Task<List<TeamLeader>> GetTeamLeaderListAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    List<TeamLeader> teamLeaders = new List<TeamLeader>();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT tl.\"Id\" AS TeamLeaderId, tl.*, u.\"Username\" FROM  \"TeamLeader\" tl INNER JOIN \"User\" u ON tl.\"UserId\" = u.\"Id\"";
                    connection.Open();
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (!reader.HasRows)
                    {
                        connection.Close();
                        return null;
                    }

                    while(await reader.ReadAsync())
                    {
                        TeamLeader teamLeader = new TeamLeader
                        {
                            Id = (Guid)reader["TeamLeaderId"],
                            UserId = (Guid)reader["UserId"],
                            SportCategoryId = (Guid)reader["SportCategoryId"],
                            Description = (reader["Description"] as string) ?? reader["Description"]?.ToString(),
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            IsAccepted = (bool)reader["IsAccepted"],
                            Username = reader["Username"] as string
                        };
                        teamLeaders.Add(teamLeader);
                    }
                    connection.Close();
                    return teamLeaders;
                }
            }
            catch
            {
               throw;

            }

        }
    }
}
