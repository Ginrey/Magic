using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x46, PacketType.ServerContainer)]
    public class NpcGreetingS46 : GamePacket
    {
        public uint WorldId;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            WorldId = ds.ReadUInt32();
            return base.Deserialize(ds, vc);
        }
    }
}
