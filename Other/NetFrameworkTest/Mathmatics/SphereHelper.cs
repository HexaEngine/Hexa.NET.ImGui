namespace ExampleD3D11.Mathmatics
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    public static class SphereHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetSphericalCoordinates(Vector3 cartesian)
        {
            float r = (float)Math.Sqrt(
                (float)Math.Pow(cartesian.X, 2) +
                (float)Math.Pow(cartesian.Y, 2) +
                (float)Math.Pow(cartesian.Z, 2)
            );

            // use atan2 for built-in checks
            float phi = (float)Math.Atan2(cartesian.Z / cartesian.X, cartesian.X);
            float theta = (float)Math.Acos(cartesian.Y / r);

            return new Vector3(r, phi, theta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetCartesianCoordinates(Vector3 spherical)
        {
            Vector3 ret = new();

            ret.Z = -(spherical.X * (float)Math.Cos(spherical.Z) * (float)Math.Cos(spherical.Y));
            ret.Y = spherical.X * (float)Math.Sin(spherical.Z);
            ret.X = spherical.X * (float)Math.Cos(spherical.Z) * (float)Math.Sin(spherical.Y);

            return ret;
        }
    }
}