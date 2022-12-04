using System.Net.Sockets;
using System.Threading;

namespace sockets.sockets
{
    class tcpServer
    {
        private TcpListener _server;
        private TcpClient _client;
        private Thread _serverConnection;

        public NetworkStream _ns;
        public bool applicationRunning = true;

        public tcpServer(int serverPort)
        {
            _server = new TcpListener(System.Net.IPAddress.Any, serverPort);
            _server.Start();
            _serverConnection = new Thread(new ThreadStart(serverConnection_DoWork));
            _serverConnection.Start();
        }

        private void serverConnection_DoWork()
        {
            do
            {
                _client = _server.AcceptTcpClient();
                _ns = _client.GetStream();
                Thread.Sleep(1000);
            } while (applicationRunning);
        }

        public byte readByte()
        {
            return (byte)_ns.ReadByte();
        }

        public void sendPackage(byte[] package)
        {
            _ns.Write(package, 0, package.Length);
        }
    }
}
