using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.Data.Types;

namespace Magic.Net.Plugins
{
    public delegate void TargetInfoEventHandler(object sender, TargetInfoEventArgs e);
    public class TargetInfoEventArgs
    {
        public TargetInfoEventArgs(uint targedId, HpMp hp) : this(targedId, false, 0, hp, null)
        {

        }
        public TargetInfoEventArgs(uint targetId, int level, HpMp hp, HpMp mp) : this(targetId, true, level, hp, mp)
        {

        }
        public TargetInfoEventArgs(uint targetId, bool isRole, int level, HpMp hp, HpMp mp)
        {
            TargetId = targetId;

            IsRole = isRole;
            Level = level;
            Hp = hp;
            Mp = mp;
        }

        public uint TargetId { get; private set; }
        public bool IsRole { get; private set; }
        public int Level { get; private set; }
        public HpMp Hp { get; private set; }
        public HpMp Mp { get; private set; }
    }
    public class TargetPlugin : Plugin
    {
        public event TargetInfoEventHandler TargetInfoReceive = (a, b) => { };
        public event EventHandler TargetChanged = (a, b) => { };
        public event EventHandler GreetingChanged = (a, b) => { };

        private uint targetId;
        public uint TargetId
        {
            get
            {
                return targetId;
            }
            set
            {
                targetId = value;
                TargetChanged(this, new EventArgs());
            }
        }
        private uint greetingId;
        public uint GreetingId
        {
            get
            {
                return greetingId;
            }
            set
            {
                greetingId = value;
                GreetingChanged(this, new EventArgs());
            }
        }

        protected internal override void Initialize()
        {
            Host.Handler.AddHandler<ResetTargetReS27>(OnResetTarget);
            Host.Handler.AddHandler<InvalidTargetSC2>(OnResetTarget);
            Host.Handler.AddHandler<SelectTargetReS34>(OnSelectTargetRe);
            Host.Handler.AddHandler<TargetInfoS20>(OnRoleInfo);
            Host.Handler.AddHandler<NpcTargetInfoS21>(OnNpcInfo);
            Host.Handler.AddHandler<NpcGreetingS46>(OnNpcGreeting);

            Host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;

            base.Initialize();
        }

        public void SetTarget(uint targetId)
        {
            Host.Send(new SetTargetC02(targetId));
        }
        public void ResetTarget()
        {
            Host.Send(new ResetTargetC08());
        }
        public void CloseDialog()
        {
            GreetingId = 0;
        }
        public void OpenDialog()
        {
            OpenDialog(targetId);
        }
        public void OpenDialog(uint worldId)
        {
            Host.Send(new OpenNpcDialogC23(worldId));
        }

        private void OnResetTarget(object sender, PacketEventArgs e)
        {
            TargetId = 0;
        }
        private void OnSelectTargetRe(object sender, PacketEventArgs e)
        {
            TargetId = ((SelectTargetReS34)e.Packet).TargetId;
        }
        private void OnRoleInfo(object sender, PacketEventArgs e)
        {
            var info = (TargetInfoS20)e.Packet;
            TargetInfoReceive(this, new TargetInfoEventArgs(info.RoleId, info.Level, info.Hp, info.Mp));
        }
        private void OnNpcInfo(object sender, PacketEventArgs e)
        {
            var info = (NpcTargetInfoS21)e.Packet;
            TargetInfoReceive(this, new TargetInfoEventArgs(info.WorldId, info.Hp));
        }
        private void OnNpcGreeting(object sender, PacketEventArgs e)
        {
            GreetingId = ((NpcGreetingS46)e.Packet).WorldId;
        }

        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            if (Host.ConnectionInfo.Status == Data.ConnectionStatus.Disconnected)
            {
                TargetId = 0;
                GreetingId = 0;
            }
        }
    }
}
