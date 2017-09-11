using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0xFA2, PacketType.ServerPacket)]
    public class TradeStart_ReSFA2 : GamePacket
    {
        public uint ResultCode;
        public uint TID; // ???
        public uint PartnerId;
        public uint RoleId;
        public uint Localsid;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadUInt32();
            TID = ds.ReadUInt32();
            PartnerId = ds.ReadUInt32();
            RoleId = ds.ReadUInt32();
            Localsid = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}
