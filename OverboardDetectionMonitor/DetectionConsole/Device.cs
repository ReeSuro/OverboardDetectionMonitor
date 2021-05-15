using System;
using System.Collections.Generic;
using System.Text;
using MOBAL.Networking;

namespace DetectionConsole
{
    class Device
    {
        private DeviceState state { get; set;}//An enumerator holding the state of the devices connection to a socket
        private Int16 portNumber { get; set; }//Holds the port to connect to
        private string deviceID { get; set; }//Holds a unique device ID
        private string deviceIP { get; set; }//Holds the local IP address of the device

        private DeviceSyncSocketServer server;

        public Device(DeviceState state, Int16 portNumber, string deviceID, string deviceIP) {
            this.state = state;
            this.portNumber = portNumber;
            this.deviceID = deviceID;
            this.deviceIP = deviceIP;
            //Construct Socket server



        }




    }
}
