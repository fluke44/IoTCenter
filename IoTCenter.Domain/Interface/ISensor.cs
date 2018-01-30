namespace IoTCenter.Domain.Interface
{
    public interface ISensor
    {
        string Command { get; }

        bool DataReceived { get; }

        IDevice Device { get; }

        string Read();

        string ReadCache();
    }
}
