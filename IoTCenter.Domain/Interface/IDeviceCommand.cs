using IoTCenter.Domain.Enum;
using System;

namespace IoTCenter.Domain.Interface
{
    public interface IDeviceCommand
    {
        int Id { get; set; }

        string Command { get; set; }

        string Url { get; set; }

        CommandStatus Status { get; set; }

        DateTime DateAdded { get; set; }

        string Result { get; set; }

        bool Success { get; set; }

        Exception Error { get; set; }
    }
}
