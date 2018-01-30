using System.Net;
using System.Net.Sockets;

namespace MulticastListener
{
    internal class UdpState
    {
        internal UdpState(UdpClient c, IPEndPoint e)
        {
            this.c = c;
            this.e = e;
        }

        internal UdpClient c;
        internal IPEndPoint e;
    }
}
