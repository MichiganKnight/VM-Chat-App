using System.Net.Sockets;
using System.Text;

namespace VM_Chat_Server.Net.IO
{
    public class PacketReader : BinaryReader
    {
        private NetworkStream _ns;

        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            byte[] msgBuffer;

            int length = ReadInt32();

            msgBuffer = new byte[length];

            _ns.Read(msgBuffer, 0, length);

            string msg = Encoding.ASCII.GetString(msgBuffer);

            return msg;
        }
    }
}
