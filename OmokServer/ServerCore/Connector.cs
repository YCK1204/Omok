using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Connector
    {
        Socket _socket;
        Func<PacketSession> _sessionFactory;
        public void Connect(IPAddress address, int port, Func<PacketSession> sessionFactory)
        {
            _sessionFactory = sessionFactory;
            IPEndPoint ep = new IPEndPoint(address, port);
            _socket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = ep;
            args.UserToken = _socket;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectCompleted);

            RegisterConnect(args);
        }
        private void RegisterConnect(SocketAsyncEventArgs args)
        {
            bool pending = _socket.ConnectAsync(args);
            if (pending == false)
                OnConnectCompleted(null, args);
        }
        private void OnConnectCompleted(object o, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                PacketSession session = _sessionFactory();
                session.Start(args.ConnectSocket);
                session.OnConnected(args.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine("connected failed");
                RegisterConnect(args);
            }
        }
    }
}
