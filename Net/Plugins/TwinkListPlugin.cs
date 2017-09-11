using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.Data.Types;
using Magic.Threading;
using System.Threading;

namespace Magic.Net.Plugins
{
    public delegate void TwinkListEventHandler(object sender, TwinkListEventArgs e);
    public class TwinkListEventArgs
    {
        public TwinkListEventArgs(uint key)
        {
            Key = key;

            Roles = new Dictionary<uint, RoleBaseInfoS5C>(16);
            RequestNames = new HashSet<string>();
        }

        public uint Key { get; private set; }
        public Dictionary<uint, RoleBaseInfoS5C> Roles { get; private set; }
        public HashSet<string> RequestNames { get; private set; }

        public bool ContainsName(string name)
        {
            return RequestNames.Contains(name);
        }
        public bool RoleIdContains(uint roleId)
        {
            return Roles.ContainsKey(roleId);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
    [Obsolete]
    public class TwinkListPlugin : Plugin
    {
        HashSet<string> names = new HashSet<string>();
        Dictionary<uint, TwinkListEventArgs> args = new Dictionary<uint, TwinkListEventArgs>();

        ActionTimer timer;
        int pulseCount = 0;
        int lastPulse = 0;
        int timercc = 0;

        public event TwinkListEventHandler OnTwinksReceive = (a, b) => { };
        private void OnComplete(TwinkListEventArgs arg)
        {
            OnTwinksReceive(this, arg);
        }
        private void Flush()
        {
            timercc = 0;
            foreach (var arg in args.Values)
            {
                OnComplete(arg);
            }
            args.Clear();
            pulseCount = 0;
        }
        private void Pulse()
        {
            lock(args)
            {
                timercc = 0;
                pulseCount++;
            }
        }
        private void AuthFlush(object state)
        {
            lock(args)
            {
                if (lastPulse == pulseCount && pulseCount < args.Count * 16)
                {
                    timercc++;
                }
                else
                {
                    timercc = 0;
                }
                lastPulse = pulseCount;
                if (timercc == 30)
                {
                    Flush();
                }
            }


            if (pulseCount >= args.Count * 16 && pulseCount > 0)
            {
                var pulse = pulseCount;
                Thread.Sleep(200);
                lock (args)
                {
                    if (pulseCount == pulse)
                    {
                        Flush();
                    }
                }
            }

        }

        public void BeginGetTwinks(string name)
        {
            Host.Send(new GetRoleIdByNameC76(name));
        }
        public void BeginGetTwinks(uint roleId)
        {
            BeginGetTwinks(roleId, null);
        }
        public void BeginGetTwinks(uint roleId, string name)
        {
            uint key = roleId / 16;
            lock (args)
            {
                TwinkListEventArgs arg;
                if (!args.TryGetValue(key, out arg))
                {
                    arg = new TwinkListEventArgs(key);
                    args[key] = arg;

                    BeginGetTwinksByKey(key);
                }
                if (name != null)
                {
                    arg.RequestNames.Add(name);
                }
            }
        }
        private void BeginGetTwinksByKey(uint key)
        {
            uint[] element = new uint[1];
            for (uint i = 0; i < 16; i++)
            {
                element[0] = key * 16 + i;
                Host.Send(new GetRoleBaseInfoC5B(element));
            }
        }

        protected internal override void Initialize()
        {
            timer = new ActionTimer(AuthFlush);

            Host.Data.Register<AccountInfo>();

            Host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;

            Host.Handler.AddHandler<RoleBaseInfoS5C>(OnRoleBaseReceive);
            Host.Handler.AddHandler<GetRoleIdByName_ReS77>(OnRoleIdReceive);

            base.Initialize();
        }

        private void OnRoleBaseReceive(object sender, PacketEventArgs e)
        {
            var role = (RoleBaseInfoS5C)e.Packet;

            var roleId = role.RoleBase.Id;
            if (roleId == 0) return;
            var key = roleId / 16;

            TwinkListEventArgs arg;
            bool contains;
            lock(args)
            {
                if (contains = args.TryGetValue(key, out arg))
                {
                    arg.Roles.Add(key, role);
                }
            }
        }
        private void OnRoleIdReceive(object sender, PacketEventArgs e)
        {
            var role = (GetRoleIdByName_ReS77)e.Packet;

            if (role.ResultCode != 0)
            {
                var arg = new TwinkListEventArgs(0);
                arg.RequestNames.Add(role.RoleName);

                OnComplete(arg);
            }
            else
            {
                BeginGetTwinks(role.RoleId, role.RoleName);
            }
        }
        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            if (Host.ConnectionInfo.Status == ConnectionStatus.Disconnected)
            {
                OnStop();
            }
        }

        protected internal override void OnStart()
        {
            timer.Start(0, 100);
            base.OnStart();
        }
        protected internal override void OnStop()
        {
            timer.Stop();
            names.Clear();

            lock(args)
            {
                Flush();
            }
            base.OnStop();
        }
    }
}
