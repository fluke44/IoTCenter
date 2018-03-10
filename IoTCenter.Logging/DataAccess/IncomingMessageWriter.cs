using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace IoTCenter.Logging.DataAccess
{
    public class IncomingMessageWriter : DataAccessBase
    {
        public void Log(string message)
        {
            var command = new SqlCommand("Logging.spLogMessage");

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Message", message)
            };

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(sqlParameters.ToArray());
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
        }
    }
}
