using System;
using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;

namespace IoTCenter.Devices.Devices
{
    public class SwitchShield : SwitchBase, ISwitch
    {
        private const string On = "ON";
        private const string Off = "OFF";

        private string _data = string.Empty;

        public SwitchShield(IDevice device) : base(device)
        {
        }

        protected override IDeviceData ParseData(IDeviceData data)
        {
            var pair = data.Data.Split('|');
            switch(data.Command.ToLower())
            {
                case "state":
                    data.Data = ParseState(pair);
                    break;
            }

            return data;
        }

        private string ParseState(string[] data)
        {
            return data[2].Equals("1") ? "ON" : "OFF";
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_data))
                return "--";

            if (_data.Equals("1", StringComparison.OrdinalIgnoreCase)) return On;

            return Off;
        }
    }
}
