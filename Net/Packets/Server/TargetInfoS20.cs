using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x20, PacketType.ServerContainer)]
    public class TargetInfoS20 : GamePacket
    {
        public uint RoleId;

        public int Level;

        public byte Unk1;
        public byte Unk2;

        public HpMp Hp;
        public HpMp Mp;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();

            Level = ds.ReadInt16();

            Unk1 = ds.ReadByte();
            Unk2 = ds.ReadByte();

            Hp = ds.Read<HpMp>();
            Mp = ds.Read<HpMp>();

            return base.Deserialize(ds, vc);
        }
    }
}
