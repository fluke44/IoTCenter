using System.Text;
using System.Net;
using System.Collections.Generic;
using IoTCenter.Domain.Enum;
using System;
using System.Linq;
using IoTCenter.Domain.Interface;
using System.Data;
using System.Globalization;

namespace IoTCenter.Domain
{
    public class Device : IDevice
    {
        public const int Port = 8603;
        private const string ConfirmRegistrationTemplate = "REGISTRATION_OK|{0}|{1}";
        public const string Ping = "PING";

        public Device()
        {
            CommandList = new List<IDeviceCommand>();
        }

        public Device(IDevice device) : this(
            device.Name,
            device.Mac,
            device.Ip.ToString(),
            device.Type.ToString(),
            device.SubType.ToString(),
            device.Registered,
            device.DateRegistered.ToString(CultureInfo.CurrentCulture),
            device.Sleeping
            )
        {
        }

        public Device(string requestData) : this(
            requestData.Split('|')[1], 
            requestData.Split('|')[2], 
            requestData.Split('|')[3], 
            requestData.Split('|')[4], 
            requestData.Split('|')[5],
            true,
            DateTime.Now.ToString(CultureInfo.CurrentCulture),
            Convert.ToInt16(requestData.Split('|')[6]) == 1 ? true : false)
        {
        }

        public Device(DataRow row) : this(
            Convert.ToString(row["Name"]),
            Convert.ToString(row["Mac"]),
            Convert.ToString(row["Ip"]),
            Convert.ToString(row["DeviceType"]),
            Convert.ToString(row["DeviceSubType"]),
            Convert.ToInt16(row["Registered"]) == 1 ? true : false,
            Convert.ToString(row["DateRegistered"]),
            Convert.ToInt16(row["Sleeping"]) == 1 ? true : false)
        {
        }

        public Device(string name, string mac, string ip, string type, string subType, bool registered, string dateRegistered, bool sleeping = false) : this()
        {
            CommandList = new List<IDeviceCommand>();
            Name = name;
            Mac = mac;
            Ip = IPAddress.Parse(ip);
            Type = (DeviceType)System.Enum.Parse(typeof(DeviceType), type);
            SubType = (DeviceSubType)System.Enum.Parse(typeof(DeviceSubType), subType);
            Registered = registered;
            DateRegistered = string.IsNullOrEmpty(dateRegistered) ? DateTime.MinValue : Convert.ToDateTime(dateRegistered, CultureInfo.CurrentCulture);
            //DateRegistered = DateTime.ParseExact(dateRegistered, Constants.DateTimeFormat, CultureInfo.InvariantCulture);
            Sleeping = sleeping;
        }

        public int Id { get; set; }

        public bool Registered { get; set; }

        public string Name { get; set; }

        public string Mac { get; set; }

        public IPAddress Ip { get; set; }

        public DeviceType Type { get; set; }

        public DeviceSubType SubType { get; set; }

        public DateTime DateRegistered { get; set; }

        public bool Sleeping { get; set; }

        public ICollection<IDeviceCommand> CommandList { get; private set; }

        public bool HasFailedCommand => CommandList.Any(x => !x.Success);

        public bool IsOnline
        {
            get
            {
                if (DateRegistered == null) return false;

                if (Sleeping)
                {
                    return DateTime.Now <= DateRegistered.AddSeconds(Constants.SensorSleepingTime);
                }

                return DateTime.Now <= DateRegistered.AddSeconds(Constants.SensorPingInterval);
            }
        }

        public string ConfirmRegistrationMessage
        {
            get
            {
                return string.Format(ConfirmRegistrationTemplate, Mac, Ip);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Name: {0}\n\n", Name);
            sb.AppendFormat("Mac: {0}\n\n", Mac);
            sb.AppendFormat("Ip address: {0}\n\n", Ip.ToString());
            sb.AppendFormat("Type: {0}\n\n", Type);

            return sb.ToString();
        }
    }
}
