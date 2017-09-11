using System;
using System.Net;
using System.Net.Sockets;

namespace Magic.Net
{
    public class GameServer
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public GameServer() : this("127.0.0.1", 29000)
        {
        }
        public GameServer(string host, int port) : this(host, port, string.Empty)
        {
        }
        public GameServer(string host, int port, string name)
        {
            Name = name;
            Host = host;
            Port = port;
        }
        public GameServer(string server)
        {
            string[] args = server.Replace(" ", "").Split(':', ';', '=', '\t');
            if (args.Length == 0)
            {
                Host = "127.0.0.1";
                Port = 29000;
                return;
            }
            if (args.Length == 1)
            {
                Host = args[0];
                Port = 29000;
                return;
            }

            int port;

            if (int.TryParse(args[0], out port))
            {
                Port = port;
                Host = args[1];
            }
            else
            {
                if (int.TryParse(args[1], out port))
                {
                    Port = port;
                    Host = args[0];
                }
                else
                {
                    Host = args[0];
                    Port = 29000;
                }
            }
        }

        public override string ToString()
        {
            string srvName = string.IsNullOrEmpty(Name) ? "" : Name + " ";
            return String.Format("{0}{1}:{2}", srvName, Host, Port);
        }
        public string ToShortString()
        {
            return string.IsNullOrEmpty(Name) ? String.Format("{0}:{1}", Host, Port) : Name;
        }
    }
}
