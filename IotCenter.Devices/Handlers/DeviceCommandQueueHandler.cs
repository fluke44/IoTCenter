using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.Domain;
using IoTCenter.Domain.Model;
using IoTCenter.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoTCenter.DbAccess.DataAccess.Writers;
using IoTCenter.Domain.Enum;

namespace IoTCenter.Devices.Handlers
{
    public class DeviceCommandQueueHandler
    {
        private readonly CommandQueueReader _devQueue;
        private readonly CommandQueueWriter _queueWriter;
        //private readonly DeviceCommander _devCmd;

        public DeviceCommandQueueHandler()
        {
            _devQueue = new CommandQueueReader();
            _queueWriter = new CommandQueueWriter();
            //_devCmd = new DeviceCommander();
        }

        public IDeviceCommand AddCommand(string mac, IDeviceCommand cmd)
        {
            cmd.Id = _queueWriter.EnqueueCommand(mac, cmd);

            return cmd;
        }

        //public void RunPendingCommands(IDevice device)
        //{
        //    var pendingCommands = _devQueue.GetPendingCommands(device);
        //    var deviceToExecute = GetDeviceWithCommands(pendingCommands);

        //    _devCmd.ExecuteCommands(deviceToExecute);
        //}

        public DataTable GetPendingCommands(IDevice device = null)
        {
            return _devQueue.GetPendingCommands(device);
        }

        public void UpdateCommandStatus(IDeviceCommand command)
        {
            _queueWriter.UpdateStatus(command.Id, command.Status.ToString(), (int)command.StatusCode, command.ResponseTime, command.Error?.Message);
        }

        public IDevice GetDeviceWithCommands(DataTable dt)
        {
            IDevice device = null;

            foreach(DataRow row in dt.Rows)
            {
                if (device == null)
                {
                    device = new Device(row);
                }
                
                device.CommandList.Add(new DeviceCommand()
                {
                    Id = Convert.ToInt32(row["CommandQueueId"]),
                    Command = Convert.ToString(row["Command"]),
                    Url = Convert.ToString(row["Url"]),
                    Status = Convert.ToString(row["Status"]).FromDescription<CommandStatus>(),
                    DateAdded = Convert.ToDateTime(row["DateAdded"]),
                });
            }

            return device;
        }
    }
}
