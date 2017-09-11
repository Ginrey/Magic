using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
   // [PacketIdentifier(0x0C, PacketType.ServerContainer)] // enter slice
  //  [PacketIdentifier(0x11, PacketType.ServerContainer)] // enter world
    public class RoleEnterS0C_11 : GamePacket<WorldObjectsSet>
    {
        public RoleWorldInfo Role;

        protected internal override void HandleData(WorldObjectsSet data)
        {
            data.Add(Role);
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Role = ds.Read<RoleWorldInfo>(vc);
            return base.Deserialize(ds, vc);
        }
    }
}
