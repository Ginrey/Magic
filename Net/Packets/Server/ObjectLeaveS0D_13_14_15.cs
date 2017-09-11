using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x0D, PacketType.ServerContainer)]
    [PacketIdentifier(0x13, PacketType.ServerContainer)]
    [PacketIdentifier(0x14, PacketType.ServerContainer)]
    [PacketIdentifier(0x15, PacketType.ServerContainer)]
    public class ObjectLeaveS0D_13_14_15 : GamePacket<WorldObjectsSet>
    {
        public uint WorldId;

        protected internal override void HandleData(WorldObjectsSet data)
        {
            data.Remove(WorldId);
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            WorldId = ds.ReadUInt32();
            return base.Deserialize(ds, vc);
        }
    }
}
