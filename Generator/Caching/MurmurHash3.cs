namespace Generator.Caching
{
    using System;

    public static class MurmurHash3
    {
        public static uint Hash32(ReadOnlySpan<byte> data, uint seed = 0)
        {
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            uint length = (uint)data.Length;
            uint h1 = seed;
            int currentIndex = 0;

            while (length >= 4)
            {
                uint k1 = (uint)(data[currentIndex]
                    | data[currentIndex + 1] << 8
                    | data[currentIndex + 2] << 16
                    | data[currentIndex + 3] << 24);

                k1 *= c1;
                k1 = RotateLeft(k1, 15);
                k1 *= c2;

                h1 ^= k1;
                h1 = RotateLeft(h1, 13);
                h1 = h1 * 5 + 0xe6546b64;

                currentIndex += 4;
                length -= 4;
            }

            uint k2 = 0;

            switch (length)
            {
                case 3:
                    k2 ^= (uint)data[currentIndex + 2] << 16;
                    goto case 2;
                case 2:
                    k2 ^= (uint)data[currentIndex + 1] << 8;
                    goto case 1;
                case 1:
                    k2 ^= data[currentIndex];
                    k2 *= c1;
                    k2 = RotateLeft(k2, 15);
                    k2 *= c2;
                    h1 ^= k2;
                    break;
            }

            h1 ^= (uint)data.Length;
            h1 = FMix(h1);

            return h1;
        }

        private static uint RotateLeft(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static uint FMix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}