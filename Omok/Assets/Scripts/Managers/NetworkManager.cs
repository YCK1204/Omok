using Google.FlatBuffers;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;
public class ServerSession : PacketSession
{
    #region AES
    Aes _aes = Aes.Create();
    readonly int PacketHeaderSize = 4;
    ICryptoTransform _encryptor;
    ICryptoTransform _decryptor;
    public ServerSession()
    {
        _aes.Mode = CipherMode.CBC;
        _aes.Padding = PaddingMode.PKCS7;
        _encryptor = _aes.CreateEncryptor();
        _decryptor = _aes.CreateDecryptor();
    }
    public byte[] Encrypt(byte[] value)
    {
        return _encryptor.TransformFinalBlock(value, 0, value.Length);
    }
    public byte[] Decrypt(byte[] value)
    {
        return _decryptor.TransformFinalBlock(value, PacketHeaderSize, value.Length - PacketHeaderSize);
    }
    public ByteBuffer DecryptAsByteBuffer(byte[] data)
    {
        return new ByteBuffer(Decrypt(data));
    }
    public Tuple<byte[], byte[]> GetAesInfo()
    {
        return new Tuple<byte[], byte[]>(_aes.Key, _aes.IV);
    }
    #endregion
    public override void OnConnected(EndPoint endPoint)
    {
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
    }

    public override void OnRecvPacket(ArraySegment<byte> buffer)
    {
        GameManager.Network.Push(buffer);
    }

    public override void OnSend(int numOfBytes)
    {
        Debug.Log($"OnSend {numOfBytes}");
    }
}
public class NetworkManager
{
    Connector _connector = new Connector();
    ServerSession _session = new ServerSession();
    Queue<ArraySegment<byte>> _pktQueue = new Queue<ArraySegment<byte>>();
    public void Connect()
    {
        IPAddress addr = IPAddress.Loopback;

        _connector.Connect(addr, 8080, () => { return _session; });
    }
    public void Update()
    {
        while (_pktQueue.Count > 0)
        {
            var buffer = _pktQueue.Dequeue();
            GameManager.Packet.OnRecvPacket(_session, buffer);
        }
    }
    public void Push(ArraySegment<byte> buffer)
    {
        _pktQueue.Enqueue(buffer);
    }
    public Tuple<byte[], byte[]> GetAesInfo()
    {
        return _session.GetAesInfo();
    }
    public byte[] Encrypt(byte[] value)
    {
        return _session.Encrypt(value);
    }
    public byte[] Decrypt(byte[] value)
    {
        return _session.Decrypt(value);
    }
}
