using Magic.IO;

namespace Magic.Net.Packets.Client
{
    [PacketIdentifier(0x02, PacketType.ClientPacket, 1410)]
    public class LogginAnnounceC02 : GamePacket
    {
        public LogginAnnounceC02()
        {

        }
        public LogginAnnounceC02(string login, string password)
        {
            Security.MD5Hash md5 = new Security.MD5Hash();
            Login = login.ToLower();
            Password = password;
            Hash = md5.GetHash(login, password, new byte[16]);
        }
        public LogginAnnounceC02(string login, byte[] hash)
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
