using System;
using System.Collections.Generic;
using System.Text;

namespace DetectionConsole
{
    public enum DeviceState
    {
        CONNECTED, 
        CONNECTING, 
        OFFLINE, 
        DISCONNECTING,
        NOTRESPONDING,
        UNDEFINED
    }
}
