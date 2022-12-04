using System.Net;
using System.Net.Sockets;

namespace sockets.sockets
{
    class udp
    {
        private UdpClient udpClient;
        private IPAddress targetIPAddress;
        IPEndPoint sendPoint; 
        IPEndPoint receivePoint; 

        public udp(string ipAddress,int sender,int receiver)
        {
            udpClient = new UdpClient(receiver);
            targetIPAddress = IPAddress.Parse(ipAddress);
            sendPoint = new IPEndPoint(targetIPAddress, sender);
            receivePoint = new IPEndPoint(targetIPAddress, receiver);
        }
        public void sendPackage(byte[] package)
        {
            udpClient.Send(package, package.Length, sendPoint);
        }

        public byte[] readPackage()
        {
            return udpClient.Receive(ref receivePoint);
        }
    }
}
