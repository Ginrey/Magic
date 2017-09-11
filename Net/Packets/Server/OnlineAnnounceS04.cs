using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x04, PacketType.ServerPacket)]
    public class OnlineAnnounceS04 : GamePacket<AccountInfo>
    {
        public uint AccountId;
        public uint UnkId;

        protected internal override void HandleData(AccountInfo data)
        {
            data.AccountId = AccountId;
            data.UnkId = UnkId;
            data.SetRolesPage();
        }

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            AccountId = ds.ReadUInt32();
            UnkId = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}
