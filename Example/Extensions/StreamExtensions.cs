namespace HexaEngine.Core.IO
{
    using HexaEngine.Mathematics;
    using System;
    using System.Buffers;
    using System.Buffers.Binary;
    using System.IO;
    using System.Numerics;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class StreamExtensions
    {
        public static int WriteString(this Span<byte> dest, string str, Encoder encoder)
        {
            BinaryPrimitives.WriteInt32LittleEndian(dest, encoder.GetByteCount(str, true));
            return encoder.GetBytes(str, dest[4..], true) + 4;
        }

        public static int ReadString(this ReadOnlySpan<byte> src, out string str, Decoder decoder)
        {
            int len = BinaryPrimitives.ReadInt32LittleEndian(src);
            ReadOnlySpan<byte> bytes = src.Slice(4, len);
            int charCount = decoder.GetCharCount(bytes, true);
            Span<char> chars = charCount < 2048 ? stackalloc char[charCount] : new char[charCount];
            decoder.GetChars(bytes, chars, true);
            str = new(chars);
            return len + 4;
        }

        public static int SizeOf(this string str, Encoder encoder)
        {
            return 4 + encoder.GetByteCount(str, true);
        }

        public static int WriteInt32(this Span<byte> dest, int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(dest, value);
            return 4;
        }

        public static int ReadInt32(this ReadOnlySpan<byte> src, out int value)
        {
            value = BinaryPrimitives.ReadInt32LittleEndian(src);
            return 4;
        }

        public static void WriteString(this Stream stream, string? str, Encoding encoder, Endianness endianness)
        {
            if (str == null)
            {
                stream.WriteInt(0, endianness);
                return;
            }
            var count = encoder.GetByteCount(str);
            var bytes = count + 4;
            Span<byte> dst = bytes < 2048 ? stackalloc byte[bytes] : new byte[bytes];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(dst, count);
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(dst, count);
            }

            encoder.GetBytes(str, dst[4..]);
            stream.Write(dst);
        }

        public static string? ReadString(this Stream stream, Encoding decoder, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            stream.Read(buf);
            int len = 0;
            if (endianness == Endianness.LittleEndian)
            {
                len = BinaryPrimitives.ReadInt32LittleEndian(buf);
            }
            else
            {
                len = BinaryPrimitives.ReadInt32BigEndian(buf);
            }

            if (len == 0)
            {
                return null;
            }

            Span<byte> src = len < 2048 ? stackalloc byte[len] : new byte[len];
            stream.Read(src);

            int charCount = decoder.GetCharCount(src);
            Span<char> chars = charCount < 2048 ? stackalloc char[charCount] : new char[charCount];
            decoder.GetChars(src, chars);
            return new(chars);
        }

        public static void WriteInt(this Stream stream, int val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt32LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteInt32BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static int ReadInt(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            stream.Read(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadInt32LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadInt32BigEndian(buf);
            }
        }

        public static uint ReadUInt(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            stream.Read(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadUInt32LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadUInt32BigEndian(buf);
            }
        }

        public static void WriteUInt(this Stream stream, uint val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[4];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteUInt32BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static void WriteUInt64(this Stream stream, ulong val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteUInt64LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteUInt64BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static void WriteInt64(this Stream stream, long val, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteInt64LittleEndian(buf, val);
            }
            else
            {
                BinaryPrimitives.WriteInt64BigEndian(buf, val);
            }

            stream.Write(buf);
        }

        public static long ReadInt64(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            stream.Read(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadInt64LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadInt64BigEndian(buf);
            }
        }

        public static ulong ReadUInt64(this Stream stream, Endianness endianness)
        {
            Span<byte> buf = stackalloc byte[8];
            stream.Read(buf);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadUInt64LittleEndian(buf);
            }
            else
            {
                return BinaryPrimitives.ReadUInt64BigEndian(buf);
            }
        }

        public static byte[] Read(this Stream stream, long length)
        {
            var buffer = new byte[length];
            stream.Read(buffer, 0, (int)length);
            return buffer;
        }

        public static bool Compare(this Stream stream, byte[] compare)
        {
#nullable disable
            bool pool = compare.Length > 2048;
            byte[] array = null;
            Span<byte> buffer = pool ? (Span<byte>)(array = ArrayPool<byte>.Shared.Rent(compare.Length)) : (stackalloc byte[compare.Length]);
            stream.Read(buffer);
            var result = buffer.SequenceEqual(compare);
            if (pool)
            {
                ArrayPool<byte>.Shared.Return(array);
            }

            return result;
#nullable enable
        }

        public static bool Compare(this Stream stream, ulong value, Endianness endianness)
        {
            return stream.ReadUInt64(endianness) == value;
        }

        public static bool Compare(this ReadOnlySpan<byte> src, ulong value)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(src) == value;
        }

        public static string ReadString(this ReadOnlySpan<byte> src, Encoding encoding, out int read)
        {
            var len = BinaryPrimitives.ReadInt32LittleEndian(src);
            read = len + 4;
            return encoding.GetString(src.Slice(4, len));
        }

        public static int WriteString(this Span<byte> dest, ReadOnlySpan<char> src, Encoding encoding)
        {
            var len = encoding.GetByteCount(src);
            BinaryPrimitives.WriteInt32LittleEndian(dest, len);
            encoding.GetBytes(src, dest[4..]);
            return 4 + len;
        }

        public static byte[] ReadBytes(this Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return bytes;
        }

        public static byte[] ReadBytes(this Stream stream, uint length)
        {
            byte[] bytes = new byte[length];
            stream.Read(bytes);
            return bytes;
        }

        public static unsafe T ReadStruct<T>(this Stream stream) where T : unmanaged
        {
#nullable disable
            var byteLength = Marshal.SizeOf(typeof(T));
            var bytes = stream.ReadBytes(byteLength);
            var pinned = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var stt = (T)Marshal.PtrToStructure(
                pinned.AddrOfPinnedObject(),
                typeof(T));
            pinned.Free();
            return stt;
#nullable enable
        }

        public static unsafe void WriteStruct<T>(this Stream stream, T t) where T : unmanaged
        {
#nullable disable
            var sizeOfT = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(sizeOfT);
            Marshal.StructureToPtr(t, ptr, false);
            var bytes = new byte[sizeOfT];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);
            Marshal.FreeHGlobal(ptr);
            stream.Write(bytes);
#nullable enable
        }

        public static void WriteVector4(this Stream stream, Vector4 vector, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[16];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteSingleLittleEndian(dst, vector.X);
                BinaryPrimitives.WriteSingleLittleEndian(dst[4..], vector.Y);
                BinaryPrimitives.WriteSingleLittleEndian(dst[8..], vector.Z);
                BinaryPrimitives.WriteSingleLittleEndian(dst[12..], vector.W);
            }
            else
            {
                BinaryPrimitives.WriteSingleBigEndian(dst, vector.X);
                BinaryPrimitives.WriteSingleBigEndian(dst[4..], vector.Y);
                BinaryPrimitives.WriteSingleBigEndian(dst[8..], vector.Z);
                BinaryPrimitives.WriteSingleBigEndian(dst[12..], vector.W);
            }

            stream.Write(dst);
        }

        public static Vector4 ReadVector4(this Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[16];
            stream.Read(src);
            Vector4 vector;
            if (endianness == Endianness.LittleEndian)
            {
                vector.X = BinaryPrimitives.ReadSingleLittleEndian(src);
                vector.Y = BinaryPrimitives.ReadSingleLittleEndian(src[4..]);
                vector.Z = BinaryPrimitives.ReadSingleLittleEndian(src[8..]);
                vector.W = BinaryPrimitives.ReadSingleLittleEndian(src[12..]);
            }
            else
            {
                vector.X = BinaryPrimitives.ReadSingleBigEndian(src);
                vector.Y = BinaryPrimitives.ReadSingleBigEndian(src[4..]);
                vector.Z = BinaryPrimitives.ReadSingleBigEndian(src[8..]);
                vector.W = BinaryPrimitives.ReadSingleBigEndian(src[12..]);
            }

            return vector;
        }

        public static void WriteVector3(this Stream stream, Vector3 vector, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[12];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteSingleLittleEndian(dst, vector.X);
                BinaryPrimitives.WriteSingleLittleEndian(dst[4..], vector.Y);
                BinaryPrimitives.WriteSingleLittleEndian(dst[8..], vector.Z);
            }
            else
            {
                BinaryPrimitives.WriteSingleBigEndian(dst, vector.X);
                BinaryPrimitives.WriteSingleBigEndian(dst[4..], vector.Y);
                BinaryPrimitives.WriteSingleBigEndian(dst[8..], vector.Z);
            }

            stream.Write(dst);
        }

        public static Vector3 ReadVector3(this Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[12];
            stream.Read(src);
            Vector3 vector;
            if (endianness == Endianness.LittleEndian)
            {
                vector.X = BinaryPrimitives.ReadSingleLittleEndian(src);
                vector.Y = BinaryPrimitives.ReadSingleLittleEndian(src[4..]);
                vector.Z = BinaryPrimitives.ReadSingleLittleEndian(src[8..]);
            }
            else
            {
                vector.X = BinaryPrimitives.ReadSingleBigEndian(src);
                vector.Y = BinaryPrimitives.ReadSingleBigEndian(src[4..]);
                vector.Z = BinaryPrimitives.ReadSingleBigEndian(src[8..]);
            }

            return vector;
        }

        public static void WriteVector2(this Stream stream, Vector2 vector, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[8];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteSingleLittleEndian(dst, vector.X);
                BinaryPrimitives.WriteSingleLittleEndian(dst[4..], vector.Y);
            }
            else
            {
                BinaryPrimitives.WriteSingleBigEndian(dst, vector.X);
                BinaryPrimitives.WriteSingleBigEndian(dst[4..], vector.Y);
            }

            stream.Write(dst);
        }

        public static Vector2 ReadVector2(this Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[8];
            stream.Read(src);
            Vector2 vector;
            if (endianness == Endianness.LittleEndian)
            {
                vector.X = BinaryPrimitives.ReadSingleLittleEndian(src);
                vector.Y = BinaryPrimitives.ReadSingleLittleEndian(src[4..]);
            }
            else
            {
                vector.X = BinaryPrimitives.ReadSingleBigEndian(src);
                vector.Y = BinaryPrimitives.ReadSingleBigEndian(src[4..]);
            }

            return vector;
        }

        public static void WriteFloat(this Stream stream, float value, Endianness endianness)
        {
            Span<byte> dst = stackalloc byte[4];
            if (endianness == Endianness.LittleEndian)
            {
                BinaryPrimitives.WriteSingleLittleEndian(dst, value);
            }
            else
            {
                BinaryPrimitives.WriteSingleBigEndian(dst, value);
            }

            stream.Write(dst);
        }

        public static float ReadFloat(this Stream stream, Endianness endianness)
        {
            Span<byte> src = stackalloc byte[4];
            stream.Read(src);
            if (endianness == Endianness.LittleEndian)
            {
                return BinaryPrimitives.ReadSingleLittleEndian(src);
            }
            else
            {
                return BinaryPrimitives.ReadSingleBigEndian(src);
            }
        }
    }
}