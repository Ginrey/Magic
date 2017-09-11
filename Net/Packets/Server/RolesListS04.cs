using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
   // [PacketIdentifier(0x04, PacketType.ServerContainer)]
    public class RolesListS04 : GamePacket<WorldObjectsSet>
    {
        public RoleWorldInfo[] PlayersList;

        protected internal override void HandleData(WorldObjectsSet data)
        {
            foreach(var player in PlayersList)
            {
                data.Add(player);
            }
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            PlayersList = ds.ReadArray<RoleWorldInfo>(ds.ReadInt16());

            return base.Deserialize(ds, vc);
        }
    }
}
