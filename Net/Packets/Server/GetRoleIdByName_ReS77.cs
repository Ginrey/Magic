using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x77, PacketType.ServerPacket)]
    public class GetRoleIdByName_ReS77 : GamePacket
    {
        public int ResultCode;
        public uint UnkId;
        public string RoleName;
        public uint RoleId;
        public byte Reason;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadInt32();
            UnkId = ds.ReadUInt32();
            RoleName = ds.ReadUnicodeString();
            RoleId = ds.ReadUInt32();
            Reason = ds.ReadByte();

            return base.Deserialize(ds, vc);
        }
    }
}
