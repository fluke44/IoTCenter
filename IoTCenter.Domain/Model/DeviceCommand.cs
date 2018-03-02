using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;
using System;

namespace IoTCenter.Domain.Model
{
    public class DeviceCommand : IDeviceCommand
    {
        public int Id { get; set; }

        public string Command { get; set; }

        public string Url { get; set; }

        public CommandStatus Status { get; set; }

        public DateTime DateAdded { get; set; }

        public string Result { get; set; }

        public bool Success { get; set; }

        public Exception Error { get; set; }
    }
}
