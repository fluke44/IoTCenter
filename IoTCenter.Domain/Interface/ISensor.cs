namespace IoTCenter.Domain.Interface
{
    public interface ISensor : IDevice
    {
        string Read();
    }
}
