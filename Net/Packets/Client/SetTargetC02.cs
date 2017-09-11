using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x02, PacketType.ClientContainer)]
    public class SetTargetC02 : GamePacket
    {
        public SetTargetC02()
        {

        }
        public SetTargetC02(uint id)
        {
            Id = id;
        }
        public uint Id;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(Id);
        }
    }
}
