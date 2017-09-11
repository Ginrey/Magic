using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x78, PacketType.ClientPacket)]
    public class ChatBroadCastC78 : GamePacket<AccountInfo>
    {
        static Tuple<uint, uint>[] emptyRoles = { };
        static byte[] emptyBytes = { };

        public ChatBroadCastC78()
        {
            Roles = emptyRoles;
            Data = emptyBytes;
            Message = string.Empty;
        }
        public ChatBroadCastC78(Tuple<uint, uint>[] roles, string message)
        {
            Roles = roles;
            Message = message;
            Data = emptyBytes;
        }

        public Tuple<uint, uint>[] Roles;
        public byte Channel;
        public byte Emotion;
        public uint RoleId;
        public string Message;
        public byte[] Data;
        public uint SrcLevel;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
            SrcLevel = (uint)data.SelectedRole.Level;
            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            /*
            ds.WriteCompactUInt32(Roles.Length);
            foreach (var role in Roles)
            {
                ds.Write(role.Item1);
                ds.Write(role.Item2);
            }*/
            ds.Write(Channel);
            ds.Write(Emotion);
            ds.Write(RoleId);
            ds.WriteUnicodeString(Message);
            ds.Write(Data, true);
            //ds.Write(SrcLevel);

            return base.Serialize(ds, vc);
        }
    }
}
