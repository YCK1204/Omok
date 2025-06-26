using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    public class PacketFormat_CSharp
    {
        public static string ServerPMTotal = @"

using Google.FlatBuffers;
using ServerCore;
using System;
using OmokServer.Session;
using System.Collections.Generic;
using System.Security.Cryptography;

public class PacketManager
{{
    Dictionary<ushort, Action<PacketSession, byte[]>> _handler = new Dictionary<ushort, Action<PacketSession, byte[]>>();
    public PacketManager()
    {{
        Register();
    }}
    void Register()
    {{
        {0}
    }}
    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += sizeof(ushort);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);

        Action<PacketSession, byte[]> action = null;
        if (_handler.TryGetValue(id, out action))
        {{
            byte[] bytes = buffer.Array;
            int start = (buffer.Offset + PacketHederSize);
            int end = start + (size - PacketHederSize);
            action.Invoke(session, bytes[start..end]);
        }}
    }}
    ushort PacketHederSize = 4;
    public byte[] CreatePacket<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();

        return Serialize(bytes, builder, id);
    }}
    public byte[] CreatePacketWithRSA<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id, RSA rsa) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();
        var encryptedData = rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);

        return Serialize(encryptedData, builder, id);
    }}
    public byte[] CreatePacketWithAES<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id, ClientSession clientSession) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();
        var encrpytedData = clientSession.Encrypt(bytes);

        return Serialize(encrpytedData, builder, id);
    }}
    byte[] Serialize(byte[] data, FlatBufferBuilder builder, PKT_Type id)
    {{
        ushort size = (ushort)(data.Length + PacketHederSize);
        ushort count = 0;
        ArraySegment<byte> segment = new ArraySegment<byte>(new byte[size]);

        BitConverter.TryWriteBytes(new Span<byte>(segment.Array, count, count + sizeof(ushort)), size);
        count += sizeof(ushort);
        BitConverter.TryWriteBytes(new Span<byte>(segment.Array, count, count + sizeof(ushort)), (ushort)id);
        count += sizeof(ushort);
        Buffer.BlockCopy(data, 0, segment.Array, count, data.Length);

        return segment.Array;
    }}
}}
";
        public static string ClientPMTotal = @"

using Google.FlatBuffers;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class PacketManager
{{
    Dictionary<ushort, Action<PacketSession, byte[]>> _handler = new Dictionary<ushort, Action<PacketSession, byte[]>>();
    public PacketManager()
    {{
        Register();
    }}
    void Register()
    {{
		{0}
    }}
    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += sizeof(ushort);
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += sizeof(ushort);

        Action<PacketSession, byte[]> action = null;
        if (_handler.TryGetValue(id, out action))
        {{
            byte[] bytes = buffer.Array;
            int start = (buffer.Offset + PacketHederSize);
            int end = start + (size - PacketHederSize);
            action.Invoke(session, bytes[start..end]);
        }}
    }}
    ushort PacketHederSize = 4;
    public byte[] CreatePacket<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();

        return Serialize(bytes, builder, id);
    }}
    public byte[] CreatePacketWithRSA<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id, RSA rsa) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();
        var encryptedData = rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);

        return Serialize(encryptedData, builder, id);
    }}
    public byte[] CreatePacketWithAES<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id, ServerSession serverSession) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();
        var encrpytedData = serverSession.Encrypt(bytes);
        
        return Serialize(encrpytedData, builder, id);
    }}
    byte[] Serialize(byte[] data, FlatBufferBuilder builder, PKT_Type id)
    {{
        ushort size = (ushort)(data.Length + PacketHederSize);
        ushort count = 0;
        ArraySegment<byte> segment = new ArraySegment<byte>(new byte[size]);

        BitConverter.TryWriteBytes(new Span<byte>(segment.Array, count, count + sizeof(ushort)), size);
        count += sizeof(ushort);
        BitConverter.TryWriteBytes(new Span<byte>(segment.Array, count, count + sizeof(ushort)), (ushort)id);
        count += sizeof(ushort);
        Buffer.BlockCopy(data, 0, segment.Array, count, data.Length);

        return segment.Array;
    }}
}}
";
        // 0 패킷 테이블 이름
        public static string PMRegister = @"_handler.Add((ushort)PKT_Type.{0}, PKT_Handler.{0}Handler);";
    }
}