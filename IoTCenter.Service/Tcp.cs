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

namespace IoTCenter.Service
{
    public static class Tcp
    {
        public static string GetResponse(IDevice device, string command, int timeout = 10000)
        {
            string responseText = string.Empty;
            WebRequest request = WebRequest.Create($"http://{device.Ip}/{command}");
            request.Timeout = timeout;
            request.Credentials = CredentialCache.DefaultCredentials;

            using (WebResponse response = request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    responseText = reader.ReadToEnd();
                    new DeviceWriter().LogData(Convert.ToString(device.Mac), Convert.ToString(responseText));
                };
            };

            return responseText;
        }
    }
}
