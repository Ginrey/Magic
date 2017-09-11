using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x39E, PacketType.ClientPacket)]
    public class PShopListC39E : GamePacket<AccountInfo>
    {
        public PShopListC39E() : this(0xFF)
        {

        }
        public PShopListC39E(uint shopType)
        {
            ShopType = shopType;
        }

        public uint RoleId;
        public uint UnkId;
        public uint ShopType;

        protected internal override void HandleData(AccountInfo data)
        {
            RoleId = data.SelectedRole.RoleId;

            base.HandleData(data);
        }

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            return ds.
                Write(RoleId).
                Write(UnkId).
                Write(ShopType);
        }
    }
}
