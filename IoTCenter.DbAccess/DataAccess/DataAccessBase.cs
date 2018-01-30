using System.Data.SqlClient;
using System.Data;

namespace IoTCenter.DbAccess.DataAccess
{
    public abstract class DataAccessBase
    {
        protected const string DbName = "IoTCenter";
        protected string ConnectionString;

        public DataAccessBase()
        {
            var cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"JOUDA-PC\SQLEXPRESS";
            cs.InitialCatalog = "IoTCenter";
            cs.IntegratedSecurity = true;
            ConnectionString = cs.ToString();
        }

        public void ExecuteProcedure(SqlCommand command, SqlParameter[] parameters)
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

        public DataTable ExecuteQuery(SqlCommand command, SqlParameter[] parameters)
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
