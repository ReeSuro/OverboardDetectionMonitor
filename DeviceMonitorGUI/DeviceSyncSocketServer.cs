using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DeviceMonitorGUI
{
    public class DeviceSyncSocketServer
    {
        public IPAddress deviceIP;
        public Int16 portNumber;
        public DeviceState deviceState;
        public DeviceInfo deviceInfo;
        public DateTime lastPingTime;
        public bool serverRunning;

        public DeviceSyncSocketServer(string deviceIP, Int16 portNumber, DeviceInfo deviceInfo)
        {
            this.deviceIP = IPAddress.Parse(deviceIP);
            this.portNumber = portNumber;
            this.deviceInfo = deviceInfo;
            this.deviceState = DeviceState.OFFLINE;
            this.lastPingTime = DateTime.Now;
            this.serverRunning = true;
        }

        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] receiveBuffer;
            string receiveInfo;
            // Establish the local endpoint for the socket.  
            IPEndPoint localEndPoint = new IPEndPoint(deviceIP, portNumber);
            Socket listener = new Socket(deviceIP.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = 10000 //Make socket timeout 60 seconds
            };
            listener.Bind(localEndPoint);
            // Bind the socket to the local endpoint and listen for incoming connections 
            while (serverRunning)
            { 
                try
                {
                    // Create a TCP/IP socket.
                    listener.Listen(1);
                    receiveBuffer = new Byte[255];
                    receiveInfo = null;
                    // Start listening for connections.  
                    while (true)
                    {
                        Console.WriteLine("Waiting for a connection...");
                        // Program is suspended while waiting for an incoming connection.  
                        Socket handler = listener.Accept();
                        this.lastPingTime = DateTime.Now;
                        Console.WriteLine("Finished connection...");
                        //Set the amount of time to wait for received bytes to 10 seconds
                        handler.ReceiveTimeout = 30000;
                        //Set the device state to connected
                        deviceState = DeviceState.CONNECTED;
                        int numberOfReceiveBytes = 0;
                        //Once the socket is connected, manage the device state 
                        while (true)
                        {
                            //1. Wait for Receive
                            try
                            {
                                while (true)
                                {
                                    numberOfReceiveBytes = handler.Receive(receiveBuffer);
                                    receiveInfo = Encoding.ASCII.GetString(receiveBuffer, 0, numberOfReceiveBytes);
                                    
                                    if (receiveInfo.IndexOf("<EOF>") > -1)
                                    {
                                        //Check that there are not multiple commands 
                                        string[] commands = receiveInfo.Split(new string[] { "<EOF>" }, StringSplitOptions.RemoveEmptyEntries);
                                        receiveInfo = commands[0];
                                        break;
                                    }
                                }
                                //2. Parse received data
                                string data = receiveInfo;
                                //Check Checksum
                                int index = data.IndexOf('#');
                                String toParse = data.Substring(index + 1);
                                int checksum = Int32.Parse(toParse);
                                char[] checkData = data.Substring(0, index).ToCharArray();
                                int sum = 0;
                                for (int i = 0; i < checkData.Length; i++)
                                {
                                    sum += checkData[i];
                                }
                                //If the checksum is correct then process the data
                                if ((sum % 255) == checksum)
                                {

                                    if (checkData[0].Equals('A'))
                                    {
                                        //If it is a ping 
                                        Console.WriteLine("Ping from " + deviceIP.ToString());
                                        this.deviceInfo.deviceBattery = receiveInfo.Substring(1, 1);
                                        //update ping time
                                        lastPingTime = DateTime.Now;
                                    }
                                    else if (checkData[0].Equals('D'))
                                    {
                                        //If it is a disconnect request
                                        Console.WriteLine("Disconnect request received from " + this.deviceIP.ToString());
                                        deviceState = DeviceState.DISCONNECTING;
                                        //TODO: IMPLEMENT TIMEOUT ON DISCONNECT
                                        while (deviceState == DeviceState.DISCONNECTING) ;
                                        //Echo disconnect request back
                                        char[] d = { ' ' };
                                        if (deviceState == DeviceState.OFFLINE)
                                        {
                                            d[0] = 'D';
                                            handler.Send(Encoding.ASCII.GetBytes(d));
                                            //Start polling for another connection
                                            handler.Shutdown(SocketShutdown.Both);
                                            handler.Close();
                                        }
                                        else
                                        {
                                            d[0] = 'F';
                                            handler.Send(Encoding.ASCII.GetBytes(d));
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Unknown message format received");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("CheckSum failed for ping from " + this.deviceIP.ToString());
                                }
                            }
                            catch (SocketException e)
                            {
                                //If an exception occurs then close the handler and begin listeneing again.
                                handler.Close();
                                handler = null;
                                break;
                            }
                            catch (FormatException e)
                            {
                                Console.WriteLine("Message received error: " + receiveInfo);
                            }
                        }
                    }
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("2:" + e.ToString());
                }
            }
            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}