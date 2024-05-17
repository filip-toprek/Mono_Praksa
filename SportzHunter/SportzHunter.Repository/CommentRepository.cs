using Npgsql;
using SportzHunter.Common;
using SportzHunter.Model;
using SportzHunter.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SportzHunter.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<Comment>> GetCommentsAsync(Guid tournamentId, Paging paging)
        {
            List<Comment> comments = new List<Comment>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string commentsQuery = "SELECT \"Comment\".\"Id\", \"Comment\".\"PlayerId\", \"Comment\".\"Text\", \"Comment\".\"CreatedBy\", " +
                    "\"Comment\".\"UpdatedBy\", \"Comment\".\"DateUpdated\", \"Comment\".\"DateCreated\", \"Comment\".\"IsActive\", \"User\".\"Username\" " +
                    "FROM \"Comment\" INNER JOIN \"Player\" ON \"Comment\".\"PlayerId\" = \"Player\".\"Id\" INNER JOIN \"User\" ON \"Player\".\"UserId\" = \"User\".\"Id\" WHERE \"TournamentId\" = @Id AND \"Comment\".\"IsActive\" = @isActive ORDER BY \"DateCreated\" DESC";

                if(paging.PageNumber > 0 && paging.PageSize > 0)
                {
                    commentsQuery += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                }

                NpgsqlCommand command = new NpgsqlCommand(commentsQuery, connection);
                command.Parameters.AddWithValue("@Id", tournamentId);
                command.Parameters.AddWithValue("@isActive", true);

                if (paging.PageNumber > 0 && paging.PageSize > 0)
                {
                    command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                    command.Parameters.AddWithValue("PageSize", paging.PageSize);
                }

                connection.Open();

                await command.ExecuteNonQueryAsync();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read() && reader.HasRows)
                {
                    comments.Add(new Comment()
                    {
                        Id = (Guid)reader["Id"],
                        PlayerId = (Guid)reader["PlayerId"],
                        Text = (string)reader["Text"],
                        TournamentId = tournamentId,
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"],
                        Player = new Player() { User = new User() { Username = (string)reader["Username"] } }
                    });
                }
            }

            int totalCount = comments.Count;

            return new PagedList<Comment>(comments, paging.PageSize, totalCount);
        }

        public async Task<Comment> GetCommentByIdAsync(Guid commentId)
        {
            Comment comment = null;

            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                string commentQuery = "SELECT * FROM \"Comment\" WHERE \"Comment\".\"Id\" = @CommentId";
                NpgsqlCommand cmd = new NpgsqlCommand(commentQuery, conn);
                cmd.Parameters.AddWithValue("@CommentId", commentId);

                conn.Open();

                await cmd.ExecuteNonQueryAsync();
                NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                if(reader.HasRows && reader.Read())
                {
                    comment = new Comment
                    {
                        Id = (Guid)reader["Id"],
                        PlayerId = (Guid)reader["PlayerId"],
                        Text = (string)reader["Text"],
                        TournamentId = (Guid)reader["TournamentId"],
                        CreatedBy = (Guid)reader["CreatedBy"],
                        UpdatedBy = (Guid)reader["UpdatedBy"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        DateUpdated = (DateTime)reader["DateUpdated"],
                        IsActive = (bool)reader["IsActive"]
                    };
                }
            }

            return comment;
        }

        public async Task<bool> AddCommentAsync(Comment comment)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string commentQuery = "INSERT INTO \"Comment\" VALUES (@Id, @PlayerId, @Text, @TournamentId, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";
                NpgsqlCommand command = new NpgsqlCommand(commentQuery, connection);

                command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                command.Parameters.AddWithValue("@PlayerId", comment.PlayerId);
                command.Parameters.AddWithValue("@Text", comment.Text);
                command.Parameters.AddWithValue("@TournamentId", comment.TournamentId);
                command.Parameters.AddWithValue("@CreatedBy", comment.PlayerId);
                command.Parameters.AddWithValue("@UpdatedBy", comment.PlayerId);
                command.Parameters.AddWithValue("@DateCreated", comment.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", comment.DateUpdated);
                command.Parameters.AddWithValue("@IsActive", true);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                int commentAdded = await command.ExecuteNonQueryAsync();

                if (commentAdded != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string commentQuery = "UPDATE \"Comment\" SET \"IsActive\" = @IsActive, \"UpdatedBy\" = @UpdatePerson, \"DateUpdated\" = @DateNow WHERE \"Id\" = @Id";

                NpgsqlCommand command = new NpgsqlCommand(commentQuery, connection);
                command.Parameters.AddWithValue("@IsActive", false);
                command.Parameters.AddWithValue("@UpdatePerson", comment.PlayerId);
                command.Parameters.AddWithValue("@DateNow", comment.DateUpdated);
                command.Parameters.AddWithValue("@Id", comment.Id);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                int commentDeleted = await command.ExecuteNonQueryAsync();

                if (commentDeleted != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> UpdateCommentAsync(Guid commentId, Comment comment)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string commentQuery = "UPDATE \"Comment\" SET \"Text\" = @Text, \"DateUpdated\" = @DateUpdated, \"UpdatedBy\" = @UpdatePerson, \"IsActive\" = @isActive WHERE \"Id\"= @Id";

                NpgsqlCommand command = new NpgsqlCommand(commentQuery, connection);
                command.Parameters.AddWithValue("@Text", comment.Text);
                command.Parameters.AddWithValue("@DateUpdated", comment.DateUpdated);
                command.Parameters.AddWithValue("@UpdatePerson", comment.PlayerId);
                command.Parameters.AddWithValue("@isActive", comment.IsActive);
                command.Parameters.AddWithValue("@Id", commentId);

                connection.Open();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                int commentUpdated = await command.ExecuteNonQueryAsync();

                if (commentUpdated != 0)
                {
                    transaction.Commit();
                    return true;
                }
            }

            return false;
        }
    }
}
