using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
   // [PacketIdentifier(0x09, PacketType.ServerContainer)]
    public class NpcListS09 : GamePacket<WorldObjectsSet>
    {
        public NpcWorldInfo[] NpcList;

        protected internal override void HandleData(WorldObjectsSet data)
        {
            foreach(var npc in NpcList)
            {
                data.Add(npc);
            }
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            NpcList = ds.ReadArray<NpcWorldInfo>(ds.ReadInt16());
            return base.Deserialize(ds, vc);
        }
    }
}
