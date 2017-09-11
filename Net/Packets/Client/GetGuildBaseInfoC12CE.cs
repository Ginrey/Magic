using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x12CE, PacketType.ClientPacket)]
    public class GetGuildBaseInfoC12CE : GamePacket<AccountInfo>
    {
        private static uint[] emptyGuilds = { };

        public GetGuildBaseInfoC12CE() : this(emptyGuilds)
        {

        }
        public GetGuildBaseInfoC12CE(params uint[] guildsIds)
        {
            GuildsIds = guildsIds;
        }

        public uint RoleId;
        public uint UnkId;
        public uint[] GuildsIds;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;

            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            ds.Write(RoleId).Write(UnkId);
            ds.WriteCompactUInt32(GuildsIds.Length);
            foreach(var guildId in GuildsIds)
            {
                ds.Write(guildId);
            }

            return base.Serialize(ds, vc);
        }
    }
}
