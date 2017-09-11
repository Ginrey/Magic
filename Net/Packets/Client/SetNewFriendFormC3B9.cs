using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x3B9, PacketType.ClientPacket)]
    public class SetNewFriendFormC3B9 : GamePacket<AccountInfo>
    {
        static FriendForm defaultFriendForm = new FriendForm();
        public SetNewFriendFormC3B9() : this(defaultFriendForm)
        {

        }
        public SetNewFriendFormC3B9(FriendForm friendForm)
        {
            FriendForm = friendForm;
        }

        public uint RoleId;
        public FriendForm FriendForm;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.Write(RoleId).Write(FriendForm);
        }
    }
}
