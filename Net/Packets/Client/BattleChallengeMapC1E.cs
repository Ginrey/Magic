using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x1E, PacketType.ClientContainerC25)]
    public class BattleChallengeMapC1E : GamePacket<AccountInfo>
    {
        public uint Unknown1 = 1443037184;
        public uint RoleId;
        public uint GuildId;
        public uint Unknown2 = 0xFFFFFFFF;

        public BattleChallengeMapC1E()
        {
        }
        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            GuildId = data.SelectedRole.GuildId;

            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(Unknown1).
                Write(RoleId).
                Write(GuildId).
                Write(Unknown2);
        }
    }
}
