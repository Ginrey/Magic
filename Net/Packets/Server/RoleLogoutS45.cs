using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x45, PacketType.ServerPacket)]
    public class RoleLogoutS45 : GamePacket<AccountInfo>
    {
        public uint RoleId;
        public uint ConnectionId;

        protected internal override void HandleData(AccountInfo data)
        {
            data.SetRolesPage();
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ds.Skip(4);
            RoleId = ds.ReadUInt32();
            ds.Skip(4);
            ConnectionId = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }

    }
}
