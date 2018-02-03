using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;
using IoTCenter.Service;
using System;

namespace IoTCenter.Devices.Handlers
{
    public class DeviceCommander
    {
        public void ExecuteCommands(IDevice device)
        {
            foreach(DeviceCommand cmd in device.Commands)
            {
                ExecuteCommand(device, cmd);
            }
        }

        public void ExecuteCommand(IDevice device, DeviceCommand cmd)
        {
            try
            {
                cmd.Result = Tcp.GetResponse(device, cmd.Url);
            }
            catch (Exception ex)
            {
                cmd.Error = ex;
            }
        }
    }
}
