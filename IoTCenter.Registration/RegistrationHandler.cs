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
using IoTCenter.Logging;

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
            try
            {
                var writer = new DeviceWriter();
                writer.AddDevice(device);

                Udp.SendMessage(device.Ip, Device.Port, device.ConfirmRegistrationMessage);
            }
            catch(Exception ex)
            {
                ErrorHandler.Log(ex);
                throw;
            }
        }

        public void PingDevice(IDevice device)
        {
            try
            {
                Udp.SendMessage(device.Ip, Device.Port, Device.Ping);
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(ex);
                throw;
            }
        }

        public void SetRegistrationStatus(IDevice device)
        {
            try
            {
                var writer = new DeviceWriter();
                writer.RegisterDevice(device);
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(ex);
                throw;
            }
        }

        public ICollection<IDevice> GetDevices(bool onlyRegistered)
        {
            try
            {
                var devices = new List<IDevice>();
                var reader = new DeviceReader();
                var data = onlyRegistered ? reader.GetRegisteredDevices() : reader.GetAllDevices();

                devices.AddRange(data);

                return devices;
            }
            catch (Exception ex)
            {
                ErrorHandler.Log(ex);
                throw;
            }
        }

        public void HandleRegistrations()
        {
            try
            {
                var devices = GetDevices(false).Where(x => !x.Sleeping);

                Parallel.ForEach(devices, (device) =>
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
            catch (Exception ex)
            {
                ErrorHandler.Log(ex);
                throw;
            }
        }
    }
}
