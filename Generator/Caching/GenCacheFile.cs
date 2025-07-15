namespace Generator.Caching
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;

    public enum CacheFileVersion : uint
    {
        Version1 = 1 << 24 | 0 << 16 | 0 << 8 | 0
    }

    public struct CacheFileHeader
    {
        public static readonly byte[] Magic = [0x48, 0x65, 0x78, 0x61, 0x47, 0x65, 0x6e, 0x43, 0x61, 0x63, 0x68, 0x65, 0x46, 0x69, 0x6c, 0x65, 0x00];
        public CacheFileVersion Version;
        public uint Crc32;
    }

    public struct Hash256 : IEquatable<Hash256>
    {
        public ulong Value0;
        public ulong Value1;
        public ulong Value2;
        public ulong Value3;

        public Span<byte> AsSpan() => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref Value0, 4));

        public static implicit operator Span<byte>(in Hash256 hash) => hash.AsSpan();

        public override readonly bool Equals(object? obj)
        {
            return obj is Hash256 hash && Equals(hash);
        }

        public readonly bool Equals(Hash256 other)
        {
            return Value0 == other.Value0 &&
                   Value1 == other.Value1 &&
                   Value2 == other.Value2 &&
                   Value3 == other.Value3;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Value0, Value1, Value2, Value3);
        }

        public static bool operator ==(Hash256 left, Hash256 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Hash256 left, Hash256 right)
        {
            return !(left == right);
        }

        public static Hash256 HashData(Stream stream)
        {
            Hash256 hash = default;
            SHA256.HashData(stream, hash);
            return hash;
        }
    }

    [InlineArray(MaxFileNameSize)]
    public struct CacheFileName : IEquatable<CacheFileName>, IEquatable<string>
    {
        public const int MaxFileNameSize = 256;
        byte byte0;

        public CacheFileName(string str)
        {
            if (Encoding.UTF8.GetByteCount(str) > MaxFileNameSize - 1) throw new ArgumentException("The file name is too long.", nameof(str));
            var span = AsSpan();
            int idx = Encoding.UTF8.GetBytes(str, span);
            span[Math.Min(idx, MaxFileNameSize - 1)] = 0;
        }

        public Span<byte> AsSpan() => MemoryMarshal.CreateSpan(ref byte0, MaxFileNameSize);

        public override bool Equals(object? obj)
        {
            return obj is CacheFileName name && Equals(name);
        }

        public bool Equals(CacheFileName other)
        {
            return AsSpan().SequenceEqual(other.AsSpan());
        }

        public bool Equals(string? other)
        {
            if (other is null) return false;
            Span<byte> buffer = stackalloc byte[MaxFileNameSize];
            int idx = Encoding.UTF8.GetBytes(other, buffer);
            buffer[Math.Min(idx, MaxFileNameSize - 1)] = 0;
            return AsSpan().SequenceEqual(buffer);
        }

        public override int GetHashCode() => (int)MurmurHash3.Hash32(AsSpan());

        public static bool operator ==(CacheFileName left, CacheFileName right) => left.Equals(right);

        public static bool operator !=(CacheFileName left, CacheFileName right) => !(left == right);

        public static bool operator ==(CacheFileName left, string right) => left.Equals(right);

        public static bool operator !=(CacheFileName left, string right) => !(left == right);

        public override unsafe string ToString()
        {
            fixed (byte* p = AsSpan())
            {
                return MemoryMarshal.CreateReadOnlySpanFromNullTerminated(p).ToString();
            }
        }
    }

    public struct CacheFileEntry
    {
        public uint Parent;
        public uint Next;
        public CacheFileName Name;
        public Hash256 Hash;
    }

    public class GenCacheFile
    {
    }
}