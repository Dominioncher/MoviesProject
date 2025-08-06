using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    public class OracleOptions
    {
        public string Host { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string ConnectionString => GetConnectionString();

        private string GetConnectionString()
        {
            var builder = new OracleConnectionStringBuilder
            {
                UserID = User,
                Password = Password,
                DataSource = Host
            };
            return builder.ConnectionString;
        }
    }
}
