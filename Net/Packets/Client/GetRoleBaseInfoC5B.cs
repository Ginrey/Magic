using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x5B, PacketType.ClientPacket)]
    public class GetRoleBaseInfoC5B : GamePacket<AccountInfo>
    {
        static uint[] emptyArray = { };

        public uint RoleId;
        public uint Unk;
        public uint[] OtherRoles;

        public GetRoleBaseInfoC5B()
        {
            OtherRoles = emptyArray;
        }
        public GetRoleBaseInfoC5B(params uint[] otherRoles)
        {
            OtherRoles = otherRoles;
        }


        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;

            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            ds.Write(RoleId);
            ds.Write(Unk);
            ds.WriteCompactUInt32(OtherRoles.Length);
            foreach (var role in OtherRoles)
            {
                ds.Write(role);
            }

            return base.Serialize(ds, vc);
        }
    }
}
