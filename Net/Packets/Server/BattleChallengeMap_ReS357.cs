using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x357, PacketType.ServerPacket)]
    public class BattleChallengeMap_ReS357 : GamePacket
    {
        public uint RoleId;
        public ushort ResultCode;
        public uint Status;
        public uint MaxBonus;
        public GBattleChallenge[] Cities;
        public int RandomNumber;
        public uint UnkId;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            ResultCode = ds.ReadUInt16();
            Status = ds.ReadUInt32();
            MaxBonus = ds.ReadUInt32();
            Cities = ds.ReadArray<GBattleChallenge>();
            RandomNumber = ds.ReadInt32();
            UnkId = ds.ReadUInt32();

            return base.Deserialize(ds, vc);
        }
    }
}
