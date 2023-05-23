namespace HexaEngine.Mathematics
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    public static class SphereHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetSphericalCoordinates(Vector3 cartesian)
        {
            float r = MathF.Sqrt(
                MathF.Pow(cartesian.X, 2) +
                MathF.Pow(cartesian.Y, 2) +
                MathF.Pow(cartesian.Z, 2)
            );

            // use atan2 for built-in checks
            float phi = MathF.Atan2(cartesian.Z / cartesian.X, cartesian.X);
            float theta = MathF.Acos(cartesian.Y / r);

            return new Vector3(r, phi, theta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetCartesianCoordinates(Vector3 spherical)
        {
            Vector3 ret = new();

            ret.Z = -(spherical.X * MathF.Cos(spherical.Z) * MathF.Cos(spherical.Y));
            ret.Y = spherical.X * MathF.Sin(spherical.Z);
            ret.X = spherical.X * MathF.Cos(spherical.Z) * MathF.Sin(spherical.Y);

            return ret;
        }
    }
}