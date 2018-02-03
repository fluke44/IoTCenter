using System.Text;
using System.Net;
using System.Collections.Generic;
using IoTCenter.Domain.Enum;
using System;
using System.Linq;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Model;

namespace IoTCenter.Domain
{
    public class Device : IDevice
    {
        public const int Port = 8603;
        private const string ConfirmRegistrationTemplate = "REGISTRATION_OK|{0}|{1}";
        public const string Ping = "PING";

        public Device()
        {
            Commands = new List<DeviceCommand>();
        }

        public Device(string requestData) : this()
        {
            var data = requestData.Split('|');
            Name = data[1];
            Mac = data[2];
            Ip = IPAddress.Parse(data[3]);
            Type = (DeviceType)System.Enum.Parse(typeof(DeviceType), data[4]);
            SubType = (DeviceSubType)System.Enum.Parse(typeof(DeviceSubType), data[5]);
            Sleeping = Convert.ToBoolean(data[6]);
        }

        public Device(string name, string mac, string ip, string type, string subType, bool registered, bool sleeping = false) : this()
        {
            Name = name;
            Mac = mac;
            Ip = IPAddress.Parse(ip);
            Type = (DeviceType)System.Enum.Parse(typeof(DeviceType), type);
            SubType = (DeviceSubType)System.Enum.Parse(typeof(DeviceSubType), subType);
            Registered = registered;
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

        public ICollection<DeviceCommand> Commands { get; private set; }

        public bool HasFailedCommand => Commands.Any(x => !x.Success);

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
