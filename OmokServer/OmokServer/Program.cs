using ServerCore;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Google.FlatBuffers;
using OmokServer.Session;
using OmokServer.Managers;
public class Program
{
    static Listener listener = new Listener();
    static void Main(string[] args)
    {
        IPAddress addr = IPAddress.Any;
        listener.StartListen(addr, 8080, () => { return Manager.Session.GenerateSession(); });
        while (true) ;
    }
}