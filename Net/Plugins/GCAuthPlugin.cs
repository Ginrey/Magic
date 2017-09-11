using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Net.Packets;
using Magic.Net.Plugins.GameCenter;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.Data;

namespace Magic.Net.Plugins
{
    public class GCAuthPlugin : AuthPlugin
    {
        private PWAccount[] emptyAccounts = { };
        private string Token { get; set; }

        public int AccountIndex { get; private set; }
        public PWAccount[] PWAccounts { get; private set; }

        public string GCLogin { get; set; }
        public string GCPassword { get; set; }


        public override void SetAuthData(string login, string password, bool force)
        {
            GCLogin = login;
            GCPassword = password;

            base.SetAuthData(Account.Login, Account.Password, force);
        }
        public override void LogIn(string login, string password, bool force)
        {
            SetAuthData(login, password, force);
            LoadAccounts();

            base.LogIn();
        }
        public virtual void LoadAccounts(string gcLogin, string gcPassword)
        {
            GCLogin = gcLogin;
            GCPassword = gcPassword;

            LoadAccounts();
        }
        public virtual void LoadAccounts()
        {
            try
            {
                var authData = GCAuth.Login(GCLogin, GCPassword);

                PWAccounts = authData.Accounts;
                Token = authData.Token;
                return;
            }
            catch
            {
                Token = string.Empty;
            }
        }

        protected override void Receive_ServerInfo(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    LoadAccounts();
                }

                var token = Token;
                Token = string.Empty;

                if (string.IsNullOrEmpty(token))
                {
                    Host.Close();
                    return;
                }

                base.SetAuthData(PWAccounts[AccountIndex].UserId, token, Account.Force);

                var hash = Encoding.ASCII.GetBytes(token);
                Connection.MD5.SetHash(Account.Login, hash);

                Host.Send(new LogginAnnounceC03(Account.Login, hash));
            }
        }
    }
}
