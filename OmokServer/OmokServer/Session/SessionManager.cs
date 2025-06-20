using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer.Session
{
    public class SessionManager
    {
        Dictionary<UInt64, ClientSession> _sessions = new Dictionary<ulong, ClientSession>();
        UInt64 _curId = 0;
        object _lock = new object();
        public ClientSession GenerateSession()
        {
            ClientSession session = new ClientSession();
            lock (_lock)
            {
                session.ID = ++_curId;
            }
            return session;
        }
        public ClientSession Find(UInt64 id)
        {
            ClientSession session = null;
            lock (_lock)
            {
                _sessions.TryGetValue(id, out session);
            }
            return session;
        }
        public bool Remove(UInt64 id)
        {
            bool success;

            lock (_lock)
            {
                success = _sessions.Remove(id);
            }
            return success;
        }
    }
}
