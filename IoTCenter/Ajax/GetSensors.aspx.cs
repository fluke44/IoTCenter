using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IoTCenter.Domain;
using IoTCenter.Domain.Interface;
using IoTCenter.Registration;
using IoTCenter.Devices.Devices;
using System.Text;
using System.Globalization;

namespace IoTCenter.Ajax
{
    public partial class GetSensors : System.Web.UI.Page
    {
        protected ICollection<IDevice> Devices { get; set; }
        protected ICollection<ISensor> Sensors { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Devices = new RegistrationHandler().GetDevices(false);
            Sensors = new List<ISensor>();

            foreach (IDevice device in Devices)
            {
                if (device.Type == Domain.Enum.DeviceType.Sensor)
                {
                    IDevice dev = new Sht30(device);
                    Sht30 sht = dev as Sht30;
                    Sensors.Add(sht);
                }
            }
        }

        protected string GetCssClassForDevice(ISensor sensor, IDeviceData data)
        {
            var status = sensor.IsOnline && data.Success ? "activeDevice" : "inactiveDevice";
            var type = string.Empty;
            switch(sensor.SubType)
            {
                case Domain.Enum.DeviceSubType.Sht30:
                    type = "tempSensor";
                    break;
                case Domain.Enum.DeviceSubType.Shield:
                    type = "toggle";
                    break;
            }
            return $"device {status} {type}";
        }

        protected string IsSleeping(ISensor sensor)
        {
            return sensor.Sleeping ? "sleeping" : "";
        }

        protected string GetCssClassForStatus(ISensor sensor, IDeviceData data)
        {
            return sensor.IsOnline ? "online" : "offline";
        }

        protected string Battery(IDeviceData data)
        {
            if (data.Data == null) return string.Empty;

            var number = data.Data.Replace("V", string.Empty);
            if (string.IsNullOrWhiteSpace(number)) return string.Empty;

            var raw = double.Parse(number, CultureInfo.InvariantCulture);
            if (raw > 4.15) return "bat4";
            if (raw > 4.00) return "bat3";
            if (raw > 3.85) return "bat2";
            if (raw > 3.70) return "bat1";
            return "bat0";
        }
    }
}