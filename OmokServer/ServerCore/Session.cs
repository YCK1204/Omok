using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;

namespace ServerCore
{
    public abstract class Session
    {
        Socket _socket;
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        bool _disconnected = false;
        RecvBuffer _recvBuffer = new RecvBuffer(ushort.MaxValue);
        object _lock = new object();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnSend(int numOfBytes);
        public abstract int OnRecv(ArraySegment<byte> buffer);
        public void Start(Socket socket)
        {
            _socket = socket;
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            RegisterRecv();
        }
        private void RegisterRecv()
        {
            if (_disconnected == true)
                return;
            try
            {
                _recvBuffer.Clean();
                var writeSegment = _recvBuffer.WriteSegment;
                _recvArgs.SetBuffer(writeSegment.Array, writeSegment.Offset, writeSegment.Count);
                bool pending = _socket.ReceiveAsync(_recvArgs);

                if (pending == false)
                    OnRecvCompleted(null, _recvArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to RegisterRecv {e.ToString()}");
            }
        }
        private void OnRecvCompleted(object o, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                {
                    Disconnect();
                    return;
                }

                int processLen = OnRecv(_recvBuffer.ReadSegment);
                if (processLen > _recvBuffer.DataSize)
                {
                    Disconnect();
                    return;
                }
                else if (_recvBuffer.OnRead(processLen) == false)
                {
                    Disconnect();
                    return;
                }
                else
                {
                    RegisterRecv();
                }
            }
            else
            {
                Console.WriteLine($"Failed to OnRecvCompleted\n" +
                    $"[Error] : {args.SocketError.ToString()}\n" +
                    $"[BytesTransferred] : {args.BytesTransferred.ToString()}");
                Disconnect();
            }
        }
        public void Send(ArraySegment<byte> bytes)
        {
            if (_disconnected == true)
                return;
            lock (_lock)
            {
                _sendQueue.Enqueue(bytes);
                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }
        private void RegisterSend()
        {
            if (_disconnected == true)
                return;
            try
            {
                while (_sendQueue.Count > 0)
                    _pendingList.Add(_sendQueue.Dequeue());
                _sendArgs.BufferList = _pendingList;
                bool pending = _socket.SendAsync(_sendArgs);

                if (pending == false)
                    OnSendCompleted(null, _sendArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to ReigsterSend {e.ToString()}");
            }
        }
        private void OnSendCompleted(object o, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                _pendingList.Clear();
                OnSend(args.BytesTransferred);
                if (_sendQueue.Count > 0)
                    RegisterSend();
            }
            else
            {
                Console.WriteLine($"Failed to OnSendCompleted\n" +
                    $"[Error] : {args.SocketError.ToString()}\n" +
                    $"[BytesTransferred] : {args.BytesTransferred.ToString()}");
                Disconnect();
            }
        }
        private void Disconnect()
        {
            lock (_lock)
            {
                if (_disconnected == true)
                    return;
                _disconnected = true;
                OnDisconnected(_socket.RemoteEndPoint);
                _socket.Shutdown(SocketShutdown.Both);
            }
        }
    }
    public abstract class PacketSession : Session
    {
        static readonly ushort HeaderSize = 2;
        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;

            while (buffer.Count >= HeaderSize)
            {
                ushort headerSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);

                if (headerSize > buffer.Count)
                    break;
                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, headerSize));
                processLen += headerSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + headerSize, buffer.Count - headerSize);
            }
            return processLen;
        }
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }
}
