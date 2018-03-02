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
        public void EnqueueCommand(string mac, IDeviceCommand cmd)
        {
            var command = new SqlCommand("Devices.spEnqueueCommand");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", mac),
                new SqlParameter("@Command", cmd.Command),
                new SqlParameter("@Url", cmd.Url)
            };

            ExecuteProcedure(command, parameters);
        }

        public void UpdateStatus(int cmdId, string status)
        {
            var command = new SqlCommand("Devices.spUpdateCommandStatus");
            SqlParameter[] parameters =
            {
                new SqlParameter("@CommandId", cmdId),
                new SqlParameter("@Status", status)
            };

            ExecuteProcedure(command, parameters);
        }
    }
}
