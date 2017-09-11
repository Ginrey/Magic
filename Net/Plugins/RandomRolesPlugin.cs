using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.Data;
using Magic.Net.Packets;
using Magic.Net.Packets.Server;

namespace Magic.Net.Plugins
{
    public class RandomRolesPlugin : Plugin
    {
        private Random random = new Random();

        public AccountRolesPlugin AccountRoles { get; private set; }

        private object lockObj = new object();

        public string Alphabet { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }

        public int QueueSize { get; private set; }

        private string GetRandomString(string alphabet, int minLen, int maxLen)
        {
            var length = random.Next(minLen, maxLen + 1);
            var res = new char[length];

            for(var i = 0; i < length; i++)
            {
                res[i] = alphabet[random.Next(alphabet.Length)];
            }
            return new string(res);
        }

        public void CreateRandomRole()
        {
            if (Host.ConnectionInfo.Status != ConnectionStatus.Connected) return;

            lock (lockObj)
            {
                var name = GetRandomString(Alphabet, MinimumLength, MaximumLength);

                QueueSize++;
                AccountRoles.CreateRole(name, Gender.Male, Occupation.Warrior);
            }
        }
        
        protected internal override void Initialize()
        {
            var alphabet = new char['z' - 'a' + 'Z' - 'A' + '9' - '0' + 3];
            var p = 0;
            for (var i = 'a'; i <= 'z'; i++)
            {
                alphabet[p++] = (char)i;
                alphabet[p++] = char.ToUpper((char)i);
            }
            for(var i = '0'; i <= '9'; i++)
            {
                alphabet[p++] = (char)i;
            }

            MinimumLength = 1;
            MaximumLength = 9;
            Alphabet = new string(alphabet);

            AccountRoles = Host.Plugins.Register<AccountRolesPlugin>();
            Host.Handler.AddHandler<CreateRole_ReS55>(OnCreateRole);
            Host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;

            base.Initialize();
        }

        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            if (Host.ConnectionInfo.Status == ConnectionStatus.Disconnected)
            {
                lock (lockObj)
                {
                    QueueSize = 0;
                }
            }
        }

        private void OnCreateRole(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var role = (CreateRole_ReS55)e.Packet;

                lock (lockObj)
                {
                    if (role.ResultCode.ResultCode != 0)
                    {
                        if (QueueSize != 0)
                        {
                            QueueSize--;
                            CreateRandomRole();
                        }
                    }
                    else
                    {
                        if (QueueSize != 0)
                        {
                            QueueSize--;
                        }
                    }
                }
            }
        }
    }
}
