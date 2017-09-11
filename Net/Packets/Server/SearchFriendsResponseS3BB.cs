using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    public class FriendCandidateInfo : DataSerializer
    {
        public uint RoleId;
        public int RoleLevel;
        public Occupation Occupation;
        public Gender Gender;
        public float Prc;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            RoleLevel = ds.ReadInt32();
            Occupation = ds.Skip(3).Read<Occupation>();
            Gender = ds.Read<Gender>();
            Prc = ds.ReadSingle();

            return ds;
        }
    }
    [PacketIdentifier(0x3BB, PacketType.ServerPacket)]
    public class SearchFriendsResponseS3BB : GamePacket
    {
        public uint RoleId;
        public FriendCandidateInfo[] Candidates;

        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            RoleId = ds.ReadUInt32();
            Candidates = ds.ReadArray<FriendCandidateInfo>();

            return ds;
        }
    }
}
