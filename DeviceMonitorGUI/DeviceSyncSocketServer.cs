using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DeviceMonitorGUI
{
    public class DeviceSyncSocketServer
    {
        // Thread signal.  
        public ManualResetEvent allDone = new ManualResetEvent(false);
        public IPAddress deviceIP;
        public Int16 portNumber;
        public DeviceState deviceState;
        public DeviceInfo deviceInfo;


        public DeviceSyncSocketServer(string deviceIP, Int16 portNumber, DeviceInfo deviceInfo)
        {
            this.deviceIP = IPAddress.Parse(deviceIP);
            this.portNumber = portNumber;
            deviceState = DeviceState.OFFLINE;
            this.deviceInfo = deviceInfo;
            Console.WriteLine("New device constructed");
        }

        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] receiveBuffer = new Byte[1024];
            string receiveInfo = null;
            // Establish the local endpoint for the socket.  
            IPEndPoint localEndPoint = new IPEndPoint(deviceIP, portNumber);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(deviceIP.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                Console.WriteLine("Listening now");
                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    Console.WriteLine("Finished conneciton...");
                    //Set the amount of time to wait for received bytes to 10 seconds
                    handler.ReceiveTimeout = 10000;
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
                                    break;
                                }
                            }
                        }
                        catch (SocketException e)
                        {
                            //Check if the exception is caused by a device not reponding
                            if (receiveInfo.Equals(null))
                            {
                                Console.WriteLine("Receive Timed Out");
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                //Break out of the while loop
                                break;
                            }
                            else
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                        //2. Parse received data
                        string data = receiveInfo.Substring(0, receiveInfo.Length - 5);
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
                            }
                            else if (checkData[0].Equals('D'))
                            {
                                //If it is a disconnect request
                                Console.WriteLine("Disconnect request received from " + this.deviceIP.ToString());
                                deviceState = DeviceState.DISCONNECTING;
                                //TODO: IMPLEMENT TIMEOUT ON DISCONNECT
                                while (deviceState == DeviceState.DISCONNECTING) ;
                                //Echo disconnect request back
                                char[] d = {' '};
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}