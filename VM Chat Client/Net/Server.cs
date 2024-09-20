using System.Net.Sockets;
using VM_Chat_Client.Net.IO;

namespace VM_Chat_Client.Net
{
    public class Server
    {
        TcpClient _client;

        public PacketReader PacketReader;

        public event Action connectedEvent;

        public Server()
        {
            _client = new();
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("10.0.0.44", 442);

                PacketReader = new(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    PacketBuilder connectPacker = new();
                    connectPacker.WriteOpCode(0);
                    connectPacker.WriteString(username);

                    _client.Client.Send(connectPacker.GetPacketBytes());
                }

                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte opcode = PacketReader.ReadByte();

                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        default:
                            break;
                    }
                }
            });
        }
    }
}
