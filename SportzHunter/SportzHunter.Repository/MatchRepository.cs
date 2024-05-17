using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Model.Common;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class MatchRepository : IMatchRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public async Task<PagedList<Match>> GetAllMatchesAsync(Paging paging)
        {
            List<Match> matches = new List<Match>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT m.*, t.\"Name\" AS TournamentName, th.\"Name\" AS TeamHomeName, ta.\"Name\" AS TeamAwayName FROM \"Match\" m " +
                                      $"LEFT JOIN \"Tournament\" t ON m.\"TournamentId\" = t.\"Id\" " +
                                      $"LEFT JOIN \"Team\" th ON m.\"TeamHomeId\" = th.\"Id\" " +
                                      $"LEFT JOIN \"Team\" ta ON m.\"TeamAwayId\" = ta.\"Id\"";
                    connection.Open();
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    int totalcount = 0;
                    while (await reader.ReadAsync())
                    {
                        Match match = new Match
                        {
                            Id = (Guid)reader["Id"],
                            TournamentId = (Guid)reader["TournamentId"],
                            TeamHomeId = (Guid)reader["TeamHomeId"],
                            TeamAwayId = (Guid)reader["TeamAwayId"],
                            Result = (string)reader["Result"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            TournamentName = reader["TournamentName"] != DBNull.Value ? (string)reader["TournamentName"] : null,
                            TeamHomeName = reader["TeamHomeName"] != DBNull.Value ? (string)reader["TeamHomeName"] : null,
                            TeamAwayName = reader["TeamAwayName"] != DBNull.Value ? (string)reader["TeamAwayName"] : null
                        };

                        matches.Add(match);
                    }

                    totalcount = await GetTotalMatchCount();
                    connection.Close();
                    return new PagedList<Match>(matches, paging.PageSize, totalcount);
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<PagedList<Match>> GetAllMatchesByTournamentId(Guid tournamentId, Paging paging)
        {
            List<Match> matches = new List<Match>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT m.*, t.\"Name\" AS TournamentName, th.\"Name\" AS TeamHomeName, ta.\"Name\" AS TeamAwayName FROM \"Match\" m " +
                                      $"LEFT JOIN \"Tournament\" t ON m.\"TournamentId\" = t.\"Id\" " +
                                      $"LEFT JOIN \"Team\" th ON m.\"TeamHomeId\" = th.\"Id\" " +
                                      $"LEFT JOIN \"Team\" ta ON m.\"TeamAwayId\" = ta.\"Id\" " +
                                      $"WHERE m.\"TournamentId\" = @TournamentId";
                    cmd.Parameters.AddWithValue("TournamentId", tournamentId);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    int totalcount = 0;
                    while (await reader.ReadAsync())
                    {
                        Match match = new Match
                        {
                            Id = (Guid)reader["Id"],
                            TournamentId = (Guid)reader["TournamentId"],
                            TeamHomeId = (Guid)reader["TeamHomeId"],
                            TeamAwayId = (Guid)reader["TeamAwayId"],
                            Result = (string)reader["Result"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            TournamentName = reader["TournamentName"] != DBNull.Value ? (string)reader["TournamentName"] : null,
                            TeamHomeName = reader["TeamHomeName"] != DBNull.Value ? (string)reader["TeamHomeName"] : null,
                            TeamAwayName = reader["TeamAwayName"] != DBNull.Value ? (string)reader["TeamAwayName"] : null
                        };

                        matches.Add(match);
                    }

                    totalcount = await GetTotalMatchCountByTournamentId(tournamentId);
                    connection.Close();
                    return new PagedList<Match>(matches, paging.PageSize, totalcount);
                }
            }
            catch
            {
                return null;
            }
        }

        private async Task<int> GetTotalMatchCountByTournamentId(Guid tournamentId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT COUNT(*) FROM \"Match\" WHERE \"TournamentId\" = @TournamentId";
                    cmd.Parameters.AddWithValue("TournamentId", tournamentId);
                    int count = (int)await cmd.ExecuteScalarAsync();
                    return count;
                }
            }
            catch
            {
                return 0;
            }
        }

        public async Task<Match> GetMatchByIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    await connection.OpenAsync();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"SELECT m.*, t.\"Name\" AS TournamentName, th.\"Name\" AS TeamHomeName, ta.\"Name\" AS TeamAwayName FROM \"Match\" m " +
                                      $"LEFT JOIN \"Tournament\" t ON m.\"TournamentId\" = t.\"Id\" " +
                                      $"LEFT JOIN \"Team\" th ON m.\"TeamHomeId\" = th.\"Id\" " +
                                      $"LEFT JOIN \"Team\" ta ON m.\"TeamAwayId\" = ta.\"Id\" " +
                                      $"WHERE m.\"Id\" = @Id";
                    cmd.Parameters.AddWithValue("Id", id);
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        Match match = new Match
                        {
                            Id = (Guid)reader["Id"],
                            TournamentId = (Guid)reader["TournamentId"],
                            TeamHomeId = (Guid)reader["TeamHomeId"],
                            TeamAwayId = (Guid)reader["TeamAwayId"],
                            Result = (string)reader["Result"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            TournamentName = reader["TournamentName"] != DBNull.Value ? (string)reader["TournamentName"] : null,
                            TeamHomeName = reader["TeamHomeName"] != DBNull.Value ? (string)reader["TeamHomeName"] : null,
                            TeamAwayName = reader["TeamAwayName"] != DBNull.Value ? (string)reader["TeamAwayName"] : null
                        };

                        await connection.CloseAsync();
                        return match;
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


        public async Task<bool> CreateMatchAsync(Match match)
        {

            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    connection.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = $"INSERT INTO \"Match\" (\"Id\", \"TournamentId\", \"TeamAwayId\", \"TeamHomeId\", \"Result\", \"DateCreated\", \"DateUpdated\", \"IsActive\") VALUES (@Id, @TournamentId, @TeamAwayId, @TeamHomeId, @Result, @DateCreated, @DateUpdated, @IsActive)";
                    cmd.Parameters.AddWithValue("Id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("TournamentId", match.TournamentId);
                    cmd.Parameters.AddWithValue("TeamHomeId", match.TeamHomeId);
                    cmd.Parameters.AddWithValue("TeamAwayId", match.TeamAwayId);
                    cmd.Parameters.AddWithValue("Result", match.Result);
                    cmd.Parameters.AddWithValue("DateCreated", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("DateUpdated", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("IsActive", match.IsActive);


                    var count = await cmd.ExecuteNonQueryAsync();
                    if (count > 0)
                    {
                        await connection.CloseAsync();
                        return true;
                    }
                    else
                    {
                        await connection.CloseAsync();
                        return false;
                    }

                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateMatchAsync(Guid id, Match match)
        {

            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    await connection.OpenAsync();
                    NpgsqlCommand selectcmd = new NpgsqlCommand();
                    selectcmd.Connection = connection;
                    selectcmd.CommandText = $"SELECT COUNT(*) FROM \"Match\" WHERE \"Id\" = @Id";
                    selectcmd.Parameters.AddWithValue("Id", id);
                    int count = Convert.ToInt32(selectcmd.ExecuteScalar());
                    if (count > 0)
                    {
                        NpgsqlCommand updatecmd = new NpgsqlCommand();
                        updatecmd.Connection = connection;
                        updatecmd.CommandText = $"UPDATE \"Match\" SET \"TournamentId\" = @TournamentId, \"TeamAwayId\" = @TeamAwayId, \"TeamHomeId\" = @TeamHomeId, \"Result\" = @Result, \"DateUpdated\" = @DateUpdated, \"IsActive\" = @IsActive WHERE \"Id\" = @Id";
                        updatecmd.Parameters.AddWithValue("Id", id);
                        updatecmd.Parameters.AddWithValue("TournamentId", match.TournamentId);
                        updatecmd.Parameters.AddWithValue("TeamAwayId", match.TeamAwayId);
                        updatecmd.Parameters.AddWithValue("TeamHomeId", match.TeamHomeId);
                        updatecmd.Parameters.AddWithValue("Result", match.Result);
                        
                        updatecmd.Parameters.AddWithValue("DateUpdated", DateTime.UtcNow);
                        updatecmd.Parameters.AddWithValue("IsActive", match.IsActive);
                        updatecmd.ExecuteNonQuery();
                        await connection.CloseAsync();
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
        public async Task<bool> DeleteMatchAsync(Guid id)
        {

            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            try
            {
                using (connection)
                {
                    await connection.OpenAsync();
                    NpgsqlCommand selectcmd = new NpgsqlCommand();
                    selectcmd.Connection = connection;
                    selectcmd.CommandText = $"SELECT COUNT(*) FROM \"Match\" WHERE \"Id\" = @Id";
                    selectcmd.Parameters.AddWithValue("Id", id);
                    int count = Convert.ToInt32(await selectcmd.ExecuteScalarAsync());
                    if (count > 0)
                    {
                        NpgsqlCommand deletecmd = new NpgsqlCommand();
                        deletecmd.Connection = connection;
                        deletecmd.CommandText = $"DELETE FROM \"Match\" WHERE \"Id\" = @Id";
                        deletecmd.Parameters.AddWithValue("Id", id);
                        await deletecmd.ExecuteNonQueryAsync();
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

        private async Task<int> GetTotalMatchCount()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder querryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"Match\"");
               
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
    }
}
