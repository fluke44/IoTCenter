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
            var list = new List<Device>();

            var command = new SqlCommand("SELECT * FROM Devices.vwDevices");
            var dt = ExecuteQuery(command, new SqlParameter[] { });

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Device(row));
            }

            return list;
        }

        public IDevice GetDeviceById(int id)
        {
            var command = new SqlCommand("SELECT * FROM Devices.vwDevices where DeviceId = @DeviceId");
            SqlParameter[] parameters =
            {
                new SqlParameter("@DeviceId", id)
            };
            var dt = ExecuteQuery(command, parameters);

            foreach (DataRow row in dt.Rows)
            {
                return new Device(row);
            }

            return new Device();
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

        public IDeviceData GetMostRecentDeviceData(string mac, string command)
        {
            var data = new DeviceData();

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
