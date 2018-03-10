using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace IoTCenter.Logging.DataAccess
{
    public class ErrorLogWriter : DataAccessBase
    {
        public void Log(string stackTrace, string message = null, string parameters = null, string className = null, string functionName = null)
        {
            var command = new SqlCommand("Logging.spLogError");

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@StackTrace", stackTrace)
            };

            if (message != null) sqlParameters.Add(new SqlParameter("@Message", message));
            if (parameters != null) sqlParameters.Add(new SqlParameter("@Parameters", parameters));
            if (className != null) sqlParameters.Add(new SqlParameter("@ClassName", className));
            if (functionName != null) sqlParameters.Add(new SqlParameter("@FunctionName", functionName));

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
