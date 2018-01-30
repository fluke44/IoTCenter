using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using IoTCenter.Registration;
using IoTCenter.Domain;
using System.Timers;
using IoTCenter.Service;
using IoTCenter.DbAccess.DataAccess.Writers;

namespace MulticastListener
{
    class Program
    {
        private const int Port = 8603;

        private static RegistrationHandler _regHandler;
        private static DeviceWriter _devWriter;

        static void Main(string[] args)
        {
            _regHandler = new RegistrationHandler();

            Timer timer = new Timer(30000);
            timer.Elapsed += new ElapsedEventHandler(PingDevices);
            timer.Start();

            IPEndPoint remoteSender = new IPEndPoint(IPAddress.Any, 0);

            // Create UDP client
            UdpClient client = new UdpClient(Port);
            UdpState state = new UdpState(client, remoteSender);
            // Start async receiving
            client.BeginReceive(new AsyncCallback(DataReceived), state);

            // Wait for any key to terminate application
            Console.ReadKey();
            client.Close();
        }

        private static void DataReceived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)((UdpState)ar.AsyncState).c;

            try
            {
                IPEndPoint wantedIpEndPoint = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
                IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = c.EndReceive(ar, ref receivedIpEndPoint);

                // Check sender
                bool isRightHost = (wantedIpEndPoint.Address.Equals(receivedIpEndPoint.Address))
                    || wantedIpEndPoint.Address.Equals(IPAddress.Any);
                bool isRightPort = (wantedIpEndPoint.Port == receivedIpEndPoint.Port)
                    || wantedIpEndPoint.Port == 0;
                if (isRightHost && isRightPort)
                {
                    // Convert data to ASCII and print in console
                    string receivedText = ASCIIEncoding.ASCII.GetString(receiveBytes);
                    Console.WriteLine(receivedText);

                    ParseRequest(receivedText);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                // Restart listening for udp data packages
                c.BeginReceive(new AsyncCallback(DataReceived), ar.AsyncState);
            }        
        }

        private static void ParseRequest(string text)
        {
            if (!Constants.UdpActions.Any(x => text.Contains(x))) return;

            Device device;
            string[] data;
            if (text.Contains("|")) data = text.Split('|'); else data = new string[] { text };
            switch(data[0].ToUpper())
            {
                case "REGISTRATION_REQ":
                    device = new Device(text) { Registered = true };
                    _regHandler.RegisterDevice(device);
                    break;
                case "PING":
                    device = new Device() { Registered = true, Mac = Convert.ToString(data[1]) };
                    _regHandler.RegisteredDevices.Add(device);
                    break;
                case "DATA":
                    device = new Device() { Registered = true, Mac = Convert.ToString(data[1]) };
                    _devWriter.LogData(Convert.ToString(data[1]), Convert.ToString(data[2]));
                    break;
                default:
                    break;
            }
        }

        private static void PingDevices(object source, ElapsedEventArgs e)
        {
            _regHandler.HandleRegistrations();
        }
    }
}
