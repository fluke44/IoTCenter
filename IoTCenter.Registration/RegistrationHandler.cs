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
using IoTCenter.Domain;

namespace IoTCenter.Registration
{
    public class RegistrationHandler
    {
        public ICollection<IDevice> RegisteredDevices;

        public RegistrationHandler()
        {
            RegisteredDevices = new List<IDevice>();
        }

        public void RegisterDevice(IDevice device)
        {
            var writer = new DeviceWriter();
            writer.AddDevice(device);

            Udp.SendMessage(device.Ip, Device.Port, device.ConfirmRegistrationMessage);

            //if(device.Sleeping) 
        }

        public void PingDevice(IDevice device)
        {
            //var writer = new DeviceWriter();
            //writer.AddDevice(device);

            Udp.SendMessage(device.Ip, Device.Port, Device.Ping);
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
            //data = data.Where(x => !x.Sleeping).ToList();

            devices.AddRange(data);

            //foreach(var line in data)
            //{
            //    devices.Add(new Device()
            //    {
            //        Id = line.DeviceId,
            //        Name = line.Name,
            //        Mac = line.Mac,
            //        Ip = line.Ip,
            //        Type = line.Type,
            //        SubType = line.s
            //        Registered = line.Registered,
            //        DateRegistered = line.DateRegistered.HasValue ? line.DateRegistered.Value : DateTime.MinValue
            //    });
            //};

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
            var devices = GetDevices(false).Where(x => !x.Sleeping);

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
