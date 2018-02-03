using IoTCenter.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCenter.DbAccess.DataAccess.Readers
{
    public class CommandQueueReader : DataAccessBase
    {
        public DataTable GetPendingCommands(IDevice device = null)
        {
            var command = new SqlCommand("Devices.spGetPendingCommands");

            return ExecuteQuery(command, device == null ? 
                new SqlParameter[] { } : 
                new SqlParameter[] { new SqlParameter("@Mac", device.Mac) } );
        }
    }
}
