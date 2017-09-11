using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.Net.Plugins;

namespace Magic
{
    class Program
    {
        const string testMail = "qwertyy1";
        const string testPass = "qwerty";

        static OOGHost Host;
        static AuthPlugin Auth;
        static AccountRolesPlugin AccountRoles;
        static ReloginPlugin Relogin;
        static EmuPlugin Emu;

        static void Main(string[] args)
        {
            string ss = "ARR".TrimEnd('R');
            Host = new OOGHost("149.202.223.181:15129");
           
          //  Auth = Host.Plugins.Register<AuthPlugin>();
            var Auth = Host.Plugins.Register<AuthPlugin>();
            //  Relogin = Host.Plugins.Register<ReloginPlugin>();
            AccountRoles = Host.Plugins.Register<AccountRolesPlugin>();
            Emu = Host.Plugins.Register<EmuPlugin>();
         //   Auth.OldVersion = AccountRoles.OldVersion = true;
            Auth.Account.RolesListChanged += Account_RolesLoadedChanged;
            Auth.Account.EnteredWorldChanged += Account_EnteredWorldChanged;

            Host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;

            Host.Handler.Log = new StreamWriter("log.txt", true);
            Host.Handler.Log.WriteLine(DateTime.Now);
            Host.Handler.Log.WriteLine("------------------------");
            Host.Handler.Logging = true;

            Host.Handler.AddHandler<GetRoleIdByName_ReS77>(OnRoleIdReceive);
            Host.Handler.AddHandler<RoleBaseInfoS5C>(OnRoleBaseReceive);
            

            Host.Handler.AddHandler<CreateRole_ReS55>(OnCreateRole);
            Host.Handler.AddHandler<LoginErrorS05>(OnLoginError);
            Auth.LogIn(testMail, testPass, true);
          
            while (true)
            {
                string name = Console.ReadLine();

                Host.Send(new GetRoleIdByNameC76(name));
                Host.Send(new GetRoleBaseInfoC5B(0x10203040));
            }
        }
       
        private static void OnLoginError(object sender, PacketEventArgs e)
        {
            var res = (LoginErrorS05)e.Packet;

          
        }
        private static void OnRoleIdReceive(object sender, PacketEventArgs e)
        {
            var res = (GetRoleIdByName_ReS77)e.Packet;

            if (res.ResultCode != 0x00)
            {
                Console.WriteLine("Персонаж не существует");
                return;
            }

            Host.Send(new GetRoleBaseInfoC5B(res.RoleId));
        }
        private static void OnRoleBaseReceive(object sender, PacketEventArgs e)
        {
            var role = ((RoleBaseInfoS5C)e.Packet).RoleBase;

            Console.WriteLine(role.Name);
            Console.WriteLine(role.Forbid.Length);
            foreach (var forbid in role.Forbid)
            {
                Console.WriteLine("    " + forbid.Reason);
                Console.WriteLine("    " + forbid.Createtime);
                Console.WriteLine("    " + forbid.Time);
            }
        }

        private static void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            var status = string.Empty;

            switch (Host.ConnectionInfo.Status)
            {
                case Magic.Data.ConnectionStatus.Connected: status = "Connected"; break;
                case Magic.Data.ConnectionStatus.Disconnected: status = "Disconnected"; break;
                case Magic.Data.ConnectionStatus.Connecting: status = "Connecting"; break;
            }

            Console.WriteLine(status);
        }

        static Random rnd = new Random();
        private static void Account_RolesLoadedChanged(object sender, EventArgs e)
        {
            if (Auth.Account.RolesLoaded != true) return;

            if (Auth.Account.Roles.Count == 0)
            {
                MakeRandomRole();
            }
            else
            {
                Console.WriteLine("Персонажи загружены");
                AccountRoles.EnterWorld(0);
            }
        }
        private static void Account_EnteredWorldChanged(object sender, EventArgs e)
        {
            if (Auth.Account.EnteredWorld)
            {
                Console.WriteLine("Успешный вход в мир за: " + Auth.Account.SelectedRole.Name);
            }
        }

        static void MakeRandomRole()
        {
            var name = rnd.Next().ToString();
            if (name.Length > 9) name = name.Substring(0, 9);

            Console.WriteLine("Создаем персонажа: {0}", name);
            AccountRoles.CreateRole(name, Gender.Male, Occupation.Mage);
        }
        private static void OnCreateRole(object sender, PacketEventArgs e)
        {
            var result = e.Packet as CreateRole_ReS55;

            Console.WriteLine("Результат создания персонажа: " + result.ResultCode);
            if (result.ResultCode.ResultCode == 0)
            {
                Console.WriteLine(result.Role.ToString());
            }
            else
            {
                MakeRandomRole();
            }
        }
    }
}
