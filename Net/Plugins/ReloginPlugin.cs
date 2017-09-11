using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using System.Threading;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Data;
using Magic.Net.Packets.Server;

namespace Magic.Net.Plugins
{
    public class ReloginPlugin : Plugin
    {
        private Random random = new Random();

        /// <summary>
        /// To seconds
        /// </summary>
        public int ReloginInterval { get; set; }
        /// <summary>
        /// To seconds
        /// </summary>
        public int MinimalInterval { get; set; }
        /// <summary>
        /// To miliseconds
        /// </summary>
        public int AddingRandomTime { get; set; }

        public bool Reconnecting { get; private set; }
        public UnixTime LastConnecting { get; private set; }
        public UnixTime LastLogin { get; private set; }

        public WorldEnteringPlugin WorldEntering { get; private set; }

        protected internal override void Initialize()
        {
            WorldEntering = Host.Plugins.Register<WorldEnteringPlugin>();

            Host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;
            Host.Handler.AddHandler<ServerInfoS01>(OnServerInfoReceive);

            ReloginInterval = 30;
            MinimalInterval = 5;
            AddingRandomTime = 5000;

            Enabled = true;
            
            LastConnecting = new UnixTime(0);
            LastLogin = new UnixTime(0);
        }

        private void OnServerInfoReceive(object sender, PacketEventArgs e)
        {
            LastLogin = UnixTime.Now;
        }

        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            switch (Host.ConnectionInfo.Status)
            {
                case ConnectionStatus.Connected:
                    LastConnecting = UnixTime.Now;
                    break;
                case ConnectionStatus.Disconnected:
                    if (Enabled && Host.Started)
                    {
                        Reconnecting = true;
                        new Thread(OnReconnecting).Start();
                    }
                    break;
            }
        }

        private void OnReconnecting()
        {
            var sleepTime = GetSleepTime();
            Thread.Sleep(sleepTime);

            Host.Connect();

            Reconnecting = false;
        }
        private int GetSleepTime()
        {
            var sleepLogin = ReloginInterval - (UnixTime.Now.Timestamp - LastLogin.Timestamp);

            var sleep = 1000 * Math.Max(sleepLogin, MinimalInterval);
            if (sleep < 0) sleep = 0;

            var rnd = random.Next(0, AddingRandomTime);

            return sleep + rnd;
        }
    }
}
