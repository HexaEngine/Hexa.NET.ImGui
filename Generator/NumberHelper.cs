namespace Generator
{
    public static class StringHelper
    {
        public static bool IsString(this string name)
        {
            return name.StartsWith("\"") && name.EndsWith("\"");
        }
    }

    public static class NumberHelper
    {
        public static bool IsNumeric(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsNumber(name[i]))
                    return false;
            }
            return true;
        }

        public static bool IsNumeric(this string name, out NumberType numberType, NumberParseOptions options = NumberParseOptions.All)
        {
            numberType = NumberType.None;
            if (string.IsNullOrEmpty(name))
                return false;

            int index = 0;
            int length = name.Length;
            bool isHex = false;
            bool isMinus = false;
            bool typeOverwrite = false;

            numberType = NumberType.Int;

            bool allowBrackets = options.HasFlag(NumberParseOptions.AllowBrackets);
            bool allowMinus = options.HasFlag(NumberParseOptions.AllowMinus);
            bool allowHex = options.HasFlag(NumberParseOptions.AllowHex);
            bool allowSuffix = options.HasFlag(NumberParseOptions.AllowSuffix);
            bool allowExponent = options.HasFlag(NumberParseOptions.AllowExponent);

            if (allowBrackets && name.StartsWith("(") && name.EndsWith(")"))
            {
                index += 1;
                length -= 1;
            }

            if (allowMinus && name[index..length].StartsWith('-'))
            {
                numberType = NumberType.Int;
                index += 1;
                isMinus = true;
            }

            if (allowHex && name[index..length].StartsWith("0x"))
            {
                index += 2;
                isHex = true;
            }

            if (allowSuffix && name[index..length].EndsWith("UL", StringComparison.InvariantCultureIgnoreCase))
            {
                numberType = NumberType.ULong;
                length -= 2;
                typeOverwrite = true;
            }

            if (allowSuffix && name[index..length].EndsWith("L", StringComparison.InvariantCultureIgnoreCase))
            {
                numberType = NumberType.Long;
                length -= 1;
                typeOverwrite = true;
            }

            if (allowSuffix && name[index..length].EndsWith("U", StringComparison.InvariantCultureIgnoreCase))
            {
                numberType = NumberType.UInt;
                length -= 1;
                typeOverwrite = true;
            }

            if (allowSuffix && !isHex && name[index..length].EndsWith("F", StringComparison.InvariantCultureIgnoreCase))
            {
                numberType = NumberType.Float;
                length -= 1;
                typeOverwrite = true;
            }

            if (allowSuffix && !isHex && name[index..length].EndsWith("D", StringComparison.InvariantCultureIgnoreCase))
            {
                numberType = NumberType.Double;
                length -= 1;
                typeOverwrite = true;
            }

            if (allowSuffix && name[index..length].EndsWith("M", StringComparison.InvariantCultureIgnoreCase))
            {
                numberType = NumberType.Decimal;
                length -= 1;
                typeOverwrite = true;
            }

            if (allowExponent && name.Contains("E-", StringComparison.InvariantCultureIgnoreCase))
            {
                var idx = name.IndexOf("E-", StringComparison.InvariantCultureIgnoreCase);
                for (int i = idx + 2; i < length; i++)
                {
                    if (!char.IsDigit(name[i]))
                    {
                        return false;
                    }
                }
                length = idx;
            }

            if (allowExponent && name.Contains("E+", StringComparison.InvariantCultureIgnoreCase))
            {
                var idx = name.IndexOf("E+", StringComparison.InvariantCultureIgnoreCase);
                for (int i = idx + 2; i < length; i++)
                {
                    if (!char.IsDigit(name[i]))
                    {
                        return false;
                    }
                }
                length = idx;
            }

            for (int i = index; i < length; i++)
            {
                var c = name[i];
                if (!char.IsNumber(c))
                {
                    if (c == '.')
                    {
                        if ((numberType & NumberType.AnyInt) != 0)
                        {
                            numberType = NumberType.Double;
                        }
                    }
                    else if (!isHex || !char.IsAsciiHexDigit(c))
                    {
                        return false;
                    }
                }
            }

            if (typeOverwrite)
            {
                return true;
            }

            if ((numberType & NumberType.AnyInt) != 0 && !isHex)
            {
                var span = name.AsSpan(index, length - index).TrimStart('0');
                if (NumberStringCompareLessEquals(span, int.MaxValue.ToString()))
                {
                    numberType = NumberType.Int;
                    return true;
                }
                if (!isMinus && NumberStringCompareLessEquals(span, uint.MaxValue.ToString()))
                {
                    numberType = NumberType.UInt;
                    return true;
                }
                if (NumberStringCompareLessEquals(span, long.MaxValue.ToString()))
                {
                    numberType = NumberType.Long;
                    return true;
                }
                if (!isMinus && NumberStringCompareLessEquals(span, ulong.MaxValue.ToString()))
                {
                    numberType = NumberType.ULong;
                    return true;
                }

                throw new InvalidDataException($"The number {name} is outside known number ranges!");
            }
            if ((numberType & NumberType.AnyInt) != 0 && isHex)
            {
                var span = name.AsSpan(index, length - index).TrimStart('0');
                if (NumberStringCompareLessEquals(span, "7fffffff"))
                {
                    numberType = NumberType.Int;
                    return true;
                }
                if (!isMinus && NumberStringCompareLessEquals(span, "ffffffff"))
                {
                    numberType = NumberType.UInt;
                    return true;
                }
                if (NumberStringCompareLessEquals(span, "7fffffffffffffff"))
                {
                    numberType = NumberType.Long;
                    return true;
                }
                if (!isMinus && NumberStringCompareLessEquals(span, "ffffffffffffffff"))
                {
                    numberType = NumberType.ULong;
                    return true;
                }

                throw new InvalidDataException($"The number {name} is outside known number ranges!");
            }

            return true;
        }

        public static bool IsNumeric(this string name, NumberParseOptions options = NumberParseOptions.All)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            int index = 0;
            int length = name.Length;
            bool isHex = false;

            bool allowBrackets = options.HasFlag(NumberParseOptions.AllowBrackets);
            bool allowMinus = options.HasFlag(NumberParseOptions.AllowMinus);
            bool allowHex = options.HasFlag(NumberParseOptions.AllowHex);
            bool allowSuffix = options.HasFlag(NumberParseOptions.AllowSuffix);
            bool allowExponent = options.HasFlag(NumberParseOptions.AllowExponent);

            if (allowBrackets && name.StartsWith("(") && name.EndsWith(")"))
            {
                index += 1;
                length -= 1;
            }

            if (allowHex && name.StartsWith("0x"))
            {
                index = 2;
                isHex = true;
            }

            if (allowMinus && name.StartsWith('-'))
            {
                index = 1;
            }

            if (allowSuffix && name.EndsWith("L", StringComparison.InvariantCultureIgnoreCase))
            {
                length = name.Length - 1;
            }

            if (allowSuffix && name.EndsWith("U", StringComparison.InvariantCultureIgnoreCase))
            {
                length = name.Length - 1;
            }

            if (allowSuffix && name.EndsWith("UL", StringComparison.InvariantCultureIgnoreCase))
            {
                length = name.Length - 2;
            }

            if (allowSuffix && name.EndsWith("F", StringComparison.InvariantCultureIgnoreCase))
            {
                length = name.Length - 1;
            }

            if (allowSuffix && name.EndsWith("D", StringComparison.InvariantCultureIgnoreCase))
            {
                length = name.Length - 1;
            }

            if (allowSuffix && name.EndsWith("M", StringComparison.InvariantCultureIgnoreCase))
            {
                length = name.Length - 1;
            }

            if (allowExponent && name.Contains("E-", StringComparison.InvariantCultureIgnoreCase))
            {
                var idx = name.IndexOf("E-", StringComparison.InvariantCultureIgnoreCase);
                for (int i = idx + 2; i < length; i++)
                {
                    if (!char.IsDigit(name[i]))
                    {
                        return false;
                    }
                }
                length = idx;
            }

            if (allowExponent && name.Contains("E+", StringComparison.InvariantCultureIgnoreCase))
            {
                var idx = name.IndexOf("E+", StringComparison.InvariantCultureIgnoreCase);
                for (int i = idx + 2; i < length; i++)
                {
                    if (!char.IsDigit(name[i]))
                    {
                        return false;
                    }
                }
                length = idx;
            }

            for (int i = index; i < length; i++)
            {
                var c = name[i];
                if ((!isHex || !char.IsAsciiHexDigit(c)) && !char.IsNumber(c) && c != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public static bool NumberStringCompareLess(ReadOnlySpan<char> value, string max)
        {
            // Greater
            if (value.Length > max.Length)
                return false;

            // Less
            if (value.Length < max.Length)
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                var c = char.ToLower(value[i]);
                var cmp = char.ToLower(max[i]);

                // Greater
                if (cmp < c)
                {
                    return false;
                }

                // Less
                if (cmp > c)
                {
                    return true;
                }
            }

            // Equals
            return false;
        }

        public static bool NumberStringCompareLessEquals(ReadOnlySpan<char> value, string max)
        {
            // Greater
            if (value.Length > max.Length)
                return false;

            // Less
            if (value.Length < max.Length)
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                var c = char.ToLower(value[i]);
                var cmp = char.ToLower(max[i]);

                // Greater
                if (cmp < c)
                {
                    return false;
                }

                // Less
                if (cmp > c)
                {
                    return true;
                }
            }

            // Equals
            return true;
        }

        public static string GetNumberType(this NumberType number)
        {
            return number switch
            {
                NumberType.None => throw new InvalidOperationException(),
                NumberType.Int => "int",
                NumberType.Double => "double",
                NumberType.Float => "float",
                NumberType.Decimal => "decimal",
                NumberType.UInt => "uint",
                NumberType.Long => "long",
                NumberType.ULong => "ulong",
                _ => throw new InvalidOperationException(),
            };
        }
    }
}