using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCenter.Domain
{
    public static class Constants
    {
        public static int SensorSleepingTime = 1800; // 30 minutes

        public static int SensorPingInterval = 60; // 1 minute

        public static string DateTimeFormat = "yyyy-mm-dd HH:mm:ss.fff";
    }
}
