using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
  //  [PacketIdentifier(0x0B, PacketType.ServerContainer)]
  //  [PacketIdentifier(0x10, PacketType.ServerContainer)]
    public class NpcEnterS0B_10 : GamePacket<WorldObjectsSet>
    {
        public NpcWorldInfo Npc;

        protected internal override void HandleData(WorldObjectsSet data)
        {
            data.Add(Npc);
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Npc = ds.Read<NpcWorldInfo>();
            return base.Deserialize(ds, vc);
        }
    }
}
