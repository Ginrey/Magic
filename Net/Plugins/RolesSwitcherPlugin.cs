using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Net.Packets;
using Magic.Net.Packets.Server;
using Magic.Net.Packets.Client;

namespace Magic.Net.Plugins
{
    public class RolesSwitcherPlugin : Plugin
    {
        public AccountRolesPlugin AccountRoles { get; private set; }
        public WorldEnteringPlugin WorldEntering { get; private set; }
        public AccountInfo Account { get; private set; }

        protected internal override void Initialize()
        {
            AccountRoles = Host.Plugins.Register<AccountRolesPlugin>();
            WorldEntering = Host.Plugins.Register<WorldEnteringPlugin>();
            Account = Host.Data.Register<AccountInfo>();

            Host.Handler.AddHandler<AnnounceForbidInfoS7B>(OnForbidReceive);
        }

        private void OnForbidReceive(object sender, PacketEventArgs e)
        {
            var forbid = (AnnounceForbidInfoS7B)e.Packet;
            Console.WriteLine("MAGIC");

            if (Enabled)
            {
                Console.WriteLine("MAGIC1");
                if (Account.Roles.Count == 0)
                {
                    WorldEntering.SelectedIndex = 0;
                    return;
                }
                WorldEntering.SelectedIndex = (WorldEntering.SelectedIndex + 1) % Account.Roles.Count;
                if (Account.EnteredWorld)
                {
                    Host.Close();
                }
                else
                {
                    Host.Close();
                }
            }
        }
    }
}
