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
using IoTCenter.Domain.Model;
using IoTCenter.Domain.Interface;
using IoTCenter.Domain.Enum;
using IoTCenter.Devices.Handlers;

namespace MulticastListener
{
    class Program
    {
        private const int Port = 8603;

        private static readonly RegistrationHandler _regHandler;
        private static readonly DeviceWriter _devWriter;
        private static readonly DeviceCommandQueueHandler _cmdHandler;

        static Program()
        {
            _regHandler = new RegistrationHandler();
            _devWriter = new DeviceWriter();
            _cmdHandler = new DeviceCommandQueueHandler();
        }

        static void Main(string[] args)
        {
            Timer timer = new Timer(30000);
            timer.Elapsed += new ElapsedEventHandler(PingDevices);
            timer.Start();

            var client = StartUdpClient();

            // Wait for any key to terminate application
            Console.ReadKey();

            client.Close();
        }

        private static UdpClient StartUdpClient()
        {
            IPEndPoint remoteSender = new IPEndPoint(IPAddress.Any, 0);

            // Create UDP client
            UdpClient client = new UdpClient(Port);
            UdpState state = new UdpState(client, remoteSender);
            // Start async receiving
            client.BeginReceive(new AsyncCallback(DataReceived), state);

            return client;
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
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} :: {receivedText}");

                    ProcessRequest(receivedText, receivedIpEndPoint.Address);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
            finally
            {
                // Restart listening for udp data packages
                c.BeginReceive(new AsyncCallback(DataReceived), ar.AsyncState);
            }        
        }

        private static void ProcessRequest(string requestString, IPAddress ip)
        {
            IDeviceRequest request = new DeviceRequest(requestString, ip);
            IDevice device = request.Device;

            switch (request.Action)
            {
                case UdpAction.RegistrationRequest:
                    device.Registered = true;
                    _regHandler.RegisterDevice(request.Device);
                    _cmdHandler.RunPendingCommands(device);
                    break;
                case UdpAction.Ping:
                    device.Registered = true;
                    _regHandler.RegisteredDevices.Add(device);
                    break;
                case UdpAction.Data:
                    device.Registered = true;
                    _devWriter.LogData(device.Name, device.Mac);
                    break;
                default:
                    Console.WriteLine($"Unknown action from ip {ip}");
                    break;
            }
        }

        private static void PingDevices(object source, ElapsedEventArgs e)
        {
            _regHandler.HandleRegistrations();
        }

        //private static void ParseRequest(string text)
        //{
        //    //if (!Constants.UdpActions.Any(x => text.Contains(x))) return;

        //    Device device;
        //    string[] data;
        //    if (text.Contains("|")) data = text.Split('|'); else data = new string[] { text };
        //    switch(data[0].ToUpper())
        //    {
        //        case "REGISTRATION_REQ":
        //            device = new Device(text) { Registered = true };
        //            _regHandler.RegisterDevice(device);
        //            break;
        //        case "PING":
        //            device = new Device() { Registered = true, Mac = Convert.ToString(data[1]) };
        //            _regHandler.RegisteredDevices.Add(device);
        //            break;
        //        case "DATA":
        //            device = new Device() { Registered = true, Mac = Convert.ToString(data[1]) };
        //            _devWriter.LogData(Convert.ToString(data[1]), Convert.ToString(data[2]));
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
