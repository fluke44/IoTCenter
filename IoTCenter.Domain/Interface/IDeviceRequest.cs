using IoTCenter.Domain.Enum;
using System.Net;

namespace IoTCenter.Domain.Interface
{
    public interface IDeviceRequest
    {
        IDevice Device { get; set; }

        UdpAction Action { get; set; }

        string Data { get; set; }
    }
}
