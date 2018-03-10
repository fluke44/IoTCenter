using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.DbAccess.DataAccess.Writers;
using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;
using IoTCenter.Logging;
using IoTCenter.Service;
using System;
using System.Linq;

namespace IoTCenter.Devices.Handlers
{
    public class DeviceCommander
    {
        private readonly DeviceCommandQueueHandler _queueHandler;
        private readonly DeviceReader _devReader;

        public DeviceCommander()
        {
            _queueHandler = new DeviceCommandQueueHandler();
            _devReader = new DeviceReader();
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

        public IDeviceData ExecuteCommand(IDevice device, IDeviceCommand cmd)
        {
            return ExecuteCommandInternal(device, cmd);
        }

        public IDeviceData ExecuteCommandAsync(IDevice device, IDeviceCommand cmd)
        {
            _queueHandler.AddCommand(device.Mac, cmd);
            return GetRecentData(device, cmd);
        }

        private IDeviceData ExecuteCommandInternal(IDevice device, IDeviceCommand cmd)
        {
            //bool isAsyncCommand = cmd.Id != 0 && cmd.Status == CommandStatus.Pending;

            try
            {
                cmd = _queueHandler.AddCommand(device.Mac, cmd);
                cmd = Tcp.GetResponse(device, cmd);
                cmd.Success = true;

                cmd.Status = CommandStatus.Complete;
                UpdateCommand(cmd);
            }
            catch (Exception ex)
            {
                cmd.Error = ex;
                cmd.Success = false;

                cmd.Status = CommandStatus.Failed;
                UpdateCommand(cmd);

                //if (isAsyncCommand)
                //{
                //    cmd.Status = CommandStatus.Failed;
                //    UpdateCommand(cmd);
                //}
            }

            return new DeviceData(cmd.Result, DateTime.Now, cmd.Command, cmd.Error?.Message);
        }

        public void UpdateCommand(IDeviceCommand command)
        {
            try
            {
                _queueHandler.UpdateCommandStatus(command);
            }
            catch(Exception ex)
            {
                ErrorHandler.Log(ex);
            }
        }

        public void RunPendingCommands(IDevice device)
        {
            var pendingCommands = _queueHandler.GetPendingCommands(device);
            var deviceToExecute = _queueHandler.GetDeviceWithCommands(pendingCommands);

            ExecuteCommands(deviceToExecute);
        }

        public IDeviceData GetRecentData(IDevice device, IDeviceCommand cmd)
        {
            return _devReader.GetMostRecentDeviceData(device.Mac, cmd.Command);   
        }
    }
}
