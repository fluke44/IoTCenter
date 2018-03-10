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
    public abstract class SwitchBase : DeviceBase
    {
        protected SwitchBase() : this(new Device())
        {
        }

        protected SwitchBase(IDevice device) : base(device)
        {
        }

        public IDeviceData Read(CommandName commandName)
        {
            return Execute(commandName, false);
        }

        public void Write(CommandName commandName)
        {
            Execute(commandName, false);
        }
    }
}
