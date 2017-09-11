using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x12D1, PacketType.ServerPacket)]
    public class GetPlayerGuildInfo_ReS12D1 : GamePacket<AccountInfo>
    {
        public uint RoleId;
        public uint UnkId;
        public string Name;
        public uint GuildId;

        protected internal override void HandleData(AccountInfo data)
        {
            data.SelectedRole.GuildId = GuildId;
            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();
            ds.ReadUInt32();
            Name = ds.ReadUnicodeString();
            GuildId = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}
