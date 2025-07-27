using MovieApplicationDataBase.Movies;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MovieApplicationDataBase.Repositories
{
    public class ObjectsRepository
    {
        public Guid AddObject(object obj)
        {
            var sql = "INSERT INTO objects_store (obj) VALUES (:obj) returning id into :result";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    command.Parameters.Add("obj", OracleDbType.Blob, ObjectToByteArray(obj), ParameterDirection.Input);
                    command.Parameters.Add("result", OracleDbType.Raw, 16, null, ParameterDirection.ReturnValue);
                    command.ExecuteNonQuery();
                    return new Guid(((OracleBinary)command.Parameters["result"].Value).Value);
                }
            }
        }

        public void DeleteObject(Guid id)
        {
            var sql = $"DELECT FROM objects_store WHERE id = {id}";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public object GetObject(Guid id) 
        {
            var sql = $"Select obj FROM objects_store WHERE id = :id";
            using (var connection = DBAdapter.GetDBConnection())
            {
                connection.Open();
                using (var command = new OracleCommand(sql, connection))
                {
                    command.Parameters.Add("id", OracleDbType.Raw, 16, id.ToByteArray(), ParameterDirection.Input);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return ByteArrayToObject(reader.GetOracleBlob(reader.GetOrdinal("obj")).Value);
                        }
                    }
                }
            }

            return null;
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }
}
