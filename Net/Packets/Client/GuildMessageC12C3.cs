using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x12C3, PacketType.ClientPacket)]
    public class GuildMessageC12C3 : GamePacket<AccountInfo>
    {
        private static byte[] emptyData = { };

        public GuildMessageC12C3() : this(0, string.Empty)
        {
        }
        public GuildMessageC12C3(string message) : this(0, message)
        {
        }
        public GuildMessageC12C3(byte type, string message)
        {
            Type = type;
            Message = message;
            ItemData = emptyData;
        }

        public byte Type;
        public byte Emotion;
        public uint RoleId;
        public string Message;
        public byte[] ItemData;
        public uint Unk = 0;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(Type).
                Write(Emotion).
                Write(RoleId).
                WriteUnicodeString(Message).
                Write(ItemData, true).
                Write(Unk);
        }
    }
}
