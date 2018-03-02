using System.ComponentModel;

namespace IoTCenter.Domain.Enum
{
    public enum UdpAction
    {
        [Description("")]
        Unknown,
        [Description("REGISTRATION_REQ")]
        RegistrationRequest,
        [Description("PING")]
        Ping,
        [Description("DATA")]
        Data
    }
}
