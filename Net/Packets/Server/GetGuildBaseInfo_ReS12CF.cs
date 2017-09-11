using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x12CF, PacketType.ServerPacket)]
    public class GetGuildBaseInfo_ReS12CF : GamePacket
    {
        public uint RoleId;
        public uint UnkId;
        public GGuildBase Guild;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();

            Guild = ds.Read<GGuildBase>();

            return base.Deserialize(ds, vc);
        }
    }
}
