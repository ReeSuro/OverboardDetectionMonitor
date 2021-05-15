using System;
using System.Collections.Generic;
using System.Text;
using MOBAL.Networking;
namespace DetectionConsole
{
    class MonitorProgram
    {
        public static int Main(String[] args)
        {
            DeviceSyncSocketServer device1 = new DeviceSyncSocketServer("192.168.1.100",11000);
            device1.StartListening();
            return 0;
        }
    }

  
}
