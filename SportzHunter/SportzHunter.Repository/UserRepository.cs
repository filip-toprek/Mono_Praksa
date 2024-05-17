using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public async Task<Guid> GetRoleByNameAsync(string roleName)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT \"Id\" FROM \"Role\" WHERE \"RoleName\" LIKE @roleName";
                command.Parameters.AddWithValue("@roleName", roleName);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return Guid.Parse(null);
                }
                while (await reader.ReadAsync())
                    return (Guid)reader[0];
                return Guid.Parse(null);
            }

        }
        async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"Role\" WHERE \"Id\" = @roleId";
                command.Parameters.AddWithValue("@roleId", roleId);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }

                Role role = null;
                while (await reader.ReadAsync())
                {
                    role = new Role(
                        (Guid)reader["Id"],
                        (string)reader["RoleName"]
                    );
                }
                return role;
            }

        }

        public async Task<PagedList<User>> GetUsersAsync(Paging paging, Sorting sorting)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder queryBuilder = new StringBuilder("SELECT \"User\".*, \"Role\".* FROM \"User\" LEFT JOIN \"Role\" ON \"Role\".\"Id\" = \"User\".\"RoleId\" WHERE \"User\".\"IsActive\" = True");


                if (!string.IsNullOrEmpty(sorting.SortBy) && paging.PageNumber > 0 && paging.PageSize > 0)
                {
                    //TODO: fix potential SQL injection
                    queryBuilder.Append($" ORDER BY \"{sorting.SortBy}\" {sorting.SortOrder} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                    command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                    command.Parameters.AddWithValue("@PageSize", paging.PageSize);
                }

                command.CommandText = queryBuilder.ToString();
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                List<User> userList = new List<User> ();
                while (await reader.ReadAsync())
                {
                    userList.Add(new User
                    {
                        Id = (Guid)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Username = (string)reader["Username"],
                        UserRole = (Role) await GetRoleByIdAsync((Guid)reader["RoleId"]),
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"],
                    });
                }
                connection.Close();

                return new PagedList<User>(userList, paging.PageSize, await GetCountAsync());
            }
        }

        public async Task<int> GetCountAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                StringBuilder querryBuilder = new StringBuilder("SELECT COUNT(*) FROM \"User\" WHERE 1=1 AND \"IsActive\" = True");
                command.CommandText = querryBuilder.ToString();
                connection.Open();
                var result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int count))
                {
                    return count;
                }
                return 0;
            }
        }

        public async Task<List<SportCategory>> GetSportCategoriesAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"SportCategory\" WHERE \"IsActive\" = True";
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                List<SportCategory> categoryList = new List<SportCategory>();
                while (await reader.ReadAsync())
                {
                    categoryList.Add(new SportCategory((Guid)reader["Id"], (string)reader["CategoryName"], (DateTime)reader["DateCreated"], (DateTime)reader["DateUpdated"], (bool)reader["IsActive"]));
                }
                connection.Close();
                return categoryList;
            }
        }

        public async Task<List<County>> GetCountiesAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"County\" WHERE \"IsActive\" = True";
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                List<County> countyList = new List<County>();
                while (await reader.ReadAsync())
                {
                    countyList.Add(new County((Guid)reader["Id"], (string)reader["ConutyName"], (DateTime)reader["DateCreated"], (DateTime)reader["DateUpdated"], (bool)reader["IsActive"]));
                }
                connection.Close();
                return countyList;
            }
        }

        public async Task<List<PreferredPosition>> GetPrefPositionsAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"Position\" WHERE \"IsActive\" = True";
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                List<PreferredPosition> prefPositionList = new List<PreferredPosition>();
                while (await reader.ReadAsync())
                {
                    prefPositionList.Add(new PreferredPosition((Guid)reader["Id"], (string)reader["PostioionName"], (DateTime)reader["DateCreated"], (DateTime)reader["DateUpdated"], (bool)reader["IsActive"]));
                }
                connection.Close();
                return prefPositionList;
            }
        }
        public async Task<User> GetUserById(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"User\" WHERE \"Id\" = @id";
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return null;
                }
                User userToReturn = null;
                while (await reader.ReadAsync())
                {
                    userToReturn = new User
                    {
                        Id = (Guid)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Username = (string)reader["Username"],
                        UserRole = (Role)GetRoleByIdAsync((Guid)reader["RoleId"]).Result,
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"],
                    };
                }
                connection.Close();
                return userToReturn;
            }
        }

        public async Task<int> GetUserByUsername(string username)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"User\" WHERE \"Username\" = @username";
                command.Parameters.AddWithValue("@username", username);
                connection.Open();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows)
                {
                    connection.Close();
                    return 0;
                }
                connection.Close();
                return 1;
            }
        }
        public async Task<int> PostRegisterUserAsync(User userToRegister, Player newPlayer)
        {
            if (await GetUserByUsername(userToRegister.Username) == 1)
                return -1;

            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlTransaction npgsqlTransaction = null;
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"User\" " +
                   "(\"Id\", \"FirstName\", \"LastName\", \"Username\", \"PasswordHash\", " +
                   "\"RoleId\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
                   "VALUES " +
                   "(@id, @firstName, @lastName, @username, @passwordHash, " +
                   "@roleId, @createdBy, @updatedBy, @dateCreated, @dateUpdated, @isActive)";

                    command.Parameters.AddWithValue("@id", userToRegister.Id);
                    command.Parameters.AddWithValue("@firstName", userToRegister.FirstName);
                    command.Parameters.AddWithValue("@lastName", userToRegister.LastName);
                    command.Parameters.AddWithValue("@username", userToRegister.Username);
                    command.Parameters.AddWithValue("@passwordHash", userToRegister.PasswordHash);
                    command.Parameters.AddWithValue("@roleId", await GetRoleByNameAsync("Player"));
                    command.Parameters.AddWithValue("@createdBy", userToRegister.CreatedBy);
                    command.Parameters.AddWithValue("@updatedBy", userToRegister.UpdatedBy);
                    command.Parameters.AddWithValue("@dateCreated", userToRegister.DateCreated);
                    command.Parameters.AddWithValue("@dateUpdated", userToRegister.DateUpdated);
                    command.Parameters.AddWithValue("@isActive", true);

                    connection.Open();
                    npgsqlTransaction = connection.BeginTransaction();
                    if (await command.ExecuteNonQueryAsync() > 0)
                    {
                        command = new NpgsqlCommand();
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO \"Player\" " +
                       "(\"Id\", \"UserId\", \"TeamId\", \"Height\", \"Weight\", " +
                       "\"PreferredPositionId\", \"DOB\", \"CountyId\", \"SportCategoryId\", \"Description\"," +
                       " \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
                       "VALUES " +
                       "(@id, @userId, @teamId, @height, @weight, " +
                       "@preferredPositionId, @dob, @countyId, @sportCategoryId, @description, @createdBy, @updatedBy, @dateCreated, @dateUpdated, @isActive)";

                        command.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Uuid, newPlayer.Id);
                        command.Parameters.AddWithValue("@userId", NpgsqlTypes.NpgsqlDbType.Uuid, newPlayer.User.Id);
                        if (newPlayer.TeamId.HasValue)
                            command.Parameters.AddWithValue("@teamId", newPlayer.TeamId);
                        else
                            command.Parameters.AddWithValue("@teamId", DBNull.Value);
                        command.Parameters.AddWithValue("@height", newPlayer.Height);
                        command.Parameters.AddWithValue("@weight", newPlayer.Weight);
                        command.Parameters.AddWithValue("@preferredPositionId", NpgsqlTypes.NpgsqlDbType.Uuid, (object)newPlayer.PreferredPositionId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@dob", newPlayer.DateOfBirth.Date);
                        command.Parameters.AddWithValue("@countyId", newPlayer.CountyId);
                        command.Parameters.AddWithValue("@sportCategoryId", newPlayer.SportCategoryId);
                        command.Parameters.AddWithValue("@description", NpgsqlTypes.NpgsqlDbType.Varchar, (object)newPlayer.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@createdBy", userToRegister.CreatedBy);
                        command.Parameters.AddWithValue("@updatedBy", userToRegister.UpdatedBy);
                        command.Parameters.AddWithValue("@dateCreated", userToRegister.DateCreated);
                        command.Parameters.AddWithValue("@dateUpdated", userToRegister.DateUpdated);
                        command.Parameters.AddWithValue("@isActive", true);
                        await command.ExecuteNonQueryAsync();
                        await npgsqlTransaction.CommitAsync();
                        connection.Close();
                        return 1;
                    }
                    else
                    {
                        await npgsqlTransaction.RollbackAsync();
                        connection.Close();
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    await npgsqlTransaction.RollbackAsync();
                    connection.Close();
                    return -1;
                }
            }
        }

        public async Task<User> PostLoginUserAsync(User userToLogin)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = connection;
                    command.CommandText = "SELECT  * FROM \"User\" WHERE \"Username\" LIKE @username";
                    command.Parameters.AddWithValue("username", userToLogin.Username);
                    connection.Open();

                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    User loggedUser = null;
                    while (await reader.ReadAsync())
                    {
                        loggedUser = new User((Guid)reader["Id"], (string)reader["FirstName"], (string)reader["LastName"],
                            (string)reader["Username"], (string)reader["PasswordHash"],
                            (Guid)reader["CreatedBy"], (Guid)reader["UpdatedBy"], (DateTime)reader["DateCreated"],
                            (DateTime)reader["DateUpdated"], (bool)reader["IsActive"]);
                        loggedUser.UserRole = await GetRoleByIdAsync((Guid)reader["RoleId"]);
                    }

                    return loggedUser;
                }
                catch (Exception e)
                {
                    connection.Close();
                    return null;
                }
            }
        }

        public void Dispose()
        {
            return;
        }
    }
}
