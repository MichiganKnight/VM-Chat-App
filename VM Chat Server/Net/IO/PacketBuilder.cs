using System.Text;

namespace VM_Chat_Server.Net.IO
{
    public class PacketBuilder
    {
        MemoryStream _ms;

        public PacketBuilder()
        {
            _ms = new();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            int msgLength = msg.Length;

            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
