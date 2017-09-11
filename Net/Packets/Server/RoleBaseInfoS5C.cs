using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x5C, PacketType.ServerPacket)]
    public class RoleBaseInfoS5C : GamePacket
    {
        public uint ResultCode;
        public uint MyRoleId;
        public uint UnkId;
        public GRoleBase RoleBase;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadUInt32();
            MyRoleId = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();
            RoleBase = ds.Read<GRoleBase>(vc);

            return base.Deserialize(ds, vc);
        }
    }
}
