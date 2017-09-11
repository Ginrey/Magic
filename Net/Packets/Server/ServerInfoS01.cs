using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x01, PacketType.ServerPacket)]
    public class ServerInfoS01 : GamePacket<ConnectionInfo>
    {
        public byte[] Key;
        public ServerVersion ServerVersion;
        public byte AuthType;

        public string CRC;
        public byte Bonus;

        protected internal override void HandleData(ConnectionInfo data)
        {
            float serverStatus = (float)(Key[0] * 100) / 255;

            data.ServerStatus = serverStatus;
            data.S01Key = Key;

            data.ServerVersion = ServerVersion;
            data.CRC = CRC;
            data.Bonus = Bonus;

            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Key = ds.ReadBytes();
            ServerVersion = ds.Read<ServerVersion>(vc);
            AuthType = ds.ReadByte();
            return ds;
        }
    }
}
