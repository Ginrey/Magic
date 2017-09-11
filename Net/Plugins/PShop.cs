using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Net.Packets;
using Magic.Net.Packets.Server;
using Magic.Net.Packets.Client;
using Magic.Data;
using Magic.Data.Types;

namespace Magic.Net.Plugins
{
    public delegate void PShopListHandler(object sender, PShopList_ReS39F e);
    public delegate void PShopPlayerGetHandler(object sender, PShopPlayerGet_ReS39D e);
    public class PShop : Plugin
    {
        public event PShopListHandler PShopListReceive = (a, b) => { };
        public event PShopPlayerGetHandler PShopPlayerGetReceive = (a, b) => { };

        protected internal override void Initialize()
        {
            Host.Handler.AddHandler<PShopList_ReS39F>(OnPShopListReceive);
            Host.Handler.AddHandler<PShopPlayerGet_ReS39D>(OnPShopPlayerGetReceive);

            base.Initialize();
        }

        public void GetPShopList()
        {
            Host.Send(new PShopListC39E());
        }
        public void GetPShopList(uint shopType)
        {
            Host.Send(new PShopListC39E(shopType));
        }
        public void EnterPShop(uint otherRoleId)
        {
            Host.Send(new PShopPlayerGetC39C(otherRoleId));
        }

        private void OnPShopListReceive(object sender, PacketEventArgs e)
        {
            var list = (PShopList_ReS39F)e.Packet;
            PShopListReceive(this, list);
        }
        private void OnPShopPlayerGetReceive(object sender, PacketEventArgs e)
        {
            var shop = (PShopPlayerGet_ReS39D)e.Packet;
            PShopPlayerGetReceive(this, shop);
        }
    }
}
