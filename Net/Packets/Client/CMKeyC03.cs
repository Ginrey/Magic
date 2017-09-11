using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x03, PacketType.ClientPacket)]
    public class CMKeyC03 : GamePacket<ConnectionInfo>
    {
        public byte[] CMKey;
        public bool Force;

        public CMKeyC03()
        {
        }
        public CMKeyC03(bool force)
        {
            Force = force;
        }

        protected internal override void HandleData(ConnectionInfo data)
        {
            CMKey = data.CMKey;
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(CMKey, true).
                Write(Force);
        }
    }
}
