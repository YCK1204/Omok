using Google.FlatBuffers;
using ServerCore;
using System;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;
public partial class PKT_Handler
{
    public static void PKT_S_PubkeyHandler(PacketSession session, byte[] buffer)
    {
        RSA rsa = RSA.Create();
        FlatBufferBuilder builder = new FlatBufferBuilder(1);

        ByteBuffer bb = new ByteBuffer(buffer);
        var pkt = PKT_S_Pubkey.GetRootAsPKT_S_Pubkey(bb);
        rsa.FromXmlString(pkt.Key);
        var keyInfo = GameManager.Network.GetAesInfo();
        //var encryptedData = rsa.Encrypt(concated, RSAEncryptionPadding.Pkcs1);

        var keyOffset = PKT_C_Key.CreateKeyVector(builder, keyInfo.Item1);
        var ivOffset = PKT_C_Key.CreateKeyVector(builder, keyInfo.Item2);
        var data = PKT_C_Key.CreatePKT_C_Key(builder, keyOffset, ivOffset);
        var packet = GameManager.Packet.CreatePacketWithRSA(data, builder, PKT_Type.PKT_C_Key, rsa);
        session.Send(packet);

        //builder.Clear();

        //var a = PKT_C_Test.CreatePKT_C_Test(builder);
        //var s = GameManager.Packet.CreatePacket(a, builder, PKT_Type.PKT_C_Test);
        //session.Send(s);
    }
    public static void PKT_S_TestHandler(PacketSession session, byte[] buffer)
    {
    }
}
