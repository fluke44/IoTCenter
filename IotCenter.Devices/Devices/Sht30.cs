using IoTCenter.Domain;
using IoTCenter.Domain.Interface;

namespace IoTCenter.Devices.Devices
{
    public class Sht30 : SensorBase, ISensor
    {
        private string[] _data = new string[2];

        protected override string DefaultCommand { get { return "/data"; } }

        public override string Read()
        {
            var data = DevReader.GetMostRecentDeviceData(Mac);

            return ParseData(data);
        }

        protected override string ParseData(string data)
        {
            var pair = data.Split('|');

            _data[0] = pair[2];
            _data[1] = pair[3];

            return $"{_data[0]}|{_data[1]}";
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_data[0]) && string.IsNullOrEmpty(_data[1]))
                return "--";

            return string.Format("Temp: {0}°C, Hum: {1}%", _data[0], _data[1]);
        }
    }
}
