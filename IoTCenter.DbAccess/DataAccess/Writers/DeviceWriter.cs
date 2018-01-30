using IoTCenter.Domain;
using IoTCenter.DbAccess.IoTCenter;
using IoTCenter.Domain.Interface;
using System.Data.SqlClient;

namespace IoTCenter.DbAccess.DataAccess.Writers
{
    public class DeviceWriter : DataAccessBase
    {
        public void AddDevice(IDevice device)
        {
            //new IoTCenterEntities().spAddDevice(device.Name, device.Mac, device.Ip.ToString(), device.Type.ToString());
            var command = new SqlCommand("Devices.spAddDevice");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Name", device.Name),
                new SqlParameter("@Mac", device.Mac),
                new SqlParameter("@Ip", device.Ip.ToString()),
                new SqlParameter("@Type", device.Type)
            };

            ExecuteProcedure(command, parameters);
        }

        public void RegisterDevice(IDevice device)
        {
            //new IoTCenterEntities().spRegisterDevice(device.Mac, device.Registered);
            var command = new SqlCommand("Devices.spRegisterDevice");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", device.Mac),
                new SqlParameter("@Register", device.Registered ? 1 : 0)
            };

            ExecuteProcedure(command, parameters);
        }

        public void LogData(string mac, string data)
        {
            var command = new SqlCommand("Devices.spLogData");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Mac", mac),
                new SqlParameter("@Data", data)
            };

            ExecuteProcedure(command, parameters);
        }
    }
}
