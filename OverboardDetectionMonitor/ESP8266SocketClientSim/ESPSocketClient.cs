using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MOBAL.Networking
{
    class ESPSocketClient
    {
        public IPAddress EndPointAddress;
        public int PortNumber;
        public Socket sender;
        public ESPSocketClient(string endPointIP, int portNumber) 
        {
            EndPointAddress = IPAddress.Parse(endPointIP);
            PortNumber = portNumber;
        }

        public void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPEndPoint remoteEP = new IPEndPoint(EndPointAddress, 11000);

                // Create a TCP/IP  socket.  
                 sender = new Socket(EndPointAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    //Attempt connection
                    sender.Connect(remoteEP);

                    while (true)
                    {
                        //send a ping constantly every 5 seconds
                        byte[] msg = Encoding.ASCII.GetBytes("A");
                        int bytesSent = sender.Send(msg);
                        Thread.Sleep(5000);
                    }    

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
          }
        }
    }
}
