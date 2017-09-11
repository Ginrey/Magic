using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x39C, PacketType.ClientPacket)]
    public class PShopPlayerGetC39C : GamePacket<AccountInfo>
    {
        public PShopPlayerGetC39C()
        {

        }
        public PShopPlayerGetC39C(uint otherRoleId)
        {
            OtherRoleId = otherRoleId;
        }

        public uint RoleId;
        public uint OtherRoleId;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(OtherRoleId);
        }
    }
}
