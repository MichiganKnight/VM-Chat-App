using System.Net.Sockets;
using VM_Chat_Client.Net.IO;

namespace VM_Chat_Client.Net
{
    public class Server
    {
        TcpClient _client;

        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectedEvent;

        public Server()
        {
            _client = new();
        }

        public void ConnectToServer(string username)
        {
            if (!_client.Connected)
            {
                _client.Connect("10.0.0.10", 442);

                PacketReader = new(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    PacketBuilder connectPacker = new();
                    connectPacker.WriteOpCode(0);
                    connectPacker.WriteMessage(username);

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
                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectedEvent?.Invoke();
                            break;
                        default:
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            PacketBuilder messagePacket = new();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);

            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
