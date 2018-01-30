using IoTCenter.Domain.Enum;
using IoTCenter.Service;
using IoTCenter.DbAccess.DataAccess.Writers;
using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.Domain.Interface;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Net;

namespace IoTCenter.Registration
{
    public class RegistrationHandler
    {
        public ICollection<Domain.Device> RegisteredDevices;

        public RegistrationHandler()
        {
            RegisteredDevices = new List<Domain.Device>();
        }

        public void RegisterDevice(Domain.Device device)
        {
            var writer = new DeviceWriter();
            writer.AddDevice(device);

            Udp.SendMessage(device.Ip, Domain.Device.Port, device.ConfirmRegistrationMessage);
        }

        public void PingDevice(IDevice device)
        {
            //var writer = new DeviceWriter();
            //writer.AddDevice(device);

            Udp.SendMessage(device.Ip, Domain.Device.Port, Domain.Device.Ping);
        }

        public void SetRegistrationStatus(IDevice device)
        {
            var writer = new DeviceWriter();
            writer.RegisterDevice(device);
        }

        public ICollection<IDevice> GetDevices(bool onlyRegistered)
        {
            var devices = new List<IDevice>();
            var reader = new DeviceReader();
            var data = onlyRegistered ? reader.GetRegisteredDevices() : reader.GetAllDevices();

            foreach(var line in data)
            {
                devices.Add(new Domain.Device()
                {
                    Id = line.Id,
                    Name = line.Name,
                    Mac = line.Mac,
                    Ip = IPAddress.Parse(line.Ip),
                    Type = (DeviceType)Enum.Parse(typeof(DeviceType), line.Type, true),
                    Registered = line.Registered,
                    DateRegistered = line.DateRegistered.HasValue ? line.DateRegistered.Value : DateTime.MinValue
                });
            };

            //foreach(DataRow row in data.Rows)
            //{
            //    var device = new Device(
            //        Convert.ToString(row["Name"]),
            //        Convert.ToString(row["Mac"]),
            //        Convert.ToString(row["Ip"]),
            //        Convert.ToString(row["Type"]),
            //        Convert.ToBoolean(row["Registered"])
            //    );
            //    var date = row["DateRegistered"];
            //    if(date != DBNull.Value) device.DateRegistered = Convert.ToDateTime(date);

            //    devices.Add(device);
            //}

            return devices;
        }

        public void HandleRegistrations()
        {
            var devices = GetDevices(false);

            Parallel.ForEach<IDevice>(devices, (device) =>
            {
                PingDevice(device);
            });

            Thread.Sleep(5000);

            var unresponsiveDevices = devices.Where(x => !RegisteredDevices.Any(
                y => y.Mac.Equals(x.Mac, StringComparison.OrdinalIgnoreCase)));

            foreach (var device in RegisteredDevices)
            {
                device.Registered = true;
                SetRegistrationStatus(device);
            }

            foreach (var device in unresponsiveDevices)
            {
                device.Registered = false;
                SetRegistrationStatus(device);
            }

            RegisteredDevices.Clear();
        }
    }
}
