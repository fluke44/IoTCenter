using IoTCenter.DbAccess.DataAccess.Writers;
using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;
using IoTCenter.Service;
using System;
using System.Linq;

namespace IoTCenter.Devices.Handlers
{
    public class DeviceCommander
    {
        //private readonly DeviceCommandQueueHandler _queue;
        private readonly CommandQueueWriter _queueWriter;

        public DeviceCommander()
        {
            //_queue = new DeviceCommandQueueHandler();
            _queueWriter = new CommandQueueWriter();
        }

        public void ExecuteCommands(IDevice device)
        {
            if (device == null || device.CommandList == null) return;

            foreach(DeviceCommand cmd in device.CommandList)
            {
                if(cmd.Status == CommandStatus.Pending)
                {
                    ExecuteCommand(device, cmd);
                    if (!cmd.Success) Console.WriteLine(cmd.Error);
                }
            }
        }

        public void ExecuteCommand(IDevice device)
        {
            if (device == null || device.CommandList == null) return;

            ExecuteCommand(device, device.CommandList.FirstOrDefault());
        }

        public void ExecuteCommand(IDevice device, IDeviceCommand cmd)
        {
            bool isAsyncCommand = cmd.Id != 0 && cmd.Status == CommandStatus.Pending;

            try
            {
                if (isAsyncCommand || cmd.Status == CommandStatus.Unknown)
                {
                    cmd.Result = Tcp.GetResponse(device, cmd.Url);
                    cmd.Success = true;

                    if(isAsyncCommand)
                    {
                        UpdateCommandStatus(cmd.Id, CommandStatus.Complete);
                    }
                }
            }
            catch (Exception ex)
            {
                cmd.Error = ex;
                cmd.Success = false;

                if (isAsyncCommand)
                {
                    UpdateCommandStatus(cmd.Id, CommandStatus.Failed);
                }
            }
        }

        public void UpdateCommandStatus(int cmdId, CommandStatus status)
        {
            _queueWriter.UpdateStatus(cmdId, status.ToString());
        }
    }
}
