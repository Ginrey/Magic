using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x85, PacketType.ServerPacket)]
    public class WorldChatS85 : GamePacket
    {
        public byte Channel;
        public byte Emotion;
        public uint RoleId;
        public string Name
        {
            get
            {
                return Encoding.Unicode.GetString(NameBytes);
            }
        }
        public string Message
        {
            get
            {
                return Encoding.Unicode.GetString(MessageBytes);
            }
        }
        public byte[] Data;

        public byte[] NameBytes;
        public byte[] MessageBytes;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Channel = ds.ReadByte();
            Emotion = ds.ReadByte();
            RoleId = ds.ReadUInt32();
            NameBytes = ds.ReadBytes();
            MessageBytes = ds.ReadBytes();
            Data = ds.ReadBytes();

            return base.Deserialize(ds, vc);
        }
    }
}
