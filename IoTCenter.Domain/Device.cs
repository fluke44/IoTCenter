using System.Text;
using System.Net;
using System.Collections.Generic;
using IoTCenter.Domain.Enum;
using System;
using IoTCenter.Domain.Interface;

namespace IoTCenter.Domain
{
    public class Device : IDevice
    {
        public const int Port = 8603;
        private const string ConfirmRegistrationTemplate = "REGISTRATION_OK|{0}|{1}";
        public const string Ping = "PING";

        public Device()
        {   
        }

        public Device(string requestData)
        {
            var data = requestData.Split('|');
            Name = data[1];
            Mac = data[2];
            Ip = IPAddress.Parse(data[3]);
            Type = (DeviceType)System.Enum.Parse(typeof(DeviceType), data[4]);
            Sleeping = Convert.ToBoolean(data[5]);
        }

        public Device(string name, string mac, string ip, string type, bool registered, bool sleeping = false)
        {
            Name = name;
            Mac = mac;
            Ip = IPAddress.Parse(ip);
            Type = (DeviceType)System.Enum.Parse(typeof(DeviceType), type);
            Registered = registered;
            Sleeping = sleeping;
        }

        public int Id { get; set; }

        public bool Registered { get; set; }

        public string Name { get; set; }

        public string Mac { get; set; }

        public IPAddress Ip { get; set; }

        public DeviceType Type { get; set; }

        public DateTime DateRegistered { get; set; }

        public bool Sleeping { get; set; }

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
