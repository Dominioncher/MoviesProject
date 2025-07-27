using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplicationDataBase
{
    public static class DBAdapter
    {
        private static string connectionString;

        static DBAdapter()
        {
            var connection = new OracleConnectionStringBuilder();
            connection.UserID = "video_rent";
            connection.Password = "video_rent";
            connection.DataSource = "localhost:1521/XEPDB1";
            connectionString = connection.ConnectionString;
        }

        public static OracleConnection GetDBConnection()
        {
            return new OracleConnection(connectionString);
        }
    }
}
