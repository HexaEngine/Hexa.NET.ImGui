namespace Generator
{
    using System;

    [Flags]
    public enum NumberParseOptions
    {
        None = 0,
        AllowBrackets = 1,
        AllowHex = 2,
        AllowMinus = 4,
        AllowExponent = 8,
        AllowSuffix = 16,

        All = AllowBrackets | AllowHex | AllowMinus | AllowExponent | AllowSuffix,
    }
}