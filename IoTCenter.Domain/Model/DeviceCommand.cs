using System;

namespace IoTCenter.Domain.Model
{
    public class DeviceCommand
    {
        public int Id { get; set; }

        public string Command { get; set; }

        public string Url { get; set; }

        public string Status { get; set; }

        public DateTime DateAdded { get; set; }

        public string Result { get; set; }

        public bool Success { get; set; }

        public Exception Error { get; set; }
    }
}
