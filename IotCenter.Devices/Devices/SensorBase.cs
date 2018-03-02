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
    public abstract class SensorBase : Device
    {
        protected readonly DeviceReader DevReader;
        private readonly DeviceCommandQueueHandler _queue;

        protected abstract ISensorData ParseData(ISensorData data);

        protected SensorBase() : this(new Device())
        {
        }

        protected SensorBase(IDevice device) : base(device)
        {
            DevReader = new DeviceReader();
            _queue = new DeviceCommandQueueHandler();
        }

        public ISensorData Read()
        {
            return Read(CommandName.Data);
        }

        public ISensorData Read(CommandName commandName)
        {
            var data = new SensorData();

            try
            {
                var command = Commands.Get(commandName);

                if (Sleeping)
                {
                    _queue.AddCommand(Mac, command);
                    return GetRecentData(command);
                }
                else
                {
                    new DeviceCommander().ExecuteCommand(this, command);
                    if (!command.Success)
                    {
                        return GetRecentData(command);
                    }

                    return ParseData(new SensorData() { Data = command.Result, DateReceived = DateTime.Now });
                }
            }
            catch(Exception ex)
            {
                data.Error = ex.Message;
            }

            return data;
        }

        private ISensorData GetRecentData(IDeviceCommand cmd)
        {
            var data = DevReader.GetMostRecentDeviceData(Mac, cmd.Command);
            return ParseData(data);
        }
    }
}
