using Npgsql;
using SportzHunter.Model;
using SportzHunter.Model.Common;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class InviteRepository : IInviteRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private TeamRepository _teamRepository = new TeamRepository();
        private IPlayerRepository _playerRepository;

        public InviteRepository(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        async Task<Guid> GetTeamIdFromInviteAsync(Guid playerId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string invitesQuery = "SELECT \"Id\", \"PlayerId\", \"TeamId\" FROM \"Invite\" " +
                    "WHERE \"PlayerId\" = @Id AND \"CreatedBy\" <> @CreatedPerson AND \"Invite\".\"IsActive\" = true";

                NpgsqlCommand cmd = new NpgsqlCommand(invitesQuery, connection);
                cmd.Parameters.AddWithValue("@CreatedPerson", playerId);
                cmd.Parameters.AddWithValue("@Id", playerId);

                connection.Open();
                await cmd.ExecuteNonQueryAsync();

                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.HasRows && await reader.ReadAsync())
                {
                    return (Guid)reader["TeamId"];
                }
            }

            return Guid.Empty;
        }

        public async Task<List<Invite>> GetAllInvitesByIdAsync(Guid teamId, Guid userId, Guid playerId)
        {
            List<Invite> invites = new List<Invite>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string invitesQuery = "SELECT \"Id\", \"PlayerId\", \"TeamId\" FROM \"Invite\" " +
                    "WHERE \"TeamId\" = @Id AND \"CreatedBy\" <> @CreatedPerson AND \"Invite\".\"IsActive\" = true";

                NpgsqlCommand cmd = new NpgsqlCommand(invitesQuery, connection);
                if(teamId == Guid.Empty)
                {
                    teamId = await GetTeamIdFromInviteAsync(playerId);
                }
                cmd.Parameters.AddWithValue("@Id", teamId);
                cmd.Parameters.AddWithValue("@CreatedPerson", userId);

                connection.Open();
                await cmd.ExecuteNonQueryAsync();

                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.HasRows && await reader.ReadAsync())
                {
                    invites.Add(new Invite
                    {
                        Id = (Guid)reader["Id"],
                        PlayerId = (Guid)reader["PlayerId"],
                        Player = await _playerRepository.GetPlayerByIdAsync((Guid)reader["PlayerId"]),
                        Team = await _teamRepository.GetTeamByIdAsync((Guid)reader["TeamId"])
                    });
                }
            }

            return invites;
        }

        private async Task<bool> CheckIfInviteExists(Guid receivingPlayerId, Guid teamId)
        {
            InviteStatusRepository status = new InviteStatusRepository();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string checkQuery = "SELECT COUNT(*) FROM \"Invite\" WHERE \"PlayerId\" = @PlayerId AND \"TeamId\" = @TeamId AND \"IsActive\" = true AND \"InviteStatus\" = @Status";
                NpgsqlCommand command = new NpgsqlCommand(checkQuery, connection);
                command.Parameters.AddWithValue("@PlayerId", receivingPlayerId);
                command.Parameters.AddWithValue("@TeamId", teamId);
                command.Parameters.AddWithValue("@Status", await status.GetInviteStatus("Pending"));

                await connection.OpenAsync();

                object result = await command.ExecuteScalarAsync();

                if (result != null && result != DBNull.Value)
                {
                    if (int.TryParse(result.ToString(), out int count))
                    {
                        return count > 0;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid result returned from query.");
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> SendInviteAsync(Invite invite, Guid receivingPlayerId, Guid teamId)
        {
            if (await CheckIfInviteExists(receivingPlayerId, teamId))
                return false;
            InviteStatusRepository status = new InviteStatusRepository();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string invitationQuery = "INSERT INTO \"Invite\" VALUES (@Id, @PlayerId, @TeamId, @InviteStatus, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";
                NpgsqlCommand command = new NpgsqlCommand(invitationQuery, connection);

                command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                command.Parameters.AddWithValue("@PlayerId", receivingPlayerId);
                command.Parameters.AddWithValue("@TeamId", teamId);
                command.Parameters.AddWithValue("@InviteStatus", await status.GetInviteStatus("Pending"));
                command.Parameters.AddWithValue("@CreatedBy", invite.CreatedBy);
                command.Parameters.AddWithValue("@UpdatedBy", invite.UpdatedBy);
                command.Parameters.AddWithValue("@DateCreated", invite.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", invite.DateUpdated);
                command.Parameters.AddWithValue("@IsActive", true);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                int InviteSent = await command.ExecuteNonQueryAsync();

                if (InviteSent != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return false;
        }

        public async Task<int> AcceptOrDeclineInviteAsync(PutInviteModel invite)
        {
            InviteStatusRepository status = new InviteStatusRepository();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                NpgsqlCommand commandPlayer = new NpgsqlCommand();
                commandPlayer.Connection = connection;

                string inviteQuery = "UPDATE \"Invite\" SET \"InviteStatus\" = @InviteStatus, \"IsActive\" = false, \"DateUpdated\" = @DateNow, \"UpdatedBy\" = @UpdatePerson WHERE \"Id\" = @Id";
                string PlayerQuery = "";

                NpgsqlCommand commandInvite = new NpgsqlCommand(inviteQuery, connection);

                if (invite.ResponseSent)
                {
                    commandInvite.Parameters.AddWithValue("@InviteStatus", await status.GetInviteStatus("Accepted"));

                    PlayerQuery = "UPDATE \"Player\" SET \"TeamId\" = @Team WHERE \"Id\" = @Id AND \"TeamId\" IS NULL";
                    commandPlayer.Parameters.AddWithValue("@Team", invite.TeamId);
                    commandPlayer.Parameters.AddWithValue("@Id", invite.PlayerId);
                    commandPlayer.CommandText = PlayerQuery;
                }
                else
                    commandInvite.Parameters.AddWithValue("@InviteStatus", await status.GetInviteStatus("Declined"));

                commandInvite.Parameters.AddWithValue("@Id", invite.Id);
                commandInvite.Parameters.AddWithValue("@IsActive", false);
                commandInvite.Parameters.AddWithValue("@DateNow", invite.DateUpdated);
                commandInvite.Parameters.AddWithValue("@UpdatePerson", invite.UpdatedBy);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                commandInvite.Transaction = commandPlayer.Transaction = transaction;

                int statusUpdatedInvite = await commandInvite.ExecuteNonQueryAsync();

                if (invite.ResponseSent)
                {
                    int statusUpdatedPlayer = await commandPlayer.ExecuteNonQueryAsync();
                    if (statusUpdatedPlayer == 0) return -1;
                }

                if (statusUpdatedInvite != 0)
                {
                    transaction.Commit();
                    return 1;
                }
            }

            return 0;
        }

        public async Task<bool> DeleteInviteAsync(DeleteInviteModel invite)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string invitationQuery = "UPDATE \"Invite\" SET \"IsActive\" = @ActiveStatus, \"DateUpdated\" = @DateNow, \"UpdatedBy\" = @UpdatePerson WHERE \"Id\" = @Id";

                NpgsqlCommand command = new NpgsqlCommand(invitationQuery, connection);
                command.Parameters.AddWithValue("@ActiveStatus", false);
                command.Parameters.AddWithValue("@DateNow", invite.DateUpdated);
                command.Parameters.AddWithValue("@updatePerson", invite.UpdatedBy);
                command.Parameters.AddWithValue("@Id", invite.Id);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                int valueUpdated = await command.ExecuteNonQueryAsync();
                if (valueUpdated != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> CheckIfInviteWasSentAsync(Guid playerId, Guid teamId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string checkQuery = "SELECT COUNT(*) AS RowCount FROM \"Invite\" WHERE \"PlayerId\" = @playerId " +
                    "AND \"TeamId\" = @teamId AND \"IsActive\" = True";

                NpgsqlCommand command = new NpgsqlCommand(checkQuery, connection);
                command.Parameters.AddWithValue("@playerId", playerId);
                command.Parameters.AddWithValue("teamId", teamId);

                await connection.OpenAsync();

                object result = await command.ExecuteScalarAsync();

                if (result != null && result != DBNull.Value)
                {
                    if (int.TryParse(result.ToString(), out int count))
                    {
                        return count > 0;
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid result returned from query.");
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
