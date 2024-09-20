using System.Net.Sockets;

namespace VM_Chat_Client.Net
{
    public class Server
    {
        TcpClient _client;

        public Server()
        {
            _client = new();
        }

        public void ConnectToServer()
        {
            if (!_client.Connected)
            {
                _client.Connect("10.0.0.44", 442);
            }
        }
    }
}
