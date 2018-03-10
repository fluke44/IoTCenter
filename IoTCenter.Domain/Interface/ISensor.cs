using IoTCenter.Domain.Enum;

namespace IoTCenter.Domain.Interface
{
    public interface ISensor : IDevice
    {
        IDeviceData Read();

        IDeviceData Read(CommandName commandName);
    }
}
