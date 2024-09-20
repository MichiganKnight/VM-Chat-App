using System.Net.Sockets;
using VM_Chat_Server.Net.IO;

namespace VM_Chat_Server
{
    public class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new(ClientSocket.GetStream());

            byte opcode = _packetReader.ReadByte();

            Username = _packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Username}");

            Task.Run(Process);
        }

        private void Process()
        {
            while ( true )
            {
                try
                {
                    byte opcode = _packetReader.ReadByte();

                    switch (opcode)
                    {
                        case 5:
                            string msg = _packetReader.ReadMessage();

                            Console.WriteLine($"[{DateTime.Now}]: Message Received: {msg}");

                            ChatServer.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        default:
                            break;

                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{UID}]: Disconnected");

                    ChatServer.BroadcastDisconnect(UID.ToString());

                    ClientSocket.Close();

                    break;
                }
            }
        }
    }
}
