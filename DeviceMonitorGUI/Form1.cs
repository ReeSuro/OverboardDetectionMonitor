using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;


namespace DeviceMonitorGUI
{
    public partial class MainForm : Form
    {
        List<DeviceSyncSocketServer> offlineDevices;
        List<DeviceSyncSocketServer> onlineDevices;
        List<DeviceSyncSocketServer> emergencyDevices;
        List<Task> taskList;
        Timer timer;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ////////GUI Init///////////
            //1. Disable all GUI fields
            DeviceGridBox.Enabled = false;
            LogBox.Enabled = true;
            LogBox.ReadOnly = true;

            ///////////Device init/////////////
            //1. Load device information from a CSV file 
            List<DeviceInfo> AllDeviceInfo = LoadDeviceInfo();
           
            /////////Server Init///////////////
            //1. Load each port by looping through each device.
            offlineDevices = new List<DeviceSyncSocketServer>();
            onlineDevices = new List<DeviceSyncSocketServer>();
            emergencyDevices = new List<DeviceSyncSocketServer>();
            taskList = new List<Task>();

            foreach (DeviceInfo info in AllDeviceInfo)
            {
                DeviceSyncSocketServer newDevice = new DeviceSyncSocketServer(info.deviceIP, short.Parse(info.devicePort), info);
                offlineDevices.Add(newDevice);
                taskList.Add(new Task(newDevice.StartListening));
            }

            addLog("->Starting Listeners...");

            foreach (Task t in taskList) 
            {
                t.Start();
            }


            ////Setup timer/////
            ///Refresh the GUI every time interval 
            timer = new Timer
            {
                Interval = (1000)
            };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //Manage the updating of the GUI Table and logs 
            //Loop through the online listeners
            for (int i = 0; i < onlineDevices.Count; i++)
            {
                //Check for any offline devices and move them to the offline list
                if (onlineDevices[i].deviceState == DeviceState.DISCONNECTING)
                {
                    if (onlineDevices[i].serverRunning == false) 
                    {
                        addLog("Listener Error - Server not running: " + onlineDevices[i].deviceInfo.deviceID);
                    }
                    //if the device has requested a disconnect then remove from the active list and log
                    addLog("Device: " + onlineDevices[i].deviceInfo.deviceName + " Offline");
                    onlineDevices[i].deviceState = DeviceState.OFFLINE;
                    offlineDevices.Add(onlineDevices[i]);
                    onlineDevices.RemoveAt(i);         
                }
                else if (DateTime.Now.Subtract(onlineDevices[i].lastPingTime).Seconds > 10) //Check the times and create warning when above 10 seconds 
                {
                    //Add device to the emergency list
                    var overboardDevice = onlineDevices[i];

                    addLog("Device: " + overboardDevice.deviceInfo.deviceID + " Registered to " + overboardDevice.deviceInfo.deviceName + " is not responding");
                    overboardDevice.deviceState = DeviceState.NOTRESPONDING;
                    emergencyDevices.Insert(i, overboardDevice);
                    onlineDevices.RemoveAt(i);

                    //Show message
                    var overboardMessage = MessageBox.Show("Device:  " + overboardDevice.deviceInfo.deviceID + " Registered to " + overboardDevice.deviceInfo.deviceName + " is overboard!",
                                                           "OVERBOARD ALERT!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                   
                    //Wait for OK pushed then add device to offline list
                    if (overboardMessage == DialogResult.OK) 
                    {
                        Console.WriteLine("Moving device to offline");
                        overboardDevice.deviceState = DeviceState.OFFLINE;
                        offlineDevices.Add(overboardDevice);
                        emergencyDevices.RemoveAt(i);
                    }
                }
            }
            //Loop through the offline listeners
            for (int i = 0; i < offlineDevices.Count; i++)
            {
                if (offlineDevices[i].serverRunning == false)
                {
                    addLog("Listener Error - Server not running: " + offlineDevices[i].deviceInfo.deviceID);
                }
                //Check for any offline devices and move them to the offline list
                if (offlineDevices[i].deviceState == DeviceState.CONNECTED)
                {
                    addLog("Device: " + offlineDevices[i].deviceInfo.deviceName + " Online");
                    onlineDevices.Add(offlineDevices[i]);
                    offlineDevices.RemoveAt(i);
                }
            }
            //Update the table 
            DeviceGridBox.Rows.Clear();
            foreach (DeviceSyncSocketServer dev in emergencyDevices)
            {
                DeviceGridBox.Rows.Add(dev.deviceInfo.deviceID, dev.deviceIP.ToString(), dev.portNumber, dev.deviceInfo.deviceName, "EMERGENCY", dev.deviceInfo.deviceBattery, DateTime.Now.Subtract(dev.lastPingTime));
            }
            foreach (DeviceSyncSocketServer dev in onlineDevices) 
            {
                DeviceGridBox.Rows.Add(dev.deviceInfo.deviceID, dev.deviceIP.ToString(), dev.portNumber, dev.deviceInfo.deviceName, dev.deviceState.ToString(), dev.deviceInfo.deviceBattery, DateTime.Now.Subtract(dev.lastPingTime));
            }

        }

        public void addLog(string message) {

            LogBox.Text += message + Environment.NewLine; 
        }
        private List<DeviceInfo> LoadDeviceInfo() {

            List<DeviceInfo> deviceList = new List<DeviceInfo>();
            DeviceInfo currentDevice;
            //1. Create stream reader object
            try
            {
                string filePath = Path.GetFullPath("../../DeviceInfo.csv");
                StreamReader reader = new StreamReader(filePath);
                string line;
                string[] info = new string[4];
                //2. Parse the file 
                while ((line = reader.ReadLine()) != null) {

                    //Split the line 
                    info = line.Split(',');
                    currentDevice = new DeviceInfo();
                    currentDevice.deviceID = info[0];
                    currentDevice.deviceIP = info[1];
                    currentDevice.devicePort = info[2];
                    currentDevice.deviceName = info[3];
                    currentDevice.deviceBattery = "N";
            
                    //add the device to the list
                    deviceList.Add(currentDevice);
                }
                //return the device list 
                return deviceList;
            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception when reading from devices file");
                return null;
            }
        }
    }

    public class DeviceInfo
    {
        public string deviceID, deviceIP, devicePort, deviceName, deviceBattery;
    }
}
