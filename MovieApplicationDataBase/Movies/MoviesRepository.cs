using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplicationDataBase.Movies
{
    public class MoviesRepository
    {
        public List<DBMovie> GetAllMovies()
        {
            var sql = "SELECT * FROM MOVIES_VIEW";
            var movies = new List<DBMovie>();
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                // Load Movies
                using (var command = new OracleCommand(sql, connection))
                {
                    command.Transaction = transaction;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var movie = new DBMovie
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Image = reader.IsDBNull(reader.GetOrdinal("Photo")) ? null : ImageHelpers.FromByteArray(reader.GetOracleBlob(reader.GetOrdinal("Photo")).Value)
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Release_date")))
                            {
                                movie.ReleaseDate = reader.GetDateTime(reader.GetOrdinal("Release_date"));
                            }
                            movies.Add(movie);
                        }
                 
                    }
                }

                if (!movies.Any())
                {
                    transaction.Commit();
                    return movies;
                }

                // Load Movies Janres
                var janresSQL = $"SELECT * FROM MOVIES_JANRES JOIN JANRES ON (MOVIES_JANRES.JANRE_ID = JANRES.ID) WHERE MOVIE_ID IN ({string.Join(", ", movies.Select(x => x.ID))})";
                using (var command = new OracleCommand(janresSQL, connection))
                {
                    command.Transaction = transaction;
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var movieID = reader.GetInt32(reader.GetOrdinal("Movie_id"));

                            var janre = new DBJanres
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            movies.First(x => x.ID == movieID).Janres.Add(janre);
                        }
                    }
                }
                transaction.Commit();
            }
            
            return movies;
        }

        public void IncreaceMovieView(int MovieID)
        {
            var sql = "MERGE INTO movies_statistics USING dual ON (movie_id=:id) " +
                "WHEN MATCHED THEN UPDATE SET views_count=views_count+1 " +
                "WHEN NOT MATCHED THEN INSERT VALUES (:id,1)";

            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    var parameters = new OracleParameter[] {
                     new OracleParameter("id", MovieID)
                    };
                    command.Parameters.AddRange(parameters);
                    var upsertrows = command.ExecuteNonQuery();
                }
            }

        }

        public void UpdateMovie(DBMovie movie)
        {
            var sql = "UPDATE MOVIES_VIEW " +
                "SET TITLE = :title, " +
                "DESCRIPTION = :description, " +
                "PHOTO = :photo, " +
                "RELEASE_DATE = :release " +
                $"WHERE ID = {movie.ID}";
            var deleteJanres = "DELETE FROM MOVIES_JANRES WHERE Movie_id = :id ";
            var insertJanres = "INSERT INTO MOVIES_JANRES VALUES (:id, :janre_id)";

            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                // Update Movie
                using (var command = new OracleCommand(sql, connection))
                {
                    command.Transaction = transaction;
                    var parameters = new OracleParameter[] {
                     new OracleParameter("title", movie.Title),
                     new OracleParameter("description", movie.Description),
                     new OracleParameter("photo", movie.Image.ToByteArray()),
                     new OracleParameter("release", movie.ReleaseDate),
                    };
                    command.Parameters.AddRange(parameters);
                    var updated = command.ExecuteNonQuery();
                }

                // Delete old Janres
                using (var command = new OracleCommand(deleteJanres, connection))
                {
                    command.Transaction = transaction;
                    var parameters = new OracleParameter[] {
                     new OracleParameter("id", (int)movie.ID)
                    };
                    command.Parameters.AddRange(parameters);
                    var deleted = command.ExecuteNonQuery();
                }

                // Insert new Janres
                foreach (var janre in movie.Janres)
                {                    
                    using (var command = new OracleCommand(insertJanres, connection))
                    {
                        command.Transaction = transaction;
                        var parameters = new OracleParameter[] {
                         new OracleParameter("id", (int)movie.ID),
                         new OracleParameter("janre_id", janre.ID)
                        };
                        command.Parameters.AddRange(parameters);
                        var inserted = command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
        }

        public void CreateMovie(DBMovie movie)
        {
            var sql = "INSERT INTO MOVIES_VIEW " +
                    "(TITLE, DESCRIPTION) " +
                     "VALUES (:title, :description)";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    var parameters = new OracleParameter[] {
                     new OracleParameter("title", movie.Title),
                     new OracleParameter("description", movie.Description)
                    };
                    command.Parameters.AddRange(parameters);
                    var inserted = command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveMovie(int MovieID)
        {
            var procedure = "DELETE_MOVIE";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(procedure, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameters = new OracleParameter[] {
                     new OracleParameter("p_movie_id", MovieID),
                     new OracleParameter("p_user_oid", "admin")
                    };
                    command.Parameters.AddRange(parameters);
                    var deleted = command.ExecuteNonQuery();
                }
            }
        }
    }
}
