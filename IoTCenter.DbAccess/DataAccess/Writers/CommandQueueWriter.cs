using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCenter.DbAccess.DataAccess.Writers
{
    public class CommandQueueWriter : DataAccessBase
    {
        public int EnqueueCommand(string mac, IDeviceCommand cmd)
        {
            var command = new SqlCommand("Devices.spEnqueueCommand");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", mac),
                new SqlParameter("@Command", cmd.Command),
                new SqlParameter("@Url", cmd.Url)
            };

            return ExecuteProcedureWithReturnValue(command, parameters);
        }

        public void UpdateStatus(int cmdId, string status, int statusCode = 0, long responseTime = 0, string error = "")
        {
            var command = new SqlCommand("Devices.spUpdateCommand");

            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@CommandId", cmdId),
                new SqlParameter("@Status", status)
            };

            if (statusCode != 0) sqlParameters.Add(new SqlParameter("@StatusCode", statusCode));
            if (responseTime != 0) sqlParameters.Add(new SqlParameter("@ResponseTime", responseTime));
            if (!string.IsNullOrEmpty(error)) sqlParameters.Add(new SqlParameter("@Error", error));

            ExecuteProcedure(command, sqlParameters.ToArray());
        }
    }
}
