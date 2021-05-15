using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DetectionConsole;

namespace MOBAL.Networking
{
    public class DeviceSyncSocketServer
    {
        // Thread signal.  
        public ManualResetEvent allDone = new ManualResetEvent(false);
        public IPAddress deviceIP;
        public Int16 portNumber;
        public DeviceState deviceState;


        public DeviceSyncSocketServer(string deviceIP, Int16 portNumber)
        {
           
            this.deviceIP = IPAddress.Parse(deviceIP);
            this.portNumber = portNumber;
            deviceState = DeviceState.OFFLINE;
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
                            numberOfReceiveBytes = handler.Receive(receiveBuffer);
                            receiveInfo = Encoding.ASCII.GetString(receiveBuffer, 0, numberOfReceiveBytes);
                        }
                        catch (SocketException e)
                        {
                            //Check if the exception is caused by a device not reponding
                            if (receiveInfo.Equals(null))
                            {
                                Console.WriteLine("Receive Timed Out");
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                //Break out of thewhile loop
                                break;
                            }
                            else
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                        //2. Parse received data
                        if (receiveInfo.Equals("A"))
                        {
                            //If it is a ping 
                            Console.WriteLine("Ping from " + deviceIP.ToString());
                        }
                        else if (receiveInfo.Equals("D"))
                        {
                            //If it is a disconnect request
                            Console.WriteLine("Disconnect request received");
                            deviceState = DeviceState.OFFLINE;
                            //Echo disconnect request back
                            handler.Send(receiveBuffer);
                            //Start polling for another connection
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                            break;
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