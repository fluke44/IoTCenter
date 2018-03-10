using IoTCenter.Logging.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCenter.Logging
{
    public static class ErrorHandler
    {
        private static ErrorLogWriter _writer;

        static ErrorHandler()
        {
            _writer = new ErrorLogWriter();
        }

        public static void Log(Exception ex)
        {
            try
            {
                _writer.Log(ex.StackTrace, ex.Message);

                if (ex.InnerException != null)
                {
                    _writer.Log(ex.InnerException.StackTrace, ex.InnerException.Message);
                }
            }
            catch
            {
            }
        }
    }
}
