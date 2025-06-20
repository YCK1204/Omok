using ServerCore;
using System.Net;
using OmokServer.Managers;
using Google.FlatBuffers;
using System.Security.Cryptography;

namespace OmokServer.Session
{
    public class ClientSession : PacketSession
    {
        public UInt64 ID { get; set; }
        Aes AES = Manager.Security.GenerateAes();
        ICryptoTransform Encryptor;
        ICryptoTransform Decryptor;
        public override void OnConnected(EndPoint endPoint)
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1);
            var strOffset = builder.CreateString(Manager.Security.RSAKey);
            var data = PKT_S_Pubkey.CreatePKT_S_Pubkey(builder, strOffset);
            var pkt = Manager.Packet.CreatePacket(data, builder, PKT_Type.PKT_S_Pubkey);
            Send(pkt);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            Manager.Packet.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
        }
        public byte[] Encrypt(byte[] data)
        {
            return Manager.Security.AESEncrypt(data, Encryptor);
        }
        public byte[] Decrypt(byte[] data)
        {
            return Manager.Security.AESDecrypt(data, Decryptor);
        }
        public ByteBuffer DecryptAsByteBuffer(byte[] data)
        {
            return new ByteBuffer(Decrypt(data));
        }
        public void SetAes(byte[] key, byte[] iv)
        {
            AES.Key = key;
            AES.IV = iv;
            Encryptor = AES.CreateEncryptor();
            Decryptor = AES.CreateDecryptor();
        }
    }
}
