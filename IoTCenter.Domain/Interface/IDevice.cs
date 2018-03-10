using System;
using System.Net;
using IoTCenter.Domain.Enum;
using System.Collections.Generic;
using IoTCenter.Domain.Model;

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

        DeviceSubType SubType { get; set; }

        DateTime DateRegistered { get; set; }

        string ConfirmRegistrationMessage { get; }

        ICollection<IDeviceCommand> CommandList { get; }

        bool Sleeping { get; set; }

        bool HasFailedCommand { get; }

        bool IsOnline { get; }
    }
}
