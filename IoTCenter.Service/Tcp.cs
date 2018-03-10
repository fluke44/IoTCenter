using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain;
using System.IO;
using IoTCenter.DbAccess.DataAccess.Writers;
using System.Diagnostics;

namespace IoTCenter.Service
{
    public static class Tcp
    {
        public static IDeviceCommand GetResponse(IDevice device, IDeviceCommand cmd, int timeout = 10000)
        {
            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{device.Ip}/{cmd.Url}");
            request.Timeout = timeout;
            request.Credentials = CredentialCache.DefaultCredentials;

            var sw = new Stopwatch();
            sw.Start();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                sw.Stop();
                cmd.ResponseTime = sw.ElapsedMilliseconds;
                cmd.StatusCode = response.StatusCode;

                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    responseText = reader.ReadToEnd();
                    new DeviceWriter().LogData(Convert.ToString(device.Mac), Convert.ToString(responseText));
                };
            };

            cmd.Result = responseText;

            return cmd;
        }
    }
}
