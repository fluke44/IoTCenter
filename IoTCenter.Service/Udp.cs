using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace IoTCenter.Service
{
    public class Udp
    {
        public static void SendMessage(IPAddress ip, int port, string message)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress serverAddr = ip;

            IPEndPoint endPoint = new IPEndPoint(serverAddr, port);

            byte[] send_buffer = Encoding.ASCII.GetBytes(message);

            sock.SendTo(send_buffer, endPoint);
        }
    }
}
