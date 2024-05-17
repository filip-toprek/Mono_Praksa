using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static SportzHunter.Common.TournamentFiltering;

namespace SportzHunter.Repository
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<Tournament>> GetTournamentListAsync(TournamentFiltering filtering, Sorting sorting, Paging paging)
        {
            List<Tournament> tournaments = new List<Tournament>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            StringBuilder queryBuilder = new StringBuilder("SELECT * FROM \"Tournament\" WHERE \"IsActive\" = true");
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                FilteringQuery(filtering, queryBuilder, command);
                queryBuilder.Append($" ORDER BY \"{sorting.SortBy}\" {sorting.SortOrder} OFFSET @offset ROWS FETCH NEXT {paging.PageSize} ROWS ONLY");
                command.Parameters.AddWithValue("offset", (paging.PageNumber - 1) * paging.PageSize);
                command.Connection = connection;
                command.CommandText = queryBuilder.ToString();
                using (command)
                {
                    try
                    {
                        connection.Open();
                        using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Tournament tournament = new Tournament
                                    {
                                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                        NumberOfTeams = reader.GetInt32(reader.GetOrdinal("NumberOfTeams")),
                                        Location = reader.GetString(reader.GetOrdinal("Location")),
                                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                        DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"))
                                    };
                                    tournaments.Add(tournament);
                                }
                            }
                            else
                            {
                                Debug.WriteLine("No tournaments available.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred while retrieving tournaments: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            int totalCount = await GetTotalTournamentCount(filtering);
            return new PagedList<Tournament>(tournaments, paging.PageSize, totalCount);
            //return tournaments;
        }


        public async Task<int> AddNewTournamentAsync(Tournament tournament)
        {
            int rowsAffected = 0;
            if (tournament == null)
            {
                return rowsAffected;
            }

            string query = $"INSERT INTO \"Tournament\" (\"Id\", \"Name\", \"Description\", \"NumberOfTeams\", \"Location\", \"Date\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
                           $"VALUES (@Id, @Name, @Description, @NumberOfTeams, @Location, @Date, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            NpgsqlCommand command = new NpgsqlCommand(query, connection);
            using (connection)
            {
                using (command)
                {
                    command.Parameters.AddWithValue("@Id", tournament.Id);
                    command.Parameters.AddWithValue("@Name", tournament.Name);
                    command.Parameters.AddWithValue("@Description", (object)tournament.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NumberOfTeams", tournament.NumberOfTeams);
                    command.Parameters.AddWithValue("@Location", tournament.Location);
                    command.Parameters.AddWithValue("@Date", (DateTime)tournament.Date.ToUniversalTime());
                    command.Parameters.AddWithValue("@CreatedBy", tournament.CreatedBy);
                    command.Parameters.AddWithValue("@UpdatedBy", tournament.UpdatedBy);
                    command.Parameters.AddWithValue("@DateCreated", tournament.DateCreated);
                    command.Parameters.AddWithValue("@DateUpdated", tournament.DateUpdated);
                    command.Parameters.AddWithValue("@IsActive", true);

                    try
                    {
                        connection.Open();
                        rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred while adding tournament: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public async Task<Tournament> GetTournamentByIdAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                Tournament tournament = null;
                string query = "SELECT * FROM \"Tournament\" WHERE \"Id\" = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);

                using (connection)
                {
                    using (command)
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        try
                        {
                            connection.Open();
                            using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (reader.Read())
                                {
                                    tournament = new Tournament
                                    {
                                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                        NumberOfTeams = reader.GetInt32(reader.GetOrdinal("NumberOfTeams")),
                                        Location = reader.GetString(reader.GetOrdinal("Location")),
                                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                        Attendees = await GetAttendees(id)
                                    };
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"An error occurred while fetching tournament: {ex.Message}");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                return tournament;
            }
        }

        public async Task<int> GetAttendees(Guid tournamentId)
        {
            int numberOfTeams = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM \"Attend\" WHERE \"TournamentId\" = @TournamentId AND \"IsApproved\" = true AND \"IsActive\" = true";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@TournamentId", tournamentId);

                try
                {
                    await connection.OpenAsync();

                    numberOfTeams = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while fetching the number of teams: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            return numberOfTeams;
        }

        public async Task<int> UpdateTournamentAsync(Guid id, Tournament tournament)
        {
            int rowsAffected = 0;
            if (tournament == null)
            {
                return rowsAffected;
            }
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        StringBuilder query = new StringBuilder("UPDATE \"Tournament\" SET");

                        if (!string.IsNullOrEmpty(tournament.Name))
                        {
                            query.Append(" \"Name\" = @Name,");
                            command.Parameters.AddWithValue("@Name", tournament.Name);
                        }

                        if (!string.IsNullOrEmpty(tournament.Description))
                        {
                            query.Append(" \"Description\" = @Description,");
                            command.Parameters.AddWithValue("@Description", tournament.Description);
                        }
                        if (tournament.NumberOfTeams != 0)
                        {
                            query.Append(" \"NumberOfTeams\" = @NumberOfTeams,");
                            command.Parameters.AddWithValue("@NumberOfTeams", tournament.NumberOfTeams);

                        }
                        if (!string.IsNullOrEmpty(tournament.Location))
                        {
                            query.Append(" \"Location\" = @Location,");
                            command.Parameters.AddWithValue("@Location", tournament.Location);
                        }
                        if (tournament.Date != default(DateTime))
                        {
                            query.Append(" \"Date\" = @Date,");
                            command.Parameters.AddWithValue("@Date", tournament.Date);
                        }

                        query.Append(" \"DateUpdated\" = @DateUpdated,");
                        command.Parameters.AddWithValue("@DateUpdated", tournament.DateUpdated);
                        query.Append(" \"UpdatedBy\" = @UpdatedBy");
                        command.Parameters.AddWithValue("@UpdatedBy", tournament.UpdatedBy);

                        query.Append(" WHERE \"Id\" = @Id");
                        command.Parameters.AddWithValue("@Id", id);

                        command.CommandText = query.ToString();
                        rowsAffected = await command.ExecuteNonQueryAsync();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the tournament.", ex);

            }
            return rowsAffected;
        }

        public async Task<bool> DeleteByIdTournamentAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Tournament ID cannot be empty.", nameof(id));
            }
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    string query = "UPDATE \"Tournament\" SET \"IsActive\" = @IsActive WHERE \"Id\" = @Id";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsActive", false);
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the tournament.", ex);

            }
            return false;
        }

        public async Task<List<Tournament>> GetTournamentByTeamAsync(Guid teamId, Sorting sorting)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                List<Tournament> tournamentList = new List<Tournament>();
                string query = "SELECT t.*, a.\"TeamId\", a.\"IsApproved\" FROM \"Tournament\" t JOIN \"Attend\" a ON t.\"Id\" = a.\"TournamentId\" WHERE a.\"IsApproved\" = true AND a.\"TeamId\" = @Id";
                query += ($" ORDER BY \"{sorting.SortBy}\" {sorting.SortOrder}");
                NpgsqlCommand command = new NpgsqlCommand(query, connection);

                using (connection)
                {
                    using (command)
                    {
                        command.Parameters.AddWithValue("@Id", teamId);
                        try
                        {
                            connection.Open();
                            using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (!reader.HasRows)
                                {
                                    Debug.WriteLine($"There isn't any rows to read!");
                                }
                                while (await reader.ReadAsync())
                                {
                                    Tournament tournament = new Tournament
                                    {
                                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                        NumberOfTeams = reader.GetInt32(reader.GetOrdinal("NumberOfTeams")),
                                        Location = reader.GetString(reader.GetOrdinal("Location")),
                                        Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                                    };
                                    tournamentList.Add(tournament);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"An error occurred while fetching tournament: {ex.Message}");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                return tournamentList;
            }
        }

        private void FilteringQuery (TournamentFiltering filtering, StringBuilder queryBuilder, NpgsqlCommand command)
        {

            if (!string.IsNullOrEmpty(filtering.Name))
            {
                queryBuilder.Append(" AND LOWER(\"Name\") LIKE LOWER(@Name)");
                command.Parameters.AddWithValue("Name", "%" + filtering.Name + "%");
            }
            if (!string.IsNullOrEmpty(filtering.Description))
            {
                queryBuilder.Append(" AND LOWER(\"Description\") LIKE LOWER(@Description)");
                command.Parameters.AddWithValue("Description","%"+ filtering.Description + "%");
            }
            if (!string.IsNullOrEmpty(filtering.Location))
            {
                queryBuilder.Append(" AND LOWER(\"Location\") LIKE LOWER(@Location)");
                command.Parameters.AddWithValue("Location", "%"+filtering.Location+"%");
            }
            if (filtering.Date.HasValue)
            {
                queryBuilder.Append(" AND \"Date\" = @Date");
                command.Parameters.AddWithValue("Date", filtering.Date.Value.ToUniversalTime());
            }

            if (filtering.Status.HasValue)
            {
                switch (filtering.Status)
                {
                    case TournamentStatus.Past:
                        queryBuilder.Append(" AND \"Date\" < CURRENT_DATE");
                        break;
                    case TournamentStatus.Upcoming:
                        queryBuilder.Append(" AND \"Date\" > CURRENT_DATE");
                        break;
                    case TournamentStatus.Active:
                        queryBuilder.Append(" AND \"Date\" = CURRENT_DATE");
                        break;
                }
            }
        }

        private async Task<int> GetTotalTournamentCount(TournamentFiltering filtering)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder querryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"Tournament\" WHERE \"IsActive\" = True");
                FilteringQuery(filtering, querryBuilder, command);
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
