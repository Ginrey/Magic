using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0xFA1, PacketType.ClientPacket)]
    public class TradeStartCFA1 : GamePacket<AccountInfo>
    {
        public TradeStartCFA1()
        {

        }
        public TradeStartCFA1(uint partnerId)
        {
            PartnerId = partnerId;
        }

        public uint RoleId;
        public uint Localsid;
        public uint PartnerId;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(Localsid).
                Write(PartnerId);
        }
    }
}
