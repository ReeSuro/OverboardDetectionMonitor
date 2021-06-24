using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitorGUI
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
