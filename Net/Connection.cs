using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Magic.Data;
using Magic.IO;


namespace Magic.Net
{
    public class Connection
    {
        Socket socket;

        object socketLock = new object();

        public OOGHost Host { get; private set; }
        private ConnectionData Data { get; set; }
        private ConnectionInfo Information { get; set; }
        
        public bool IsWork { get; private set; }
        
        SocketAsyncEventArgs socketArgsRecv;
        AsyncCallback socketAsyncConnect;
        DataStream ReceiveStream;

        public Connection(OOGHost host)
        {

            Host = host;
            Data = host.Data;
            Information = host.Data.Register<ConnectionInfo>();

            ReceiveStream = new DataStream();

            socketAsyncConnect = EndConnect;
            socketArgsRecv = new SocketAsyncEventArgs();

            socketArgsRecv.SetBuffer(new byte[1024], 0, 1024);
            socketArgsRecv.Completed += socketArgsRecv_Completed;
        }

        public void Connect()
        {
            lock (socketLock)
            {
                if (IsWork)
                    return;
                IsWork = true;

            }
            BeginConnect();
        }
        private void BeginConnect()
        {
            Information.Status = ConnectionStatus.Connecting;

            ReceiveStream.Clear();
            socket = Host.SocketFactory.BuildSocket();
            socket.BeginConnect(Host.GameServer.Host, Host.GameServer.Port, socketAsyncConnect, socket);
        }
        private void EndConnect(IAsyncResult res)
        {
            try
            {
                var skt = res.AsyncState as Socket;
                skt.EndConnect(res);

                if (!skt.Connected)
                {
                    Close();
                    return;
                }
            }
            catch
            {
                Close();
                return;
            }
            
            Information.Status = ConnectionStatus.Connected;
            StartReceive();
        }
        public void Close()
        {
            lock (socketLock)
            {
                if (!IsWork) return;
                IsWork = false;
            }
            if (socket != null)
            {
                socket.Close();
            }
            Information.Status = ConnectionStatus.Disconnected;
        }


        // RECEIVE ASYNC
        private void StartReceive()
        {
            try
            {
                if (!socket.ReceiveAsync(socketArgsRecv))
                {
                    ReceiveProcess(socketArgsRecv);
                }
            }
            catch
            {
                Close();
            }
        }
        private void socketArgsRecv_Completed(object sender, SocketAsyncEventArgs e)
        {
            ReceiveProcess(e);
        }
        private void ReceiveProcess(SocketAsyncEventArgs socketArgs)
        {
            if (!IsWork)
                return;
            try
            {
                ReceiveStream.Clear();
                ReceiveStream.Reset();
                if (socketArgs.SocketError == SocketError.SocketError ||
                    socketArgs.BytesTransferred == 0)
                {
                    Close();
                    return;
                }
                Information.ReceivedBytes += socketArgs.BytesTransferred;
                Information.TotalReceivedBytes += socketArgs.BytesTransferred;

                var buffer = socketArgs.Buffer;
                var count = socketArgs.BytesTransferred;
                if (Information.RC4Dec != null)
                {
                    Information.RC4Dec.Decrypt(buffer, 0, count);
                }
               /* if (Information.MPPC != null)
                {
                    buffer = Information.MPPC.Unpack(buffer, 0, count);
                    count = buffer.Length;
                }*/

                ReceiveStream.PushBack(buffer, 0, count);
            }
            catch
            {
                Close();
            }

            Host.Processor.ProcessServerStream(ReceiveStream);
            StartReceive();
        }
        
        public void Send(byte[] buffer)
        {
            Send(buffer, 0, buffer.Length);
        }
        public void Send(byte[] buffer, int offset, int count)
        {
            if (!IsWork) return;
            try
            {
                lock (socket)
                {
                    if (Information.RC4Enc != null)
                    {
                        Information.RC4Enc.Encrypt(buffer, offset, count);
                    }
                    
                    socket.Send(buffer, offset, count, SocketFlags.None);
                    Information.SendedBytes += count;
                    Information.TotalSendedBytes += count;
                }
            }
            catch
            {
                Close();
            }
        }
    }
}
