using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.Devices.Handlers;
using IoTCenter.Domain;
using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;
using System;

namespace IoTCenter.Devices.Devices
{
    public abstract class DeviceBase : Device
    {
        //protected readonly DeviceCommandQueueHandler Queue;
        protected abstract IDeviceData ParseData(IDeviceData data);
        //protected readonly DeviceReader DevReader;
        protected readonly DeviceCommander Commander;

        protected DeviceBase() : this(new Device())
        {
        }

        protected DeviceBase(IDevice device) : base(device)
        {
            //DevReader = new DeviceReader();
            //Queue = new DeviceCommandQueueHandler();
            Commander = new DeviceCommander();
        }

        protected IDeviceData Execute(CommandName commandName, bool async = true)
        {
            var data = new DeviceData();

            try
            {
                var command = Commands.Get(commandName);

                if (Sleeping || async)
                {
                    return ParseData(Commander.ExecuteCommandAsync(this, command));
                }
                else
                {
                    if (Registered && !async)
                    {
                        //new DeviceCommander().ExecuteCommand(this, command);
                        var result = Commander.ExecuteCommand(this, command);
                        return ParseData(result);
                    }

                    if (!Registered || !command.Success)
                    {
                        return ParseData(Commander.GetRecentData(this, command));
                    }

                    return ParseData(new DeviceData() { Data = command.Result, DateReceived = DateTime.Now });
                }
            }
            catch (Exception ex)
            {
                data.Error = ex.Message;
            }

            return data;
        }
    }
}
