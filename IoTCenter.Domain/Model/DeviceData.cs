using IoTCenter.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCenter.Domain.Model
{
    public class DeviceData : IDeviceData
    {
        public DeviceData()
        {
        }

        public DeviceData(string data, DateTime dateReceived, string command = "", string error = "")
        {
            Data = data;
            DateReceived = dateReceived;
            Command = command;
            Error = error;
        }

        public DateTime DateReceived { get; set; }

        public string Command { get; set; }

        public string Data { get; set; }

        public bool Success => string.IsNullOrEmpty(Error);

        public string Error { get; set; }
    }
}
