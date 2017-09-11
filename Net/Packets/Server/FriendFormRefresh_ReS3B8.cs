using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x3B8, PacketType.ServerPacket)]
    public class FriendFormRefresh_ReS3B8 : GamePacket
    {
        public uint ResultCode;
        public FriendForm FriendForm;
        public uint UnkId;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ResultCode = ds.ReadUInt32();
            FriendForm = ds.Read<FriendForm>();
            UnkId = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}
