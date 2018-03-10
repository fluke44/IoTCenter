using IoTCenter.Logging.DataAccess;

namespace IoTCenter.Logging
{
    public static class EventLogger
    {
        private static IncomingMessageWriter _writer;

        static EventLogger()
        {
            _writer = new IncomingMessageWriter();
        }

        public static void LogIncomingMessage(string message)
        {
            try
            {
                _writer.Log(message);
            }
            catch
            {
            }
        }
    }
}
