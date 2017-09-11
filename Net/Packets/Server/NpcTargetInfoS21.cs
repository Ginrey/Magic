using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x21, PacketType.ServerContainer)]
    public class NpcTargetInfoS21 : GamePacket
    {
        public uint WorldId;

        public HpMp Hp;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            WorldId = ds.ReadUInt32();

            Hp = ds.Read<HpMp>();
            return base.Deserialize(ds, vc);
        }
    }
}
