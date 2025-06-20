using Google.FlatBuffers;
using OmokServer.Managers;
using OmokServer.Session;
using ServerCore;
using System.Text;

public partial class PKT_Handler
{
    public static void PKT_C_KeyHandler(PacketSession session, byte[] buffer)
    {
        ClientSession clientSession = session as ClientSession;
        int rsaEncSize = 256;
        if (buffer.Length != rsaEncSize) // todo 암호화 크기가 다른 경우 조작 의심
            return;

        var data = Manager.Security.RSADecrypt(buffer);
        ByteBuffer bb = new ByteBuffer(data);
        var pkt = PKT_C_Key.GetRootAsPKT_C_Key(bb);
        byte[] key = pkt.GetKeyArray();
        byte[] iv = pkt.GetIvArray();
        clientSession.SetAes(key, iv);

        string test = "hello world";
        var bytes = Encoding.UTF8.GetBytes(test);
        var a = clientSession.Encrypt(bytes);
        var b = clientSession.Decrypt(a);
        var c =Encoding.UTF8.GetString(b);
        Console.WriteLine(c);
    }
}