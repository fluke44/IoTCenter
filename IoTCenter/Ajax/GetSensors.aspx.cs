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

            foreach (var device in Devices)
            {
                if (device.Type == Domain.Enum.DeviceType.Sensor)
                {
                    Sensors.Add(new Sht30(device));
                }
            }
        }

        protected string GetCssClassForDevice(ISensor sensor)
        {
            if(sensor.DataReceived)
            {
                return "device activeDevice";
            }
            return "device inactiveDevice";
        }
    }
}