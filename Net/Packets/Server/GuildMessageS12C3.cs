using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x12C3, PacketType.ServerPacket)]
    public class GuildMessageS12C3 : GamePacket
    {
        public byte Type;
        public byte Emotion;
        public uint RoleId;
        public string Message;
        public byte[] ItemData;
        public uint Unk = 0;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Type = ds.ReadByte();
            Emotion = ds.ReadByte();
            RoleId = ds.ReadUInt32();
            Message = ds.ReadUnicodeString();
            ItemData = ds.ReadBytes();
            Unk = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}
