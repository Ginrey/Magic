using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x39D, PacketType.ServerPacket)]
    public class PShopPlayerGet_ReS39D : GamePacket
    {
        public uint ResultCode;
        public uint UnkId;

        public PShopBase Shop;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();
            Shop = ds.Read<PShopBase>();

            return base.Deserialize(ds, vc);
        }
    }
}
