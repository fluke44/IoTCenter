using IoTCenter.Domain.Enum;

namespace IoTCenter.Domain.Interface
{
    public interface ISwitch : IDevice
    {
        IDeviceData Read(CommandName commandName);

        void Write(CommandName commandName);
    }
}
