using System.Data.SqlClient;
using System.Data;

namespace IoTCenter.DbAccess.DataAccess
{
    public abstract class DataAccessBase
    {
        protected const string DbName = "IoTCenter";
        protected string ConnectionString;

        protected DataAccessBase()
        {
            var cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"JOUDA-PC\SQLEXPRESS";
            cs.InitialCatalog = "IoTCenter";
            cs.IntegratedSecurity = true;
            ConnectionString = cs.ToString();
        }

        protected void ExecuteProcedure(SqlCommand command, SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
        }

        protected DataTable ExecuteProcedureWithReturn(SqlCommand command, SqlParameter[] parameters)
        {
            var dt = new DataTable();

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
                command.Connection = conn;
                var reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dt.Load(reader);
                }

                return dt;
            }
        }

        protected int ExecuteProcedureWithReturnValue(SqlCommand command, SqlParameter[] parameters)
        {
            var dt = new DataTable();

            using (var conn = new SqlConnection(ConnectionString))
            {
                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);

                var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                command.Connection = conn;

                conn.Open();
                var reader = command.ExecuteReader();

                return (int)returnParameter.Value;
            }
        }

        protected DataTable ExecuteQuery(SqlCommand command, SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters);
                command.Connection = conn;

                var dt = new DataTable();
                dt.Load(command.ExecuteReader());

                return dt;
            }
        }
    }
}
