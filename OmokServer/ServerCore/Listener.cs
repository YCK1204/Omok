using System;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    public class Listener
    {
        Socket _socket;
        Func<PacketSession> _sessionFactory;
        public void StartListen(IPAddress address, int port, Func<PacketSession> sessionFactory)
        {
            _sessionFactory = sessionFactory;
            IPEndPoint ep = new IPEndPoint(address, port);
            _socket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _socket.Bind(ep);
            _socket.Listen(5);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = ep;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }
        private void RegisterAccept(SocketAsyncEventArgs args)
        {
            try
            {
                args.AcceptSocket = null;
                bool pending = _socket.AcceptAsync(args);

                if (pending == false)
                    OnAcceptCompleted(null, args);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to RegisterAccept {e.ToString()}");
            }
        }
        private void OnAcceptCompleted(object o, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                PacketSession session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.RemoteEndPoint);
                RegisterAccept(args);
            }
            else
            {
                Console.WriteLine($"Failed to OnAcceptCompleted {args.SocketError.ToString()}");
            }
        }
    }
}