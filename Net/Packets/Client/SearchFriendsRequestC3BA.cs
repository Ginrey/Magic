using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x3BA, PacketType.ClientPacket)]
    public class SearchFriendsRequestC3BA : GamePacket<AccountInfo>
    {
        public SearchFriendsRequestC3BA() : this(0)
        {
        }
        public SearchFriendsRequestC3BA(uint reqType)
        {
            ReqType = reqType;
        }

        public uint RoleId;
        /// <summary>
        /// Наставник, друг, супруг
        /// 0-2
        /// </summary>
        public uint ReqType;
        public uint Unk;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;

            base.HandleData(data);
        }
        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(ReqType).
                Write(Unk);
        }
    }
}
