using OmokServer.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer.Managers
{
    public class Manager
    {
        static Manager _instance = new Manager();
        SecurityManager _security = new SecurityManager();
        PacketManager _packet = new PacketManager();
        SessionManager _session = new SessionManager();
        public static SecurityManager Security { get { return _instance._security; } }
        public static PacketManager Packet { get { return _instance._packet; } }
        public static SessionManager Session { get { return _instance._session; } }
    }
}
