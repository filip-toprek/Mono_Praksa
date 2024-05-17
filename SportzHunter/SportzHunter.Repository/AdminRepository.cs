using Npgsql;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        protected IUserRepository UserRepository { get; set; }
        protected ITeamRepository TeamRepository { get; set; }

        public AdminRepository(IUserRepository userRepository, ITeamRepository teamRepository)
        {
            UserRepository = userRepository;
            TeamRepository = teamRepository;
        }

        public async Task<List<TeamLeader>> GetPendingApplicationsAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"TeamLeader\" WHERE \"IsAccepted\" = False AND \"IsActive\" = True";
                    connection.Open();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                    if (!reader.HasRows)
                        return null;

                    List<TeamLeader> pendingApplications = new List<TeamLeader>();
                    while (reader.Read())
                    {
                        pendingApplications.Add(new TeamLeader
                        {
                            Id = (Guid)reader["Id"],
                            UserId = (Guid)reader["UserId"],
                            SportCategoryId = (Guid)reader["SportCategoryId"],
                            Description = (string)reader["Description"],
                            IsAccepted = (bool)reader["IsAccepted"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"],
                            Team = await TeamRepository.GetTeamByTeamLeaderAsync((Guid)reader["Id"])
                        }) ;
                    }
                    connection.Close();
                    return pendingApplications;
                }
                catch
                {
                    connection.Close();
                    return null;
                }
            }
        }

        public async Task<int> ApproveApplicationAsync(Guid id, Guid adminId)
        {
            NpgsqlTransaction npgsqlTransaction = null;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"TeamLeader\" SET \"IsAccepted\" = True, \"IsActive\" = True, \"UpdatedBy\" = @updatedBy, " +
                        "\"DateUpdated\" = @dateUpdated WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@updatedBy", adminId);
                    command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    npgsqlTransaction = connection.BeginTransaction();

                    if (await command.ExecuteNonQueryAsync() <= 0)
                        return 0;

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Team\" SET \"IsActive\" = True, \"UpdatedBy\" = @updatedBy, " +
                        "\"DateUpdated\" = @dateUpdated WHERE \"TeamLeaderId\" = @id";
                    command.Parameters.AddWithValue("@updatedBy", adminId);
                    command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@id", id);

                    if (await command.ExecuteNonQueryAsync() <= 0)
                    {
                        await npgsqlTransaction.RollbackAsync();
                        connection.Close();
                        return -1;
                    }

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM \"TeamLeader\" " +
                        "WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@id", id);

                    NpgsqlDataReader reader = command.ExecuteReader();

                    if(!reader.HasRows)
                    {
                        await npgsqlTransaction.RollbackAsync();
                        connection.Close();
                        return -1;
                    }

                    Guid userId = Guid.Empty;
                    while(await reader.ReadAsync())
                    {
                        userId = (Guid)reader["UserId"];
                    }
                    await reader.CloseAsync();

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" " +
                        "SET \"RoleId\" = @role, " +
                        "\"DateUpdated\" = @dateUpdated, " +
                        "\"UpdatedBy\" = @updatedBy " +
                        "WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@updatedBy", adminId);
                    command.Parameters.AddWithValue("@id", userId);
                    command.Parameters.AddWithValue("@role", await UserRepository.GetRoleByNameAsync("Teamleader"));

                    if (await command.ExecuteNonQueryAsync() <= 0)
                    {
                        await npgsqlTransaction.RollbackAsync();
                        connection.Close();
                        return -1;
                    }

                    await npgsqlTransaction.CommitAsync();
                    return 1;
                }
                catch (Exception ex)
                {
                    await npgsqlTransaction.RollbackAsync();
                    connection.Close();
                    return -1;
                }
            }
        }

        public async Task<int> DisapproveApplicationAsync(Guid id)
        {
            NpgsqlTransaction npgsqlTransaction = null;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"TeamLeader\" SET \"IsAccepted\" = False, \"IsActive\" = False WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    npgsqlTransaction = connection.BeginTransaction();

                    if (await command.ExecuteNonQueryAsync() <= 0)
                        return 0;

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Team\" SET \"IsActive\" = False WHERE \"TeamLeaderId\" = @id";
                    command.Parameters.AddWithValue("@id", id);

                    if (await command.ExecuteNonQueryAsync() <= 0)
                        return 0;

                    npgsqlTransaction.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    npgsqlTransaction.Rollback();
                    connection.Close();
                    return -1;
                }
            }
        }

        public async Task<int> DeleteUserByIdAsync(Guid id, Guid adminId, bool isPlayer)
        {
            NpgsqlTransaction npgsqlTransaction = null;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"User\" " +
                        "SET \"IsActive\" = False, " +
                        "\"DateUpdated\" = @dateUpdated, " +
                        "\"UpdatedBy\" = @updatedBy " +
                        "WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@updatedBy", adminId);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    npgsqlTransaction = connection.BeginTransaction();
                    if (await command.ExecuteNonQueryAsync() <= 0)
                    {
                        await npgsqlTransaction.RollbackAsync();
                        connection.Close();
                        return -1;
                    }

                    command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "UPDATE \"Player\" " +
                        "SET \"IsActive\" = False, " +
                        "\"DateUpdated\" = @dateUpdated, " +
                        "\"UpdatedBy\" = @updatedBy " +
                        "WHERE \"UserId\" = @id";
                    command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@updatedBy", adminId);
                    command.Parameters.AddWithValue("@id", id);

                    if (await command.ExecuteNonQueryAsync() <= 0)
                    {
                        await npgsqlTransaction.RollbackAsync();
                        connection.Close();
                        return -1;
                    }

                    if (!isPlayer)
                    {
                        command = new NpgsqlCommand();
                        command.Connection = connection;
                        command.CommandText = "UPDATE \"TeamLeader\" " +
                            "SET \"IsActive\" = False, " +
                            "\"DateUpdated\" = @dateUpdated, " +
                            "\"UpdatedBy\" = @updatedBy " +
                            "WHERE \"UserId\" = @id";
                        command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@updatedBy", adminId);
                        command.Parameters.AddWithValue("@id", id);

                        if (await command.ExecuteNonQueryAsync() <= 0)
                        {
                            await npgsqlTransaction.RollbackAsync();
                            connection.Close();
                            return -1;
                        }

                        command = new NpgsqlCommand();
                        command.Connection = connection;
                        command.CommandText = "UPDATE \"Team\" " +
                            "SET \"IsActive\" = False, " +
                            "\"DateUpdated\" = @dateUpdated, " +
                            "\"UpdatedBy\" = @updatedBy " +
                            "FROM \"TeamLeader\" WHERE \"Team\".\"TeamLeaderId\" = \"TeamLeader\".\"Id\"" +
                            " AND \"TeamLeader\".\"UserId\" = @id";
                        command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@updatedBy", adminId);
                        command.Parameters.AddWithValue("@id", id);

                        if (await command.ExecuteNonQueryAsync() <= 0)
                        {
                            await npgsqlTransaction.RollbackAsync();
                            connection.Close();
                            return -1;
                        }

                        await npgsqlTransaction.CommitAsync();
                        connection.Close();
                        return 1;
                    }
                    await npgsqlTransaction.CommitAsync();
                    connection.Close();
                    return 1;
                }
                catch (Exception ex)
                {
                    await npgsqlTransaction.RollbackAsync();
                    connection.Close();
                    return -1;
                }
            }
        }

    }
}
