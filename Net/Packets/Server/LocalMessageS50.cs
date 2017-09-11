using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x50, PacketType.ServerPacket)]
    public class LocalMessageS50 : GamePacket
    {
        public byte ChatType;
        public byte Unk1;
        public uint UID;
        public byte[] MessageBytes;
        public string Message => Encoding.Unicode.GetString(MessageBytes);

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ChatType = ds.ReadByte();
            Unk1 = ds.ReadByte();
            UID = ds.ReadUInt32();
            MessageBytes = ds.ReadBytes();
            return base.Deserialize(ds, vc);
        }
    }
}
