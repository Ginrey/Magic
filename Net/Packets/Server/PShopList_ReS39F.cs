using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x39F, PacketType.ServerPacket)]
    public class PShopList_ReS39F : GamePacket
    {
        public uint UnkId;
        public PShopEntry[] Entries;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            UnkId = ds.ReadUInt32();
            Entries = ds.ReadArray<PShopEntry>();

            return base.Deserialize(ds, vc);
        }
    }
}
