using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x03, PacketType.ClientPacket, 1410)]
    public class LogginAnnounceC03 : GamePacket
    {
       public static byte[] unkBytes = { 0xFF, 0xFF, 0xFF, 0xFF };

        public LogginAnnounceC03()
        {

        }
        public LogginAnnounceC03(string login, string password)
        {
            Security.MD5Hash md5 = new Security.MD5Hash();
            Login = login.ToLower(); 
            Password = password;
            Hash = md5.GetHash(login,password,new byte[16]);
        }
        public LogginAnnounceC03(string login, byte[] hash)
        {
            Login = login;
            Hash = hash;
        }

        public string Login;
        public string Password;
        public byte[] Hash;

        public override DataStream Serialize(DataStream ds, VersionControl vc)
        {
            ds.WriteAsciiString(Login);
            ds.Write(Hash, true);
            ds.Write((byte)0);
            ds.Write(unkBytes, true);

            return base.Serialize(ds, vc);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            Login = ds.ReadAsciiString();
            Hash = ds.ReadBytes();
            ds.ReadByte();
            ds.ReadBytes();

            return base.Deserialize(ds, vc);
        }
    }
}
