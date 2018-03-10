using IoTCenter.Domain.Interface;
using System.Linq;
using IoTCenter.Domain.Enum;
using System.Net;
using System;

namespace IoTCenter.Domain.Model
{
    public class DeviceRequest : IDeviceRequest
    {
        private readonly IPAddress _ip;

        public DeviceRequest() { }

        public DeviceRequest(string request, IPAddress ip)
        {
            _ip = ip;
            Parse(request);
        }

        public IDevice Device { get; set; }

        public UdpAction Action { get; set; }

        public string Data { get; set; }

        private void Parse(string request)
        {
            string[] data;
            if (request.Contains("|")) data = request.Split('|'); else data = new string[] { request };

            if (data.Count() == 1)
            {
                Data = data[0];
                return;
            }

            Action = data[0].FromDescription<UdpAction>();

            switch(Action)
            {
                case UdpAction.RegistrationRequest:
                    Device = new Device(request);
                    Device.Ip = _ip;
                    break;
                case UdpAction.Ping:
                    Device = new Device();
                    Device.Mac = Convert.ToString(data[1]);
                    Device.Ip = _ip;
                    break;
                default:
                    Device = new Device();
                    Device.Ip = _ip;
                    break;
            }
        }
    }
}
