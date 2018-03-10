using System;

namespace IoTCenter.Domain.Interface
{
    public interface IDeviceData
    {
        DateTime DateReceived { get; set; }

        string Command { get; set; }

        string Data { get; set; }

        bool Success { get; }

        string Error { get; set; }
    }
}
