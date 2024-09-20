using System.Net;
using System.Net.Sockets;
using VM_Chat_Server.Net.IO;

namespace VM_Chat_Server
{
    public class ChatServer
    {
        private static List<Client> _users;
        private static TcpListener _listener;

        public static void Main(string[] args)
        {
            _users = [];

            _listener = new(IPAddress.Parse("10.0.0.44"), 442);
            _listener.Start();

            while (true)
            {
                Client client = new(_listener.AcceptTcpClient());

                _users.Add(client);

                BroadcastConnection();
            }
        }

        private static void BroadcastConnection()
        {
            foreach (Client user in _users)
            {
                foreach (Client usr in _users)
                {
                    PacketBuilder broadcastPacket = new();

                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());

                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }
    }
}