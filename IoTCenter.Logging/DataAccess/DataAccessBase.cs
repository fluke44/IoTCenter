using System.Data.SqlClient;

namespace IoTCenter.Logging.DataAccess
{
    public abstract class DataAccessBase
    {
        protected string ConnectionString;

        protected DataAccessBase()
        {
            var cs = new SqlConnectionStringBuilder();
            cs.DataSource = @"JOUDA-PC\SQLEXPRESS";
            cs.InitialCatalog = "IoTCenter";
            cs.IntegratedSecurity = true;
            ConnectionString = cs.ToString();
        }
    }
}
