using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace DeviceMonitorGUI
{
    public partial class MainForm : Form
    {
        List<Device> DeviceList;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ////////GUI Init///////////
            //1. Disable all GUI fields
            DeviceGridBox.Enabled = false;
            LogBox.Enabled = false;

            ///////////Device init/////////////
            //1. Load device information from a CSV file 
            List<DeviceInfo> AllDeviceInfo = LoadDeviceInfo();
            //2. Create a device object for each CSV row
            ConstructDevices(AllDeviceInfo);
            //3. Start the socket listener for each device
            foreach (Device device in DeviceList)
            {
                device.StartDevice();
            }

        }

        private void ConstructDevices(List<DeviceInfo> deviceInfoList)
        {
            //1. Init device list
            DeviceList = new List<Device>();

            //2. Construct the device objects and add to the list
            foreach (DeviceInfo info in deviceInfoList) {

                Device newDevice = new Device(DeviceState.OFFLINE, short.Parse(info.DevicePort), info.DeviceID, info.DeviceIP);
                DeviceList.Add(newDevice);      
            }
        }



        struct DeviceInfo {
            public string DeviceID, DeviceIP, DevicePort, DeviceName; 
        }

        private List<DeviceInfo> LoadDeviceInfo() {

            List<DeviceInfo> deviceList = new List<DeviceInfo>();
            DeviceInfo currentDevice;
            //1. Create stream reader object
            try
            {
                StreamReader reader = new StreamReader(@"E:\2021\UNI\Industry Project\ProjectRepo\OverboardDetectionMonitor\OverboardDetectionMonitor\DeviceMonitorGUI\DeviceInfo.csv");
                string line;
                string[] info = new string[4];
                //2. Parse the file 
                while ((line = reader.ReadLine()) != null) {

                    //Split the line 
                    info = line.Split(',');
                    currentDevice = new DeviceInfo();
                    currentDevice.DeviceID = info[0];
                    currentDevice.DeviceIP = info[1];
                    currentDevice.DevicePort = info[2];
                    currentDevice.DeviceName = info[3];

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
}
