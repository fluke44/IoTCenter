using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IoTCenter.Domain.Interface;
using IoTCenter.Service;
using IoTCenter.DbAccess.DataAccess.Readers;

namespace IoTCenter.Devices.Devices
{
    public abstract class SensorBase
    {
        protected IDevice _device;

        public bool DataReceived { get; set; }

        public IDevice Device { get { return _device; } }

        public abstract string Read();

        protected abstract string ParseData(string data);

        public string Read(string command)
        {
            try
            {
                var data = Tcp.GetResponse(Device, command);
                DataReceived = true;
                return ParseData(data);
            }
            catch(Exception ex)
            {
                DataReceived = false;
                return $"Error: {ex.Message}";
            }
        }

        public string ReadCache()
        {
            var data = new DeviceReader().GetCachedData(Device.Mac);

            return ParseData(data.FirstOrDefault().Data);
        }
    }
}
