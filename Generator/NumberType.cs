namespace Generator
{
    public enum NumberType : byte
    {
        None = 0,

        /// <summary>
        /// 1
        /// </summary>
        Int = 1,

        /// <summary>
        /// 1.0 or 1.0d
        /// </summary>
        Double = 2,

        /// <summary>
        /// 1.0f
        /// </summary>
        Float = 4,

        /// <summary>
        /// 1.0m
        /// </summary>
        Decimal = 8,

        /// <summary>
        /// 1u
        /// </summary>
        UInt = 16,

        /// <summary>
        /// 1l or 1L
        /// </summary>
        Long = 32,

        /// <summary>
        /// 1ul or 1UL
        /// </summary>
        ULong = 64,

        AnyInt = Int | UInt | Long | ULong,

        AnyFloat = Float | Double | Decimal,
    }
}