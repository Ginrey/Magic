using System;
using System.Collections.Generic;
using System.Linq;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.Data.Types;
using System.Text;

namespace Magic.Net.Plugins
{
    public delegate void RoleBaseInfoEventHandler(object sender, RoleBaseInfoS5C role);
    public class RolesInfoRequesterPlugin : Plugin
    {
        public event RoleBaseInfoEventHandler RoleBaseReceive = (a, b) => { };

        public void BeginGetRole(string name)
        {
            Host.Send(new GetRoleIdByNameC76(name));
        }
        public void BeginGetRole(uint roleId)
        {
            Host.Send(new GetRoleBaseInfoC5B(roleId));
        }

        protected internal override void Initialize()
        {
            Host.Handler.AddHandler<RoleBaseInfoS5C>(OnRoleBaseReceive);
            Host.Handler.AddHandler<GetRoleIdByName_ReS77>(OnRoleIdReceive);

            base.Initialize();
        }

        private void OnRoleBaseReceive(object sender, PacketEventArgs e)
        {
            var role = (RoleBaseInfoS5C)e.Packet;
            RoleBaseReceive(this, role);
        }
        private void OnRoleIdReceive(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var role = (GetRoleIdByName_ReS77)e.Packet;

                if (role.ResultCode == 0)
                {
                    BeginGetRole(role.RoleId);
                }
            }
        }
    }
}
