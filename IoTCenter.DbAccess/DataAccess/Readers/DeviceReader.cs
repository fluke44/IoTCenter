using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using IoTCenter.DbAccess.IoTCenter.Devices;
using IoTCenter.DbAccess.IoTCenter;
using System.Data.SqlClient;

namespace IoTCenter.DbAccess.DataAccess.Readers
{
    public class DeviceReader : DataAccessBase
    {
        public ICollection<Device> GetRegisteredDevices()
        {
            using(var db = new IoTCenterContext())
            {
                var query = from devices in db.Device where devices.Registered select devices;
                return query.ToList();
            }
            //var command = new SqlCommand("SELECT * FROM Devices.Device WHERE Registered = 1");
            //return ExecuteQuery(command, new SqlParameter[] { });
        }

        public ICollection<Device> GetAllDevices()
        {
            using (var db = new IoTCenterContext())
            {
                var query = from devices in db.Device select devices;
                return query.ToList();
            }
            //var command = new SqlCommand("SELECT * FROM Devices.Device");
            //return ExecuteQuery(command, new SqlParameter[] { });
        }

        public ICollection<CachedDataView> GetCachedData(string mac)
        {
            using (var db = new IoTCenterContext())
            {
                var query = from devices 
                            in db.CachedDataView
                            where devices.Mac.Equals(mac, System.StringComparison.OrdinalIgnoreCase)
                            orderby devices.DateLogged descending
                            select devices;

                return query.ToList();
            }
        }

        public string GetMostRecentDeviceData(string mac)
        {
            var command = new SqlCommand("Devices.spGetMostRecentDeviceData");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", mac)
            };

            var result = ExecuteProcedureWithReturn(command, parameters);
            if(result.Rows.Count > 0)
            {
                return Convert.ToString(result.Rows[0]["Data"]);
            }

            return string.Empty;
        }
    }
}
