using IoTCenter.Domain.Interface;
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
        public void EnqueueCommand(IDevice device)
        {
            var command = new SqlCommand("Devices.spEnqueueCommand");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", device.Mac),
                new SqlParameter("@Command", device),
                new SqlParameter("@Url", device.Type)
            };

            ExecuteProcedure(command, parameters);
        }
    }
}
