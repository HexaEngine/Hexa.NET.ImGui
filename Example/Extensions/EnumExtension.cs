namespace HexaEngine.Core.Extensions
{
    using System.Runtime.CompilerServices;

    public static class EnumExtension
    {
        // size-specific version
        public static TInt AsInteger<TEnum, TInt>(this TEnum enumValue)
            where TEnum : unmanaged, Enum
            where TInt : unmanaged
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<TInt>()) throw new Exception("type mismatch");
            TInt value = Unsafe.As<TEnum, TInt>(ref enumValue);
            return value;
        }

        // long version
        public static long AsInteger<TEnum>(this TEnum enumValue)
            where TEnum : unmanaged, Enum
        {
            long value;
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<byte>()) value = Unsafe.As<TEnum, byte>(ref enumValue);
            else if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<short>()) value = Unsafe.As<TEnum, short>(ref enumValue);
            else if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<int>()) value = Unsafe.As<TEnum, int>(ref enumValue);
            else if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<long>()) value = Unsafe.As<TEnum, long>(ref enumValue);
            else throw new Exception("type mismatch");
            return value;
        }
    }
}