using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;

namespace Magic.Net.Packets
{
    public class PacketsProcessor
    {
        public OOGHost Host { get; private set; }
        public PacketsProcessor(OOGHost host)
        {
            Host = host;
        }


        public void ProcessServerStream(DataStream dataStream)
        {
            while(true)
            {
                uint opcode, length;
                if (!dataStream.TryReadCompactUInt32(out opcode))
                {
                    break;
                }
                if (!dataStream.TryReadCompactUInt32(out length))
                {
                    break;
                }
                if (!dataStream.CanReadBytes((int)length))
                {
                    break;
                }

                var packetId = new PacketIdentifier(opcode, PacketType.ServerPacket);
                var packetStream = new DataStream(dataStream.ReadBytes((int)length));
                packetStream.IsLittleEndian = false;
                dataStream.Flush();

                ProcessServerPacketStream(packetId, packetStream);
            }
            dataStream.Reset();
        }
        public void ProcessServerPacketStream(PacketIdentifier packetId, DataStream dataStream)
        {
            Host.Handler.HandlePacketStream(packetId, dataStream);

            GamePacket gamePacket;
            if (Host.PacketsRegistry.TryGetPacket(packetId, out gamePacket))
            {
                gamePacket.Deserialize(dataStream, Host.Version);
                ProcessServerPacket(packetId, gamePacket);
            }
        }
        public void ProcessServerPacket(PacketIdentifier packetId, GamePacket gamePacket)
        {
            Host.Handler.HandleServerPacket(Host, packetId, gamePacket);
        }
        public void ProcessClientPacket(GamePacket gamePacket, DataStream outputDataStream)
        {
            ProcessClientPacket(gamePacket, GamePacket.GetOnePacketIdentifier(gamePacket), outputDataStream);
        }
        public void ProcessClientPacket(GamePacket gamePacket, PacketIdentifier packetId, DataStream outputDataStream)
        {
            switch (packetId.PacketType)
            {
                case PacketType.ClientContainerC25:
                    ProcessClientPacket(new ClientContainerC25(packetId.PacketId, gamePacket), outputDataStream);
                    return;
                case PacketType.ClientContainer:
                    ProcessClientPacket(new ClientContainerC22(packetId.PacketId, gamePacket), outputDataStream);
                    return;
                case PacketType.ClientPacket:
                    outputDataStream.IsLittleEndian = false;

                    Host.Handler.HandleClientPacket(Host, packetId, gamePacket);
                    gamePacket.Serialize(outputDataStream, Host.Version);

                    Host.Handler.HandlePacketStream(packetId, outputDataStream);

                    outputDataStream.PushFront(EndianBitConverter.Big.GetCompactUInt32Bytes(outputDataStream.Count));
                    outputDataStream.PushFront(EndianBitConverter.Big.GetCompactUInt32Bytes(packetId.PacketId));

                    return;

                default: throw new ArgumentException("Unknown packet type");
            }
        }
    }
}
