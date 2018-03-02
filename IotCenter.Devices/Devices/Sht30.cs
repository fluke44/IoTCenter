using IoTCenter.Domain.Enum;
using IoTCenter.Domain.Interface;

namespace IoTCenter.Devices.Devices
{
    public class Sht30 : SensorBase, ISensor
    {
        private string[] _data = new string[2];

        public Sht30(IDevice device) : base(device)
        {
        }

        protected override ISensorData ParseData(ISensorData data)
        {
            var pair = data.Data.Split('|');
            switch(data.Command.ToLower())
            {
                case "data":
                    data.Data = ParseTempAndHum(pair);
                    break;
                case "bat":
                    data.Data = ParseBattery(pair);
                    break;
            }

            return data;
        }

        private string ParseTempAndHum(string[] data)
        {
            if (data.Length >= 4)
            {
                _data[0] = data[2];
                _data[1] = data[3];
            }

            return $"{_data[0]}°C | {_data[1]}%";
        }

        private string ParseBattery(string[] data)
        {
            return $"{data[2]}V";
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_data[0]) && string.IsNullOrEmpty(_data[1]))
                return "--";

            return $"{_data[0]}°C | {_data[1]}%";
        }
    }
}
