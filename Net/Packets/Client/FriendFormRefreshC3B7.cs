using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x3B7, PacketType.ClientPacket)]
    public class FriendFormRefreshC3B7 : GamePacket<AccountInfo>
    {
        public uint RoleId;
        public uint Code;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(RoleId).Write(Code);
        }
    }
}
