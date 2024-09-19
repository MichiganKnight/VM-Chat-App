using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    public class ChatServer
    {
        private static List<TcpClient> clients = [];
        private const int port = 442;

        private static void Main()
        {
            TcpListener server = new(IPAddress.Any, port);

            server.Start();

            Console.WriteLine($"Server started on port: {port}.");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                clients.Add(client);

                Console.WriteLine("Client Connected");

                Thread clientThread = new(HandleClient);
                clientThread.Start();
            }
        }

        private static void HandleClient(TcpClient tcpClient)
        {
            TcpClient client = tcpClient;

            NetworkStream stream = client.GetStream();

            StreamReader reader = new(stream, Encoding.UTF8);

            try
            {
                string message = reader.ReadLine();

                if (message != null)
                {
                    Console.WriteLine($"Received: {message}");

                    BroadcastMessage(message, client);
                }
            }
            catch (Exception)
            {
                clients.Remove(client);

                Console.WriteLine("Client Disconnected");

                client.Close();
            }
        }

        private static void BroadcastMessage(string message, TcpClient sender)
        {
            foreach (TcpClient client in clients)
            {
                if (client != sender)
                {
                    try
                    {
                        StreamWriter writer = new(client.GetStream(), Encoding.UTF8);

                        writer.WriteLine(message);
                        writer.Flush();
                    }
                    catch
                    {
                        client.Close();
                        clients.Remove(client);
                    }
                }
            }
        }
    }
}