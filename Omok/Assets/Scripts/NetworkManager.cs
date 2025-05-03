using ServerCore;
using System;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.LightTransport;

public class TestSession : PacketSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log("OnConnected");
        byte[] a = new byte[4];
        BitConverter.TryWriteBytes(new Span<byte>(a), 4);
        BitConverter.TryWriteBytes(new ArraySegment<byte>(a, 2, 2), 0);
        Send(a);
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log("OnDisconnected");
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        Debug.Log($"OnRecvPacket {buffer.Count}");
        byte[] a = new byte[4];
        var i = BitConverter.ToUInt16(buffer.Array, 2);
        Debug.Log(i);
        BitConverter.TryWriteBytes(new Span<byte>(a), 4);
        BitConverter.TryWriteBytes(new ArraySegment<byte>(a, 2, 2), ++i);
        Send(a);
    }

    public override void OnSend(int numOfBytes)
    {
        Debug.Log($"OnSend {numOfBytes}");
    }
}
public class NetworkManager : MonoBehaviour
{
    Connector _connector = new Connector();
    TestSession session = new TestSession();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IPAddress addr = IPAddress.Loopback;
        _connector.Connect(addr, 8080, () => { return session; });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
