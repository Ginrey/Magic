using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x23, PacketType.ClientContainer)]
    public class OpenNpcDialogC23  :GamePacket
    {
        public OpenNpcDialogC23()
        {

        }
        public OpenNpcDialogC23(uint worldId)
        {
            WorldId = worldId;
        }

        public uint WorldId;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(WorldId);
        }
    }
}
