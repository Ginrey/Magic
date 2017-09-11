using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x369, PacketType.ClientPacket)]
    public class DebugCommandC369 : GamePacket<AccountInfo>
    {
        public DebugCommandC369()
        {

        }
        public DebugCommandC369(string command)
        {
            Command = command;
        }

        public uint RoleId;
        public int Tag;
        public string Command;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;

            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(Tag).
                WriteUnicodeString(Command);
        }
    }
}
