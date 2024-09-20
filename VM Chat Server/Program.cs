using System.Net;
using System.Net.Sockets;

namespace VM_Chat_Server
{
    public class ChatServer
    {
        private static TcpListener _listener;

        public static void Main(string[] args)
        {
            _listener = new(IPAddress.Parse("10.0.0.44"), 442);
            _listener.Start();

            TcpClient client = _listener.AcceptTcpClient();

            Console.WriteLine("Client Has Connected");
        }
    }
}