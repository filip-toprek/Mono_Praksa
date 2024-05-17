using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;


        public async Task<PagedList<Team>> GetAllTeamsAsync(TeamFiltering filtering, Sorting sorting, Paging paging)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    List<Team> teams = new List<Team>();
                    int totalCount = 0;

                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;

                    StringBuilder querybuilder = new StringBuilder($"SELECT t.*, u.\"Username\" AS TeamLeaderName FROM \"Team\" t");
                    querybuilder.Append($" LEFT JOIN \"TeamLeader\" tl ON t.\"TeamLeaderId\" = tl.\"Id\"");
                    querybuilder.Append($" LEFT JOIN \"User\" u ON tl.\"UserId\" = u.\"Id\"");
                    querybuilder.Append(" WHERE t.\"IsActive\" = true");

                    SetFilters(querybuilder, filtering, cmd);
                    if (!string.IsNullOrEmpty(sorting.SortBy) && paging.PageNumber > 0 && paging.PageSize > 0)
                    {
                        querybuilder.Append($" ORDER BY \"{sorting.SortBy}\" {sorting.SortOrder} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ");
                        cmd.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                        cmd.Parameters.AddWithValue("@PageSize", paging.PageSize);
                    }
                    

                    connection.Open();
                    cmd.CommandText = querybuilder.ToString();

                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                      
                            Team team = new Team
                            {
                                Id = (Guid)reader["Id"],
                                TeamLeaderId = (Guid)reader["TeamLeaderId"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"],
                                TeamLeaderName = reader["TeamLeaderName"] != DBNull.Value ? (string)reader["TeamLeaderName"] : null
                            };

                            teams.Add(team);
                        
                    }

                    connection.Close();
                    totalCount = await GetTotalTeamCount(filtering);
                    return new PagedList<Team>(teams, paging.PageSize, totalCount);
                }
            }
            catch
            {
                return null;
            }
        }


        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT t.*, u.\"Username\" AS TeamLeaderName FROM \"Team\" t LEFT JOIN \"TeamLeader\" tl ON t.\"TeamLeaderId\" = tl.\"Id\" LEFT JOIN \"User\" u ON tl.\"UserId\" = u.\"Id\" WHERE t.\"Id\" = @Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        Team team = new Team
                        {
                            Id = id,
                            TeamLeaderId = (Guid)reader["TeamLeaderId"],
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            TeamLeaderName = reader["TeamLeaderName"] != DBNull.Value ? (string)reader["TeamLeaderName"] : null,
                            PlayerList = new List<Player>()
                        };

                        connection.Close();

                        
                        await AddPlayersToTeamAsync(team);
                      

                        return team;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        private async Task AddPlayersToTeamAsync(Team team)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT p.*, u.\"Username\" AS \"PlayerUsername\" FROM \"Player\" p LEFT JOIN \"User\" u ON p.\"UserId\" = u.\"Id\" WHERE p.\"TeamId\" = @TeamId";
                    cmd.Parameters.AddWithValue("TeamId", team.Id);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Guid userId = (Guid)reader["UserId"];
                        

                       
                       

                        Player player = new Player
                        {
                            Id = (Guid)reader["Id"],
                            UserId = userId,
                            Username = (string)reader["PlayerUsername"]
                        };
                        team.PlayerList.Add(player);
                    }
                }
            }
            catch 
            {
               
            }
            finally
            {
                connection.Close();
            }
        }




        public async Task<bool> CreateTeamAsync(Team team, Guid UserId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    await connection.OpenAsync();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"INSERT INTO \"Team\" (\"Id\", \"TeamLeaderId\", \"Name\", \"Description\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") VALUES (@Id, @TeamLeaderId, @Name, @Description, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";
                    cmd.Parameters.AddWithValue("Id", team.Id);
                    cmd.Parameters.AddWithValue("TeamLeaderId", team.TeamLeaderId);
                    cmd.Parameters.AddWithValue("Name", team.Name);
                    cmd.Parameters.AddWithValue("Description", team.Description);
                    cmd.Parameters.AddWithValue("CreatedBy", UserId);
                    cmd.Parameters.AddWithValue("UpdatedBy", UserId);
                    cmd.Parameters.AddWithValue("DateCreated", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("DateUpdated",DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("IsActive", team.IsActive);
                    var count = await cmd.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                    if (count > 0)
                    {
                        return true;
                    }
                    else { return false; }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTeamAsync(Guid id, Team team, Guid UserId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    await connection.OpenAsync();
                    NpgsqlCommand selectcmd = new NpgsqlCommand();
                    selectcmd.Connection = connection;
                    selectcmd.CommandText = $"SELECT COUNT(*) FROM \"Team\" WHERE \"Id\" = @Id";
                    selectcmd.Parameters.AddWithValue("@Id", id);
                    int count = Convert.ToInt32(selectcmd.ExecuteScalar());
                    if (count > 0)
                    {
                        NpgsqlCommand updatecmd = new NpgsqlCommand();
                        updatecmd.Connection = connection;
                        updatecmd.CommandText = $"UPDATE \"Team\" SET \"Name\" = @Name, \"Description\" = @Description, \"UpdatedBy\" = @UpdatedBy,  \"DateUpdated\" = @DateUpdated WHERE \"Id\" = @Id";
                       
                        updatecmd.Parameters.AddWithValue("Name", team.Name);
                        updatecmd.Parameters.AddWithValue("Description", team.Description);
                        
                        updatecmd.Parameters.AddWithValue("UpdatedBy", UserId);
                        
                        updatecmd.Parameters.AddWithValue("DateUpdated", DateTime.UtcNow);
                        
                        updatecmd.Parameters.AddWithValue("Id", id);
                        await updatecmd.ExecuteNonQueryAsync();
                        await connection.CloseAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTeamAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            NpgsqlTransaction transaction = null;
            try
            {
                using (connection)
                {
                    await connection.OpenAsync();
                    transaction = connection.BeginTransaction();

                    
                    NpgsqlCommand selectTeamLeaderUserIdCmd = new NpgsqlCommand();
                    selectTeamLeaderUserIdCmd.Connection = connection;
                    selectTeamLeaderUserIdCmd.Transaction = transaction;
                    selectTeamLeaderUserIdCmd.CommandText = $"SELECT \"TeamLeaderId\" FROM \"Team\" WHERE \"Id\" = @Id";
                    selectTeamLeaderUserIdCmd.Parameters.AddWithValue("Id", id);
                    Guid? teamLeaderId = (Guid?)await selectTeamLeaderUserIdCmd.ExecuteScalarAsync();

                  
                    if (teamLeaderId != null)
                    {
                        NpgsqlCommand deleteTeamLeaderCmd = new NpgsqlCommand();
                        deleteTeamLeaderCmd.Connection = connection;
                        deleteTeamLeaderCmd.Transaction = transaction;
                        deleteTeamLeaderCmd.CommandText = $"UPDATE \"TeamLeader\" SET \"IsActive\" = false WHERE \"Id\" = @TeamLeaderId";
                        deleteTeamLeaderCmd.Parameters.AddWithValue("TeamLeaderId", teamLeaderId);
                        await deleteTeamLeaderCmd.ExecuteNonQueryAsync();
                    }

                   
                    NpgsqlCommand deleteUserCmd = new NpgsqlCommand();
                    deleteUserCmd.Connection = connection;
                    deleteUserCmd.Transaction = transaction;
                    deleteUserCmd.CommandText = $"UPDATE \"User\" SET \"IsActive\" = false WHERE \"Id\" = @UserId";
                    deleteUserCmd.Parameters.AddWithValue("UserId", teamLeaderId);
                    await deleteUserCmd.ExecuteNonQueryAsync();

                   
                    NpgsqlCommand deleteTeamCmd = new NpgsqlCommand();
                    deleteTeamCmd.Connection = connection;
                    deleteTeamCmd.Transaction = transaction;
                    deleteTeamCmd.CommandText = $"UPDATE \"Team\" SET \"IsActive\" = false WHERE \"Id\" = @Id";
                    deleteTeamCmd.Parameters.AddWithValue("Id", id);
                    int rowsAffected = await deleteTeamCmd.ExecuteNonQueryAsync();
                    transaction.Commit();
                    await connection.CloseAsync();
                    
                    return true;
                }
            }
            catch
            {
               
                transaction?.Rollback();
                return false;
            }
        }


        private async Task<int> GetTotalTeamCount(TeamFiltering filtering)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder querryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"Team\" t WHERE \"IsActive\" = true");
                SetFilters(querryBuilder, filtering, command);
                command.CommandText = querryBuilder.ToString();
                connection.Open();
                var result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int count))
                {
                    return count;
                }
                connection.Close();
                return 0;
            }
        }

        public void SetFilters(StringBuilder querybuilder, TeamFiltering filtering, NpgsqlCommand command)
        {
            if (filtering.SportCategoryId != Guid.Empty)
            {
                querybuilder.Append(" AND tl.\"SportCategoryId\" = @SportCategoryId");
                command.Parameters.AddWithValue("@SportCategoryId", filtering.SportCategoryId);
            }

            if (!string.IsNullOrEmpty(filtering.Name))
            {
                querybuilder.Append($" AND LOWER(t.\"Name\") LIKE LOWER(@Name)");
                command.Parameters.AddWithValue("@Name", $"%{filtering.Name}%");
            }

            if (!string.IsNullOrEmpty(filtering.Description))
            {
                querybuilder.Append($" AND LOWER(t.\"Description\") LIKE LOWER(@Description)");
                command.Parameters.AddWithValue("@Description", $"%{filtering.Description}%");
            }

            if (filtering.TeamleaderId.HasValue)
            {
                querybuilder.Append($" AND t.\"TeamLeaderId\" = @TeamLeaderId");
                command.Parameters.AddWithValue("@TeamLeaderId", filtering.TeamleaderId);
            }

            if (filtering.DateCreated.HasValue)
            {
                querybuilder.Append(" AND DATE(\"DateCreated\") = @DateCreated");
                command.Parameters.AddWithValue("@DateCreated", filtering.DateCreated.Value.Date);
            }
        }

        public async Task<Team> GetTeamByTeamLeaderAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT t.*, u.\"Username\" AS TeamLeaderName, tl.*, u.* FROM \"Team\" t LEFT JOIN \"TeamLeader\" tl ON t.\"TeamLeaderId\" = tl.\"Id\" LEFT JOIN \"User\" u ON u.\"Id\" = tl.\"UserId\" WHERE tl.\"Id\" = @Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        Team team = new Team
                        {
                            Id = (Guid)reader["Id"],
                            TeamLeaderId = (Guid)reader["TeamLeaderId"],
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            TeamLeaderName = (string)reader["TeamLeaderName"],
                            PlayerList = new List<Player>(),
                        };

                        await AddPlayersToTeamAsync(team);

                        connection.Close();
                        return team;

                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Guid> GetTeamIdByUserIdAsync(Guid userId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT \"User\".*, \"TeamLeader\".*, \"Team\".*, \"Team\".\"Id\" AS TeamId FROM \"User\" LEFT JOIN \"TeamLeader\" ON \"User\".\"Id\" = \"TeamLeader\".\"UserId\" LEFT JOIN \"Team\" ON \"TeamLeader\".\"Id\" = \"Team\".\"TeamLeaderId\" WHERE \"User\".\"Id\" = @id";
                    cmd.Parameters.AddWithValue("@id", userId);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                    Guid teamId = Guid.Empty;
                    while (await reader.ReadAsync())
                    {
                        teamId = (Guid)reader["TeamId"];
                    }
                    connection.Close();
                    return teamId;
                }
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
            }
    }
}
