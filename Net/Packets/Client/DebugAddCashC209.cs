using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x209, PacketType.ClientPacket)]
    public class DebugAddCashC209 : GamePacket
    {
        public DebugAddCashC209()
        {

        }
        public DebugAddCashC209(uint accountId, int cash)
        {

        }

        public uint AccountId;
        public int Cash;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(AccountId).
                Write(Cash);
        }
    }
}
