using IoTCenter.Domain.Enum;

namespace IoTCenter.Domain.Interface
{
    public interface ISensor : IDevice
    {
        ISensorData Read();

        ISensorData Read(CommandName commandName);
    }
}
