using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System;

namespace sockets.sockets
{
    class comPort
    {
        private SerialPort _port;
        private string _comPort = "";
        private int _baudRate = -1;


        public bool isResetRequired = false;
        public string ComPort
        {
            get
            {
                return _comPort;
            }
            set
            {
                _comPort = value;
            }
        }
        public int BaudRate
        {
            get
            {
                return _baudRate;
            }
            set
            {
                _baudRate = value;
            }
        }

        public comPort()
        {

        }
        public comPort(string comPort, int baudRate)
        {
            _comPort = comPort;
            _baudRate = baudRate;
        }
        public bool Open()
        {
            if (string.IsNullOrEmpty(_comPort))
            {
                return false;
            }
            _port = new SerialPort(_comPort, _baudRate);
            try
            {
                _port.Open();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public void Close()
        {
            _port.Close();
        }
        
        public byte[] read(byte[] buffer)
        {
            _port.Read(buffer, 0, buffer.Length);
            return buffer;
        }
        public byte? readByte()
        {
            try
            {
                return (byte)_port.ReadByte();
            }
            catch
            {
                return null;
            }

        }
        public void sendPackage(byte[] package)
        {
            try
            {
                _port.Write(package, 0, package.Length);
                isResetRequired = false;
            }
            catch
            {
                isResetRequired = true;
            }
        }

        public List<USBDeviceInfo> portList()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                devices.Add(new USBDeviceInfo(
                    (string)queryObj["DeviceID"],
                    (string)queryObj["PNPDeviceID"],
                    (string)queryObj["Name"]
                ));
            }
            return devices;

        }
    }
}
