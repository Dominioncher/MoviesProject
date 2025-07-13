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
    public class JanresRepository
    {
        public List<DBGanres> GetGanres()
        {
            var sql = "SELECT * FROM GANRES";
            var janres = new List<DBGanres>();
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var janre = new DBGanres
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            janres.Add(janre);
                        }
                    }
                }
            }
            return janres;
        }
    }
}
