using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IoTCenter.Domain.Interface;
using IoTCenter.Service;
using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.Domain;
using IoTCenter.Domain.Model;
using IoTCenter.Devices.Handlers;
using IoTCenter.Domain.Enum;

namespace IoTCenter.Devices.Devices
{
    public abstract class SensorBase : DeviceBase
    {
        protected SensorBase() : this(new Device())
        {
        }

        protected SensorBase(IDevice device) : base(device)
        {
        }

        public IDeviceData Read()
        {
            return Execute(CommandName.Data);
        }

        public IDeviceData Read(CommandName commandName)
        {
            return Execute(commandName);
        }
    }
}
