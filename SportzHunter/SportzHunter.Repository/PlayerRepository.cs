using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Model.Common;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace SportzHunter.Repository
{
    public class PlayerRepository : IPlayerRepository, IDisposable
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        protected ITeamRepository TeamRepository { get; set; }
        protected IUserRepository UserRepository { get; set; }
        public PlayerRepository(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            this.TeamRepository = teamRepository;
            this.UserRepository = userRepository;
        }

        public PlayerRepository()
        {
        }

        public async Task<Guid> GetSportCategoryIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT \"Player\".*, \"User\".* FROM \"Player\" LEFT JOIN \"User\" ON \"User\".\"Id\" = \"Player\".\"UserId\" WHERE \"Player\".\"UserId\" = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return Guid.Empty;
                }
                Guid category = Guid.Empty;
                while (await reader.ReadAsync())
                {
                    category = (Guid)reader["SportCategoryId"];
                }
                connection.Close();
                return category;
            }
        }


        public async Task<Player> GetPlayerByUserIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT \"Player\".*, \"User\".* FROM \"Player\" LEFT JOIN \"User\" ON \"User\".\"Id\" = \"Player\".\"UserId\" WHERE \"Player\".\"UserId\" = @id AND \"Player\".\"IsActive\" = True";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                Player playerToReturn = null;
                while (await reader.ReadAsync())
                {
                    playerToReturn = new Player
                    {
                        Id = (Guid)reader["Id"],
                        User = new User((Guid)reader["UserId"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Username"]),
                        TeamId = (reader["TeamId"] as Guid?) ?? Guid.Empty,
                        Height = (decimal)reader["Height"],
                        Weight = (decimal)reader["Weight"],
                        PreferredPositionId = (reader["PreferredPositionId"] as Guid?) ?? Guid.Empty,
                        DateOfBirth = (DateTime)reader["DOB"],
                        CountyId = (Guid)reader["CountyId"],
                        SportCategoryId = (Guid)reader["SportCategoryId"],
                        Description = (reader["Description"] as string) ?? reader["Description"]?.ToString(),
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"]
                    };
                }
                connection.Close();
                return playerToReturn;
            }
        }

        public async Task<Player> GetPlayerByIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT \"Player\".*, \"User\".*, \"County\".*, \"Position\".* FROM " +
                "\"Player\" LEFT JOIN \"User\" ON \"User\".\"Id\" = \"Player\".\"UserId\" LEFT JOIN " +
                     "\"County\" ON \"County\".\"Id\" = \"Player\".\"CountyId\"" +
                  " LEFT JOIN \"Position\" ON \"Position\".\"Id\" = \"Player\".\"PreferredPositionId\" WHERE " +
                        "\"Player\".\"IsActive\" = TRUE AND \"Player\".\"Id\" = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                Player playerToReturn = null;
                while (await reader.ReadAsync())
                {
                    playerToReturn = new Player
                    {
                        Id = (Guid)reader["Id"],
                        User = new User((Guid)reader["UserId"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Username"]),
                        TeamId = (reader["TeamId"] as Guid?) ?? Guid.Empty,
                        Height = (decimal)reader["Height"],
                        Weight = (decimal)reader["Weight"],
                        PreferredPositionId = (reader["PreferredPositionId"] as Guid?) ?? Guid.Empty,
                        PreferredPosition = new PreferredPosition
                        {
                            Id = (reader["PreferredPositionId"] as Guid?) ?? Guid.Empty,
                            PositionName = (reader["PostioionName"] as string)
                        },
                        DateOfBirth = (DateTime)reader["DOB"],
                        CountyId = (Guid)reader["CountyId"],
                        County = new County
                        {
                            Id = (reader["CountyId"] as Guid?) ?? Guid.Empty,
                            CountyName = (reader["ConutyName"] as string)
                        },
                        SportCategoryId = (Guid)reader["SportCategoryId"],
                        Description = (reader["Description"] as string) ?? reader["Description"]?.ToString(),
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"]
                    };
                }
                connection.Close();
                return playerToReturn;
            }
        }

        public async Task<Player> GetUserInfoAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT \"Player\".*, \"User\".* FROM \"Player\" LEFT JOIN \"User\" ON \"User\".\"Id\" = \"Player\".\"UserId\" WHERE \"User\".\"Id\" = @id AND \"Player\".\"IsActive\" = True";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                Player playerToReturn = null;
                while (await reader.ReadAsync())
                {
                    playerToReturn = new Player
                    {
                        Id = (Guid)reader["Id"],
                        User = new User((Guid)reader["UserId"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Username"]),
                        TeamId = (reader["TeamId"] as Guid?) ?? Guid.Empty,
                        Height = (decimal)reader["Height"],
                        Weight = (decimal)reader["Weight"],
                        PreferredPositionId = (reader["PreferredPositionId"] as Guid?) ?? Guid.Empty,
                        DateOfBirth = (DateTime)reader["DOB"],
                        CountyId = (Guid)reader["CountyId"],
                        SportCategoryId = (Guid)reader["SportCategoryId"],
                        Description = (reader["Description"] as string) ?? reader["Description"]?.ToString(),
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"]
                    };

                    if(playerToReturn.TeamId != Guid.Empty)
                    {
                        playerToReturn.Team = await TeamRepository.GetTeamByIdAsync((Guid)playerToReturn.TeamId);
                    }
                }
                connection.Close();
                return playerToReturn;
            }
        }

        public async Task<int> PostCreateTeamleaderAsync(TeamLeader newTeamleader, Team newTeam)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"TeamLeader\" (\"Id\",\"UserId\",\"SportCategoryId\",\"Description\"," +
                        "\"IsAccepted\",\"CreatedBy\",\"UpdatedBy\",\"DateCreated\",\"DateUpdated\",\"IsActive\") VALUES (@Id, @UserId," +
                        "@SportCategoryId, @Description, @IsAccepted, @CreatedBy, @UpdatedBy," +
                        "@DateCreated, @DateUpdated, @IsActive)";

                    connection.Open();

                    command.Parameters.AddWithValue("@Id", newTeamleader.Id);
                    command.Parameters.AddWithValue("@UserId", newTeamleader.UserId);
                    command.Parameters.AddWithValue("@SportCategoryId", newTeamleader.SportCategoryId);
                    command.Parameters.AddWithValue("@Description", newTeamleader.Description);
                    command.Parameters.AddWithValue("@IsAccepted", false);
                    command.Parameters.AddWithValue("@CreatedBy", newTeamleader.CreatedBy);
                    command.Parameters.AddWithValue("@UpdatedBy", newTeamleader.UpdatedBy);
                    command.Parameters.AddWithValue("@DateCreated", newTeamleader.DateCreated);
                    command.Parameters.AddWithValue("@DateUpdated", newTeamleader.DateUpdated);
                    command.Parameters.AddWithValue("@IsActive", true);

                    if (await command.ExecuteNonQueryAsync() > 0 && await TeamRepository.CreateTeamAsync(newTeam, newTeamleader.UserId))
                    {
                        connection.Close();
                        return 1;
                    }
                    connection.Close();
                    return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<int> UpdatePlayerProfileAsync(Guid id, Player playerDetails, Guid userId)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    StringBuilder query = new StringBuilder("UPDATE \"Player\" SET ");

                    using (NpgsqlCommand command = new NpgsqlCommand("", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        /*
                        if (playerDetails.User != null && !string.IsNullOrEmpty(playerDetails.User.FirstName))
                        {
                            query.Append("\"FirstName\" = @FirstName, ");
                            command.Parameters.AddWithValue("@FirstName", playerDetails.User.FirstName);
                        }
                        if (playerDetails.User != null && !string.IsNullOrEmpty(playerDetails.User.LastName))
                        {
                            query.Append("\"LastName\" = @LastName, ");
                            command.Parameters.AddWithValue("@LastName", playerDetails.User.LastName);
                        }*/
                        if (playerDetails.Height.HasValue)
                        {
                            query.Append("\"Height\" = @Height, ");
                            command.Parameters.AddWithValue("@Height", playerDetails.Height);
                        }
                        if (playerDetails.Weight.HasValue)
                        {
                            query.Append("\"Weight\" = @Weight, ");
                            command.Parameters.AddWithValue("@Weight", playerDetails.Weight);
                        }
                        if (playerDetails.PreferredPositionId.HasValue)
                        {
                            query.Append("\"PreferredPositionId\" = @PreferredPositionId, ");
                            command.Parameters.AddWithValue("@PreferredPositionId", playerDetails.PreferredPositionId);
                        }
                        if (!string.IsNullOrEmpty(playerDetails.Description))
                        {
                            query.Append("\"Description\" = @Description, ");
                            command.Parameters.AddWithValue("@Description", playerDetails.Description);
                        }

                        query.Append(" \"DateUpdated\" = @DateUpdated,");
                        query.Append(" \"UpdatedBy\" = @UpdatedBy WHERE \"Id\" = @Id ");
                        command.CommandText = query.ToString();

                        command.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                        command.Parameters.AddWithValue("@UpdatedBy", userId);


                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating player profile.", ex);
            }
        }

        public async Task<PagedList<Player>> GetPlayersAsync(PlayerFiltering filtering, Sorting sorting, Paging paging)
        {
            NpgsqlConnection connection = null;
            try
            {
                connection = new NpgsqlConnection(_connectionString);
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    StringBuilder querybuilder = new StringBuilder("SELECT \"Player\".*, \"User\".*, \"County\".*, \"Position\".* FROM " +
                "\"Player\" LEFT JOIN \"User\" ON \"User\".\"Id\" = \"Player\".\"UserId\" LEFT JOIN " +
                     "\"County\" ON \"County\".\"Id\" = \"Player\".\"CountyId\"" +
                  " LEFT JOIN \"Position\" ON \"Position\".\"Id\" = \"Player\".\"PreferredPositionId\" WHERE" +
                    " \"Player\".\"SportCategoryId\" = @id AND \"Player\".\"IsActive\" = TRUE AND \"Player\".\"TeamId\" IS NULL AND \"User\".\"RoleId\" = @role");

                    SetFilters(querybuilder, filtering, command);

                    if (!string.IsNullOrEmpty(sorting.SortBy) && paging.PageNumber > 0 && paging.PageSize > 0)
                    {
                        //TODO: fix potential SQL injection
                        querybuilder.Append($" ORDER BY \"{sorting.SortBy}\" {sorting.SortOrder} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                        command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                        command.Parameters.AddWithValue("@PageSize", paging.PageSize);
                    }
                    command.Parameters.AddWithValue("@id", filtering.SportCategoryId);
                    command.Parameters.AddWithValue("@role", await UserRepository.GetRoleByNameAsync("Player"));
                    command.CommandText = querybuilder.ToString();
                    connection.Open();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (!reader.HasRows)
                        return null;

                    List<Player> playerList = new List<Player>();

                    while (await reader.ReadAsync())
                    {
                        playerList.Add(new Player
                        {
                            Id = (Guid)reader["Id"],
                            User = new User((Guid)reader["UserId"], (string)reader["FirstName"], (string)reader["LastName"], (string)reader["Username"]),
                            TeamId = Guid.Empty,
                            Height = (decimal)reader["Height"],
                            Weight = (decimal)reader["Weight"],
                            PreferredPositionId = (reader["PreferredPositionId"] as Guid?) ?? Guid.Empty,
                            PreferredPosition = new PreferredPosition
                            {
                                Id = (reader["PreferredPositionId"] as Guid?) ?? Guid.Empty,
                                PositionName = (reader["PostioionName"] as string)
                            },
                            DateOfBirth = (DateTime)reader["DOB"],
                            CountyId = (Guid)reader["CountyId"],
                            County = new County
                            {
                                Id = (reader["CountyId"] as Guid?) ?? Guid.Empty,
                                CountyName = (reader["ConutyName"] as string)
                            },
                            SportCategoryId = (Guid)reader["SportCategoryId"],
                            Description = (reader["Description"] as string) ?? reader["Description"]?.ToString(),
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"]
                        }); ;
                    }
                    connection.Close();

                    int totalCount = await GetTotalPlayerCount(filtering);
                    return new PagedList<Player>(playerList, paging.PageSize, totalCount);
                }
            }
            catch (Exception ex)
            {
                connection.Close();
                return null;
            }
        }

        private async Task<int> GetTotalPlayerCount(PlayerFiltering filtering)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder querryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"Player\" LEFT JOIN \"User\" ON \"Player\".\"UserId\" = \"User\".\"Id\" WHERE \"Player\".\"IsActive\" = TRUE " +
                    " AND \"Player\".\"TeamId\" IS NULL AND \"Player\".\"SportCategoryId\" = @id AND \"User\".\"RoleId\" = @role");
                SetFilters(querryBuilder, filtering, command);
                command.CommandText = querryBuilder.ToString();
                command.Parameters.AddWithValue("@id", filtering.SportCategoryId);
                command.Parameters.AddWithValue("@role", await UserRepository.GetRoleByNameAsync("Player"));
                connection.Open();
                var result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int count))
                {
                    return count;
                }
                return 0;
            }
        }

        public void SetFilters(StringBuilder querybuilder, PlayerFiltering filtering, NpgsqlCommand command)
        {

            if (filtering.PositionId != null)
            {
                querybuilder.Append(" AND \"Player\".\"PreferredPositionId\" = @positionId");
                command.Parameters.AddWithValue("@positionId", filtering.PositionId);
            }
            if (filtering.CountyId != null)
            {
                querybuilder.Append(" AND \"Player\".\"CountyId\" = @countyId");
                command.Parameters.AddWithValue("@countyId", filtering.CountyId);
            }
            if (filtering.AgeCategory != 0)
            {
                DateTime currentDate = DateTime.Now;
                DateTime dobLowerLimit;
                DateTime dobUpperLimit;
                switch (filtering.AgeCategory)
                {
                    case 1:
                        dobLowerLimit = currentDate.AddYears(-18);
                        dobUpperLimit = currentDate.AddYears(-16);
                        querybuilder.Append(" AND \"Player\".\"DOB\" BETWEEN @lowerLimit AND @upperLimit");
                        command.Parameters.AddWithValue("@upperLimit", dobUpperLimit.Date);
                        command.Parameters.AddWithValue("@lowerLimit", dobLowerLimit.Date);
                        break;
                    case 2:
                        dobUpperLimit = currentDate.AddYears(-18);
                        querybuilder.Append(" AND \"Player\".\"DOB\" <= @upperLimit");
                        command.Parameters.AddWithValue("@upperLimit", dobUpperLimit.Date);
                        break;
                }
            }
        }

        public async Task<int> LeaveTeamAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "UPDATE \"Player\" SET \"TeamId\" = NULL, \"DateUpdated\" = @dateUpdated, \"UpdatedBy\" = @updatedBy WHERE \"UserId\" = @id";
                command.Parameters.AddWithValue("@dateUpdated", DateTime.UtcNow);
                command.Parameters.AddWithValue("@updatedBy", id);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                if (await command.ExecuteNonQueryAsync() <= 0)
                {
                    connection.Close();
                    return -1;
                }
                connection.Close();
                return 1;
            }
        }

        public void Dispose()
        {
            return;
        }
    }
}


