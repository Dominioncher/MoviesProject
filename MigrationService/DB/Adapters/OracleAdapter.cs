using MigrationService.DB.Contexts;
using MigrationService.Exceptions;
using Oracle.ManagedDataAccess.Client;

namespace MigrationService.DB.Adapters
{
    public class OracleAdapter : IDBAdapter
    {
        private OracleConnection connection;

        public OracleAdapter(IDBContext context)
        {
            connection = new OracleConnection(context.GetConnectionString());
        }


        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }


        public void ClearSchema()
        {
            var sql = @"BEGIN
   FOR cur_rec IN (SELECT object_name, object_type
                   FROM user_objects
                   WHERE object_type IN
                             ('TABLE',
                              'VIEW',
                              'MATERIALIZED VIEW',
                              'PACKAGE',
                              'PROCEDURE',
                              'FUNCTION',
                              'SEQUENCE',
                              'SYNONYM',
                              'PACKAGE BODY',
                              'TYPE'
                             ))
   LOOP
      BEGIN
         IF cur_rec.object_type = 'TABLE'
         THEN
            EXECUTE IMMEDIATE 'DROP '
                              || cur_rec.object_type
                              || ' ""'
                              || cur_rec.object_name
                              || '"" CASCADE CONSTRAINTS';
         ELSE
            EXECUTE IMMEDIATE 'DROP '
                              || cur_rec.object_type
                              || ' ""'
                              || cur_rec.object_name
                              || '""';
         END IF;
      EXCEPTION
         WHEN OTHERS
         THEN
            DBMS_OUTPUT.put_line ('FAILED: DROP '
                                  || cur_rec.object_type
                                  || ' ""'
                                  || cur_rec.object_name
                                  || '""'
                                 );
      END;
   END LOOP;
   FOR cur_rec IN (SELECT * 
                   FROM all_synonyms 
                   WHERE table_owner IN (SELECT USER FROM dual))
   LOOP
      BEGIN
         EXECUTE IMMEDIATE 'DROP PUBLIC SYNONYM ' || cur_rec.synonym_name;
      END;
   END LOOP;
END;";

            using (var command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void CreateServiceTables()
        {
            var transaction = connection.BeginTransaction();

            var sql = "CREATE TABLE MIGRATION_HISTORY (" +
                "migration VARCHAR2(100) PRIMARY KEY, " +
                "start_date TIMESTAMP NOT NULL, " +
                "end_date TIMESTAMP, " +
                "status NUMBER(1) NOT NULL )";
            using (var command = new OracleCommand(sql, connection))
            {
                command.Transaction = transaction;
                var created = command.ExecuteNonQuery();
            }

            var sql2 = "CREATE TABLE MIGRATION_HEAD (head VARCHAR2(100))";
            using (var command = new OracleCommand(sql2, connection))
            {
                command.Transaction = transaction; 
                var created = command.ExecuteNonQuery();
            }

            var sql3 = "INSERT INTO MIGRATION_HEAD VALUES( NULL )";            
            using (var command = new OracleCommand(sql3, connection))
            {
                command.Transaction = transaction;
                var created = command.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        public bool CheckServiceTablesExists()
        {
            var sql = "Select table_Name from user_Tables Where table_name = 'MIGRATION_HISTORY'";

            using (var command = new OracleCommand(sql, connection))
            {
                var table = command.ExecuteScalar();
                if (table != null)
                {
                    return true;
                }
            }

            return false;
        }


        public void ClearFailedBeforeMigrate()
        {
            var sql = "SELECT migration, end_date FROM MIGRATION_HISTORY WHERE status = 0";
            using (var command = new OracleCommand(sql, connection))
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var broken = reader.IsDBNull(reader.GetOrdinal("end_date"));
                    if (!broken)
                    {
                        continue;
                    }

                    var name = reader.GetString(reader.GetOrdinal("migration"));
                    throw new Exception($"Detected broken migration status for {name} you need manually check changes and change migration status in DB");
                }
            }
            

            var sql2 = "DELETE FROM MIGRATION_HISTORY WHERE status = 0";
            using (var command = new OracleCommand(sql2, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public string GetMigrationsHead()
        {
            var sql = "select * FROM MIGRATION_HEAD";
            using var command = new OracleCommand(sql, connection);
            var head = command.ExecuteScalar();
            return head?.ToString();
        }


        public void ExecuteMigration(string migrationName, string sql)
        {
            try
            {
                StartMigrate(migrationName);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1)
                {
                    throw new DuplicateMigrationException();
                }

                throw;
            }

            var transaction = connection.BeginTransaction();
            try
            {
                ExecuteQuerie(GetQuerieFromRawSql(sql), transaction);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            EndMigrate(migrationName, transaction);
            transaction.Commit();
        }

        public void LogFail(string migrationName)
        {
            var sql = $"MERGE INTO Migration_history USING dual ON (migration='{migrationName}') " +
                "WHEN MATCHED THEN UPDATE SET " +
                $"end_date = '{DateTime.Now}' " +
                $"WHEN NOT MATCHED THEN INSERT VALUES ('{migrationName}', '{DateTime.Now}', '{DateTime.Now}', 0)";

            using var command = new OracleCommand(sql, connection);
            command.ExecuteNonQuery();
        }


        private void StartMigrate(string migrationName)
        {
            var sqlHistory = $"INSERT INTO Migration_history (migration, start_date, status) VALUES (" +
            $"'{migrationName}', " +
            $"'{DateTime.Now}', " +
            $"0)";

            using var command = new OracleCommand(sqlHistory, connection);
            command.ExecuteNonQuery();
        }

        private void EndMigrate(string migrationName, OracleTransaction transaction)
        {
            var sqlHistory = $"UPDATE Migration_history SET " +
                $"end_date = '{DateTime.Now}', " +
                $"status = 1 " +
                $"WHERE migration = '{migrationName}'";
            using (var command = new OracleCommand(sqlHistory, connection))
            {
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }

            var sqlHead = $"UPDATE MIGRATION_HEAD SET head = '{migrationName}'";
            using (var command = new OracleCommand(sqlHead, connection))
            {
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
        }

        private void ExecuteQuerie(string querie, OracleTransaction transaction)
        {
            using var command = new OracleCommand(querie, connection);
            command.Transaction = transaction;
            command.ExecuteNonQuery();
        }

        private string GetQuerieFromRawSql(string sql)
        {

            sql = sql.TrimEnd();
            if (!sql.Contains("begin", StringComparison.CurrentCultureIgnoreCase) && sql.Last() == ';')
            {
                sql = sql.Remove(sql.Length - 1, 1);
            }
            return sql;
        }
    }
}
