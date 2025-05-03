using ServerCore;
using System.Net;

public class TestSession : PacketSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine("OnConnected");

    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Console.WriteLine("OnDisconnected");
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        Console.WriteLine($"OnRecvPacket {buffer.Count}");

        Thread.Sleep(1000);
        byte[] a = new byte[4];
        var i = BitConverter.ToUInt16(buffer.Array, 2);
        Console.WriteLine(i);
        BitConverter.TryWriteBytes(new Span<byte>(a), 4);
        BitConverter.TryWriteBytes(new ArraySegment<byte>(a, 2, 2), ++i);
        Send(a);
    }

    public override void OnSend(int numOfBytes)
    {
        Console.WriteLine($"OnSend {numOfBytes}");
    }
}
public class Program
{
    static Listener listener = new Listener();
    static TestSession ts = new TestSession();
    static void Main(string[] args)
    {
        Console.WriteLine("hello");
        IPAddress addr = IPAddress.Any;
        listener.StartListen(addr, 8080, () => { return ts; });
        while (true) ;
    }
}