using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IoTCenter.Domain.Interface;
using IoTCenter.Service;
using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.Domain;
using IoTCenter.Domain.Model;

namespace IoTCenter.Devices.Devices
{
    public abstract class SensorBase : Device
    {
        protected readonly DeviceReader DevReader;

        protected SensorBase()
        {
            DevReader = new DeviceReader();
        }

        protected abstract string DefaultCommand { get; }

        public abstract string Read();

        protected abstract string ParseData(string data);
    }
}
