using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using IoTCenter.DbAccess.IoTCenter.Devices;
using IoTCenter.DbAccess.IoTCenter;
using System.Data.SqlClient;
using IoTCenter.Domain;
using System.Net;
using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;

namespace IoTCenter.DbAccess.DataAccess.Readers
{
    public class DeviceReader : DataAccessBase
    {
        public ICollection<Device> GetRegisteredDevices()
        {
            //using(var db = new IoTCenterContext())
            //{
            //    var query = from devices in db.Device where devices.Registered select devices;
            //    return query.ToList();
            //}

            var list = new List<Device>();

            var command = new SqlCommand("SELECT * FROM Devices.vwDevices WHERE Registered = 1");
            var dt = ExecuteQuery(command, new SqlParameter[] { });

            foreach(DataRow row in dt.Rows)
            {
                list.Add(new Device(row));
            }

            return list;
        }

        public ICollection<Device> GetAllDevices()
        {
            //using (var db = new IoTCenterContext())
            //{
            //    var query = from devices in db.Device select devices;
            //    return query.ToList();
            //}
            var list = new List<Device>();

            var command = new SqlCommand("SELECT * FROM Devices.vwDevices");
            var dt = ExecuteQuery(command, new SqlParameter[] { });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Device(row));
            }

            return list;
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

        public ISensorData GetMostRecentDeviceData(string mac, string command)
        {
            var data = new SensorData();

            var cmd = new SqlCommand("Devices.spGetMostRecentDeviceData");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", mac),
                new SqlParameter("@Command", command)
            };

            var result = ExecuteProcedureWithReturn(cmd, parameters);
            if(result.Rows.Count > 0)
            {
                data.DateReceived = Convert.ToDateTime(result.Rows[0]["DateLogged"]);
                data.Data = Convert.ToString(result.Rows[0]["Data"]);
                data.Command = Convert.ToString(result.Rows[0]["Command"]);
            }
            else
            {
                data.Error = "No data";
            }

            return data;
        }
    }
}
