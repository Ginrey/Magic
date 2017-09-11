using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x356, PacketType.ClientPacket)]
    public class BattleChallengeMapC356 : GamePacket<AccountInfo>
    {
        public uint RoleId;
        public uint GuildId;
        public uint UnkId;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            GuildId = data.SelectedRole.GuildId;
            UnkId = data.UnkId;

            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(GuildId).
                Write(UnkId);
        }
    }
}
