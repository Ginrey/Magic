using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Data.Types;
using Magic.IO;

namespace Magic.Net.Packets.Server
{
    [PacketIdentifier(0x0F, PacketType.ServerContainer)]
    public class ObjectMoveS0F : GamePacket<WorldObjectsSet>
    {
        public uint ObjectId;
        public Point3F Position;

        protected internal override void HandleData(WorldObjectsSet data)
        {
            lock (data.Objects)
            {
                WorldObject obj;
                if (data.Objects.TryGetValue(ObjectId, out obj))
                {
                    obj.Position = Position;
                }
            }

            base.HandleData(data);
        }
        public override DataStream Deserialize(DataStream ds, VersionControl vc)
        {
            ObjectId = ds.ReadUInt32();
            Position = ds.Read<Point3F>();

            return base.Deserialize(ds, vc);
        }
    }
}
