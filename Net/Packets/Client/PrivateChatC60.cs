using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x60, PacketType.ClientPacket)]
    public class PrivateChatC60 : GamePacket<AccountInfo>
    {
        static byte[] emptyBytes = { };

        public PrivateChatC60(string dstName, string message) : this(0x00, dstName, message)
        {

        }
        public PrivateChatC60(byte channel, string dstName, string message)
        {
            Channel = channel;
            DstName = dstName;
            Message = message;

            Data = emptyBytes;
        }

        public byte Channel;
        public byte Emotion;
        public string SrcName;
        public uint SrcRoleId;
        public string DstName;
        public uint DstRoleId;
        public string Message;
        public byte[] Data;
        public int SrcLevel;

        protected internal override void HandleData(AccountInfo data)
        {
            SrcName = data.SelectedRole.Name;
            SrcRoleId = data.SelectedRole.RoleId;
            SrcLevel = data.SelectedRole.Level;

            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(Channel).
                Write(Emotion).
                WriteUnicodeString(SrcName).
                Write(SrcRoleId).
                WriteUnicodeString(DstName).
                Write(DstRoleId).
                WriteUnicodeString(Message).
                Write(Data, true).
                Write(SrcLevel);
        }
    }
}
