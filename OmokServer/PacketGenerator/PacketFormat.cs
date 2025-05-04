﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    public class PacketFormat_CSharp
    {
        // 0 ServerSession or ClientSession
        // 1 Dict변수.Add
        public static string PMTotal = @"
using Google.FlatBuffers;
using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{{
    Dictionary<ushort, Action<PacketSession, ByteBuffer>> _handler = new Dictionary<ushort, Action<PacketSession, ByteBuffer>>();
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

        Action<PacketSession, ByteBuffer> action = null;
        if (_handler.TryGetValue(id, out action))
        {{
            byte[] data = new byte[size - count];
            Array.Copy(buffer.Array, buffer.Offset + count, data, 0, size - count);
            ByteBuffer bb = new ByteBuffer(data);
            action.Invoke(session, bb);
        }}
    }}
    ushort PacketHederSize = 4;
    public byte[] CreatePacket<T>(Offset<T> data, FlatBufferBuilder builder, PKT_Type id) where T : struct
    {{
        builder.Finish(data.Value);
        var bytes = builder.SizedByteArray();
        ushort size = (ushort)(bytes.Length + PacketHederSize);
        ArraySegment<byte> segment = new ArraySegment<byte>(new byte[size]);

        bool success = true;
        success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, 0, 2), size);
        success &= BitConverter.TryWriteBytes(new Span<byte>(segment.Array, 2, 4), (ushort)id);
        Buffer.BlockCopy(bytes, 0, segment.Array, 4, bytes.Length);

        if (success == false)
            return null;
        return segment.Array;
    }}
}}
";
        // 0 패킷 테이블 이름
        public static string PMRegister = @"_handler.Add((ushort)PKT_Type.{0}, PKT_Handler.{0}Handler);";
    }
}