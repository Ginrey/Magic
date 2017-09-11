using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x76, PacketType.ClientPacket)]
    public class GetRoleIdByNameC76 : GamePacket
    {
        public GetRoleIdByNameC76(string name)
        {
            Name = name;
        }
        public string Name;
        public uint Unk;
        public byte Reason;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                WriteUnicodeString(Name).
                Write(Unk).
                Write(Reason);
        }
    }
}
