using MovieApplicationDataBase.Repositories;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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
        ObjectsRepository _objectsRepository;

        public MoviesRepository()
        {
            _objectsRepository = new ObjectsRepository();
        }

        public List<DBMovie> GetAllMovies(int offset, int batchSize)
        {
            var sql = $"SELECT * FROM MOVIES_VIEW ORDER BY Title OFFSET {offset} ROWS FETCH NEXT {batchSize} ROWS ONLY";
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

                // Load Movies Ganres
                var ganresSQL = $"SELECT * FROM MOVIES_GANRES JOIN GANRES ON (MOVIES_GANRES.GANRE_ID = GANRES.ID) WHERE MOVIE_ID IN ({string.Join(", ", movies.Select(x => x.ID))})";
                using (var command = new OracleCommand(ganresSQL, connection))
                {
                    command.Transaction = transaction;
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var movieID = reader.GetInt32(reader.GetOrdinal("Movie_id"));

                            var ganre = new DBGanres
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                            movies.First(x => x.ID == movieID).Ganres.Add(ganre);
                        }
                    }
                }
                transaction.Commit();
            }
            
            return movies;
        }

        public List<Guid> GetMovieImages(int MovieID)
        {
            var sql = $"SELECT Image_guid FROM MOVIES_IMAGES WHERE movie_id = {MovieID}";
            var ids = new List<Guid>();
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ids.Add(new Guid(reader.GetOracleBinary(reader.GetOrdinal("Image_guid")).Value));
                        }
                    }
                }
            }

            return ids;
        }


        public int CreateMovie(string title)
        {
            var sql = "INSERT INTO MOVIES_VIEW " +
                     "(TITLE) " +
                     "VALUES (:title) " +
                     "returning ID into :result";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    var parameters = new OracleParameter[] {
                     new OracleParameter("title", title),
                     new OracleParameter("result", OracleDbType.Decimal, System.Data.ParameterDirection.ReturnValue)
                    };
                    command.Parameters.AddRange(parameters);
                    var inserted = command.ExecuteNonQuery();
                    return (int)((OracleDecimal)command.Parameters["result"].Value).Value;
                }
            }
        }

        public void AddMovieImages(int MovieID, List<Guid> images)
        {
            var sql = $"insert into MOVIES_IMAGES values(:movie, :image)";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                using (var command = new OracleCommand(sql, connection))
                {
                    command.Transaction = transaction;
                    foreach (var image in images)
                    {
                        var parameters = new OracleParameter[] {
                         new OracleParameter("movie", MovieID),
                         new OracleParameter("image", OracleDbType.Raw, 16, image, System.Data.ParameterDirection.Input)
                        };
                        command.Parameters.Clear();
                        command.Parameters.AddRange(parameters);
                        var inserted = command.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }
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
            var deleteJanres = "DELETE FROM MOVIES_GANRES WHERE Movie_id = :id ";
            var insertJanres = "INSERT INTO MOVIES_GANRES VALUES (:id, :janre_id)";

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
                foreach (var janre in movie.Ganres)
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

        public void RemoveMovieImages(int MovieID, List<Guid> images)
        {

        }
    }
}
