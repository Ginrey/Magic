using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x1199, PacketType.ServerPacket)]
    public class JoinTheGuildS1199 : GamePacket
    {
        public uint TypeReq;
        public uint MyUID;
        public uint Unk;
        public uint SendUID;
        public uint InviteUID;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            TypeReq = ds.ReadUInt32();
            MyUID = ds.ReadUInt32();
            Unk = ds.ReadUInt32();
            SendUID = ds.ReadUInt32();
            InviteUID = ds.ReadUInt32();
            return base.Deserialize(ds, vc);
        }
    }
}
