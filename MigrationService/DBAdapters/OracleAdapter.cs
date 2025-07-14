using MigrationService.Exceptions;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MigrationService.DBAdapters
{
    public class OracleAdapter
    {
        private OracleConnection connection;

        public OracleAdapter(OracleConnection connection)
        {
            this.connection = connection;
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
                              'PACKAGE BODY'
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

        public void CreateServiceTables()
        {            
            var sql = "CREATE TABLE MIGRATION_HISTORY (" +
                "migration VARCHAR2(100) PRIMARY KEY, " +
                "start_date TIMESTAMP NOT NULL, " +
                "end_date TIMESTAMP, " +
                "status NUMBER(1) NOT NULL )";
            using (var command = new OracleCommand(sql, connection))
            {
                var created = command.ExecuteNonQuery();
            }

            var sql2 = "CREATE TABLE MIGRATION_HEAD (head VARCHAR2(100))";
            using (var command = new OracleCommand(sql2, connection))
            {
                var created = command.ExecuteNonQuery();
            }

            var sql3 = "INSERT INTO MIGRATION_HEAD VALUES( NULL )";            
            using (var command = new OracleCommand(sql3, connection))
            {
                var created = command.ExecuteNonQuery();
            }
        }

        public void StartMigration(string migrationName)
        {
            var sqlHistory = $"INSERT INTO Migration_history (migration, start_date, status) VALUES (" +
            $"'{migrationName}', " +
            $"'{DateTime.Now}', " +
            $"0)";

            try
            {
                using (var command = new OracleCommand(sqlHistory, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                if (ex.Number == 900)
                {
                    throw new DuplicateMigrationException($"Duplicate migration {migrationName}");
                }

                throw;
            }
        }

        public void ExecuteMigrationQuieries(params string[] quieries)
        {
            foreach (var quierie in quieries)
            {
                try
                {
                    using (var command = new OracleCommand(quierie, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception($"Quierie failed {quierie}", ex);
                }
            }
        }

        public void MigrationSuccess(string migrationName)
        {
            var sqlHistory = $"UPDATE Migration_history SET " +
                $"end_date = '{DateTime.Now}', " +
                $"status = 1 " +
                $"WHERE migration = '{migrationName}'";

            using (var command = new OracleCommand(sqlHistory, connection))
            {
                command.ExecuteNonQuery();
            }

            var sqlHead = $"UPDATE MIGRATION_HEAD SET head = '{migrationName}'";
            using (var command = new OracleCommand(sqlHead, connection))
            {
                command.ExecuteNonQuery();
            }

        }

        public void MigrationFail(string migrationName)
        {
            var sql = $"MERGE INTO Migration_history USING dual ON (migration='{migrationName}') " +
                "WHEN MATCHED THEN UPDATE SET " +
                $"end_date = '{DateTime.Now}' " +
                $"WHEN NOT MATCHED THEN INSERT VALUES ('{migrationName}', '{DateTime.Now}', '{DateTime.Now}', 0)";

            using (var command = new OracleCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public string GetMigrationsHead()
        {
            var sql = "select * FROM MIGRATION_HEAD";
            using (var command = new OracleCommand(sql, connection))
            {
                var head = command.ExecuteScalar();
                return head?.ToString();
            }
        }
    }
}
