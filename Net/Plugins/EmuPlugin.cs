using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Net.Packets.Client;

namespace Magic.Net.Plugins
{
    public class EmuPlugin : Plugin
    {
        public AccountInfo Account { get; private set; }
        protected internal override void Initialize()
        {
            Account = Host.Data.Register<AccountInfo>();
            Account.EnteredWorldChanged += Account_EnteredWorldChanged;

            base.Initialize();
        }

        private void Account_EnteredWorldChanged(object sender, EventArgs e)
        {
            if (Account.EnteredWorld && Enabled)
            {
                Host.Send(new GetInventoryC27());
            }
        }
    }
}
