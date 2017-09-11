using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x05, PacketType.ServerPacket)]
   public class LoginErrorS05: GamePacket
    {
        public byte ErrorCode;
        byte MessageLength;
        public string Message;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ErrorCode = ds.ReadByte();
            MessageLength = ds.ReadByte();
            Message = ds.ReadAsciiString(MessageLength);
            return base.Deserialize(ds, vc);
        }
    }
}
