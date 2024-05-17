using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class AttendRepository : IAttendRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        protected ITeamRepository TeamRepository { get; set; }
        protected ITournamentRepository TournamentRepository { get; set; }

        public AttendRepository(ITeamRepository teamRepository, ITournamentRepository tournamentRepository)
        {
            TeamRepository = teamRepository;
            TournamentRepository = tournamentRepository;
        }
        public async Task<int> AddAttendTournamentAsync(Attend attend)
        {
            int rowsAffected = 0;
            if (attend == null)
            {
                return rowsAffected;
            }
            string query = "INSERT INTO \"Attend\" (\"Id\",\"TournamentId\",\"TeamId\",\"IsApproved\",\"CreatedBy\",\"UpdatedBy\",\"DateCreated\",\"DateUpdated\",\"IsActive\")VALUES (@Id,@TournamentId,@TeamId,@IsApproved,@CreatedBy,@UpdatedBy,@DateCreated,@DateUpdated,@IsActive)";
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            NpgsqlCommand command = new NpgsqlCommand(query.ToString(), connection);
            using (connection)
            using (command)
            {
                command.Parameters.AddWithValue("@Id", attend.Id);
                command.Parameters.AddWithValue("@TournamentId", attend.TournamentId);
                command.Parameters.AddWithValue("@TeamId", attend.TeamId);
                command.Parameters.AddWithValue("@IsApproved", false);
                command.Parameters.AddWithValue("@CreatedBy", attend.CreatedBy);
                command.Parameters.AddWithValue("@UpdatedBy", attend.UpdatedBy);
                command.Parameters.AddWithValue("@DateCreated", attend.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", attend.DateUpdated);
                command.Parameters.AddWithValue("@IsActive", true);

                try
                {
                    await connection.OpenAsync();
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
                    throw new Exception($"An error occurred while attending: {ex.Message}");
                }
            }
        }
        public async Task<bool> IsTeamAlreadyAttendingAsync(Guid tournamentId, Guid teamId)
        {
            string query = "SELECT COUNT(*) FROM \"Attend\" WHERE \"TournamentId\" = @TournamentId AND \"TeamId\" = @TeamId";
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TournamentId", tournamentId);
                    command.Parameters.AddWithValue("@TeamId", teamId);
                    int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<PagedList<Attend>> GetAttendListAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"Attend\" WHERE \"IsApproved\" = False AND \"IsActive\" = true";
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                List<Attend> attendList = new List<Attend>();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                while (await reader.ReadAsync())
                {
                    attendList.Add(new Attend
                    {
                        Id = (Guid)reader["Id"],
                        Tournament = await TournamentRepository.GetTournamentByIdAsync((Guid)reader["TournamentId"]),
                        Team = await TeamRepository.GetTeamByIdAsync((Guid)reader["TeamId"]),
                        IsApproved = (bool)reader["IsApproved"],
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"]
                    }
                        );
                }
                connection.Close();
                return new PagedList<Attend>(attendList, 5, attendList.Count);
            }
        }

        public async Task<int> ApproveAttendAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE \"Attend\" SET \"IsApproved\" = True WHERE \"Id\" = @id AND \"IsActive\" = true";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                
                if(await command.ExecuteNonQueryAsync() <=0)
                    return 0;

                connection.Close();
                return 1;
            }
        }

        public async Task<int> DisapproveAttendAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE \"Attend\" SET \"IsActive\" = False WHERE \"Id\" = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();

                if (await command.ExecuteNonQueryAsync() <= 0)
                    return 0;

                connection.Close();
                return 1;
            }
        }


    }
}
