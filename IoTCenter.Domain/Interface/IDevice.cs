using System;
using System.Net;
using IoTCenter.Domain.Enum;

namespace IoTCenter.Domain.Interface
{
    public interface IDevice
    {
        int Id { get; set; }

        bool Registered { get; set; }

        string Name { get; set; }

        string Mac { get; set; }

        IPAddress Ip { get; set; }

        DeviceType Type { get; set; }

        DateTime DateRegistered { get; set; }

        string ConfirmRegistrationMessage { get; }
    }
}
