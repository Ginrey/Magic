using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x34, PacketType.ServerContainer)]
    public class SelectTargetReS34 : GamePacket
    {
        public uint TargetId;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            TargetId = ds.ReadUInt32();
            return base.Deserialize(ds, vc);
        }
    }
}
