using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace sockets.sockets
{
    class tcpClient
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private string _ipAddress = "127.0.0.1";
        private int _port = 1905;
        private Thread autoConnection;

        private StreamWriter _output;
        private StreamReader _input;

        public bool applicationRunning = true;

        public tcpClient(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
            autoConnection = new Thread(new ThreadStart(autoConnection_DoWork));
            autoConnection.Start();
        }
        private void autoConnection_DoWork()
        {
            do
            {
                if (!checkConnection())
                {
                    try
                    {
                        _client = new TcpClient(_ipAddress, _port);
                        _stream = _client.GetStream();
                        _input = new StreamReader(_stream);
                        _output = new StreamWriter(_stream);
                    }
                    catch
                    {

                    }
                }
                Thread.Sleep(3000);
            } while (applicationRunning);
        }
        private bool checkConnection()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (TcpConnectionInformation t in connections)
            {
                if (t.RemoteEndPoint.Address.ToString() == _ipAddress && t.RemoteEndPoint.Port == _port)
                {

                    if (t.State == TcpState.Established)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public byte readByte()
        {
            return (byte)_stream.ReadByte();
        }
        public void sendPackage(byte[] package)
        {
            try
            {
                _stream.Write(package, 0, package.Length);
            }
            catch
            {

            }

        }
        public void sendPackage(byte[] package, int lenght)
        {
            try
            {
                _stream.Write(package, 0, lenght);
            }
            catch
            {

            }

        }
        public void sendLine(string value)
        {
            try
            {
                _output.Write(value);
                _output.Flush();
            }
            catch
            {

            }
        }

        public string readLine()
        {

            if (_input != null)
            {
                return _input.ReadLine();
            }
            else
            {
                return null;
            }
        }


        public void stop()
        {
            try
            {
                if (autoConnection != null)
                {
                    autoConnection.Abort();
                }
                _client.Close();
                _client = null;
                _stream = null;
            }
            catch
            {
                try
                {
                    if (autoConnection != null)
                    {
                        autoConnection.Abort();
                    }
                    _client.Close();
                    _client = null;
                    _stream = null;
                }
                catch
                {

                }
            }
        }
    }
}
