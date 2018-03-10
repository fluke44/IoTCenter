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
using IoTCenter.DbAccess.DataAccess.Readers;
using IoTCenter.Devices.Handlers;
using IoTCenter.Domain.Enum;

namespace IoTCenter.Ajax
{
    public partial class Switches : System.Web.UI.Page
    {
        protected ICollection<ISwitch> SwitchList { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request["action"] != null)
            {
                switch (Convert.ToString(Request["action"]).ToLower())
                {
                    case "switchstate":
                        var deviceId = Convert.ToInt32(Request["deviceId"]);
                        var device = new DeviceReader().GetDeviceById(deviceId);
                        var cmdName = CommandName.SwitchOn;
                        if (Convert.ToString(Request["state"]).Equals("1", StringComparison.OrdinalIgnoreCase)) cmdName = CommandName.SwitchOff;
                        new DeviceCommander().ExecuteCommand(device, Commands.Get(cmdName));
                        break;
                }
            }
            
            var devices = new RegistrationHandler().GetDevices(false);
            SwitchList = new List<ISwitch>();

            foreach (IDevice device in devices)
            {
                if (device.Type == Domain.Enum.DeviceType.Switch)
                {
                    IDevice dev = new SwitchShield(device);
                    SwitchShield swt = dev as SwitchShield;
                    SwitchList.Add(swt);
                }
            }
        }

        protected string GetCssClassForDevice(IDevice sensor, IDeviceData data)
        {
            var status = sensor.IsOnline && data.Success ? "activeDevice" : "inactiveDevice";
            var type = string.Empty;
            switch (sensor.SubType)
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

        protected string IsSleeping(ISwitch sensor)
        {
            return sensor.Sleeping ? "sleeping" : "";
        }

        protected string GetCssClassForStatus(ISwitch sensor, IDeviceData data)
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

        protected bool IsToggleOn(IDeviceData data)
        {
            return data.Data.ToString().Equals("ON", StringComparison.OrdinalIgnoreCase);
        }
    }
}