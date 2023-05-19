namespace HexaEngine.Core.Extensions
{
    using System.Globalization;
    using System.Numerics;

    public static class FormatExtensions
    {
        public static string ToHLSL(this bool scalar)
        {
            return scalar ? "1" : "0";
        }

        public static string ToHLSL(this float scalar)
        {
            return scalar.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToHLSL(this Vector2 vector)
        {
            return $"float2({vector.X.ToString(CultureInfo.InvariantCulture)}, {vector.Y.ToString(CultureInfo.InvariantCulture)})";
        }

        public static string ToHLSL(this Vector3 vector)
        {
            return $"float3({vector.X.ToString(CultureInfo.InvariantCulture)}, {vector.Y.ToString(CultureInfo.InvariantCulture)}, {vector.Z.ToString(CultureInfo.InvariantCulture)})";
        }

        public static string ToHLSL(this Vector4 vector)
        {
            return $"float4({vector.X.ToString(CultureInfo.InvariantCulture)}, {vector.Y.ToString(CultureInfo.InvariantCulture)}, {vector.Z.ToString(CultureInfo.InvariantCulture)}, {vector.W.ToString(CultureInfo.InvariantCulture)})";
        }
    }
}