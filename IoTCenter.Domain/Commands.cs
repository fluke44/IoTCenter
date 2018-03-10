using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;
using System.Collections.Generic;

namespace IoTCenter.Domain
{
    public static class Commands
    {
        private static IDictionary<CommandName, IDeviceCommand> _commandSpecs;

        static Commands()
        {
            _commandSpecs = new Dictionary<CommandName, IDeviceCommand>();
            PopulateCommands();
        }

        private static void PopulateCommands()
        {
            _commandSpecs.Add(CommandName.Data, new DeviceCommand { Command = "data", Url = "data" });
            _commandSpecs.Add(CommandName.Bat, new DeviceCommand { Command = "bat", Url = "bat" });
            _commandSpecs.Add(CommandName.State, new DeviceCommand { Command = "state", Url = "state" });
            _commandSpecs.Add(CommandName.SwitchOn, new DeviceCommand { Command = "switchOn", Url = "switch/on" });
            _commandSpecs.Add(CommandName.SwitchOff, new DeviceCommand { Command = "switchOff", Url = "switch/off" });
        }

        public static IDeviceCommand Get(CommandName commandName)
        {
            return _commandSpecs[commandName];
        }
    }
}
