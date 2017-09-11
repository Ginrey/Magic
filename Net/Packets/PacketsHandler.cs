﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using System.IO;

namespace Magic.Net.Packets
{
    public class PacketsHandler
    {
        protected Dictionary<PacketIdentifier, List<PacketEventHandler>> handlers;
        public TextWriter Log { get; set; }
        public bool Logging { get; set; }

        public PacketsHandler() : this(null)
        {

        }
        public PacketsHandler(TextWriter log)
        {
            handlers = new Dictionary<PacketIdentifier, List<PacketEventHandler>>();
            Log = log;
        }

        public virtual void AddHandler<T>(PacketEventHandler handler) where T : GamePacket
        {
            var packetIds = GamePacket.GetPacketIdentifiers<T>();

            foreach (var packetId in packetIds)
            {
                AddHandler(packetId, handler);
            }
        }
        public virtual void AddHandler(uint packetId, PacketType packetType, PacketEventHandler handler)
        {
            AddHandler(new PacketIdentifier(packetId, packetType), handler);
        }
        public virtual void AddHandler(PacketIdentifier packetId, PacketEventHandler handler)
        {
            List<PacketEventHandler> handlersList;
            if (!handlers.TryGetValue(packetId, out handlersList))
            {
                handlersList = new List<PacketEventHandler>();
                handlers.Add(packetId, handlersList);
            }
            handlersList.Add(handler);
        }

        public virtual void HandlePacketStream(PacketIdentifier packetId, DataStream ds)
        {
            if (Logging)
            {
                Log.WriteLine(packetId);
            }
        }

        public virtual void HandleServerPacket(OOGHost host, PacketIdentifier packetId, GamePacket gamePacket)
        {
            HandlePacket(host, packetId, gamePacket);
        }
        public virtual void HandleClientPacket(OOGHost host, PacketIdentifier packetId, GamePacket gamePacket)
        {
            HandlePacket(host, packetId, gamePacket);
        }
        private void HandlePacket(OOGHost host, PacketIdentifier packetId, GamePacket gamePacket)
        {
            gamePacket.HandleData(host.Data);

            List<PacketEventHandler> handlersList;
            if (handlers.TryGetValue(packetId, out handlersList))
            {
                var eventArgs = new PacketEventArgs(packetId, gamePacket);
                foreach (var handler in handlersList)
                {
                    handler(this, eventArgs);
                }
            }
        }
    }
}
