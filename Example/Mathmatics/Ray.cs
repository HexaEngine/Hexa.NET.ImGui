namespace HexaEngine.Mathematics
{
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines a ray.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public readonly struct Ray : IEquatable<Ray>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ray"/> struct.
        /// </summary>
        /// <param name="position">The position in three dimensional space of the origin of the ray.</param>
        /// <param name="direction">The normalized direction of the ray.</param>
        public Ray(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }

        /// <summary>
        /// The position in three dimensional space where the ray starts.
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// The normalized direction in which the ray points.
        /// </summary>
        public Vector3 Direction { get; }

        /// <summary>
        /// Checks whether the current <see cref="Ray"/> intersects with a specified <see cref="Vector3"/>.
        /// </summary>
        /// <param name="point">Point to test ray intersection</param>
        /// <returns></returns>
        public bool Intersects(in Vector3 point)
        {
            //Source: RayIntersectsSphere
            //Reference: None

            Vector3 m = Vector3.Subtract(Position, point);

            //Same thing as RayIntersectsSphere except that the radius of the sphere (point)
            //is the epsilon for zero.
            float b = Vector3.Dot(m, Direction);
            float c = Vector3.Dot(m, m) - float.Epsilon;

            if (c > 0f && b > 0f)
            {
                return false;
            }

            float discriminant = b * b - c;

            if (discriminant < 0f)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the current <see cref="Ray"/> intersects with a specified <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to check for intersection with the current <see cref="Ray"/>.</param>
        /// <returns>Distance value if intersects, null otherwise.</returns>
        public float? Intersects(in BoundingSphere sphere) => sphere.Intersects(this);

        /// <summary>
        /// Checks whether the current <see cref="Ray"/> intersects with a specified <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to check for intersection with the current <see cref="Ray"/>.</param>
        /// <param name="result">Distance of normalised vector to intersection if >= 0 </param>
        /// <returns>bool returns true if intersection with plane</returns>
        public bool Intersects(in BoundingBox box, out float result)
        {
            float? rs = box.Intersects(this);

            result = rs == null ? -1 : (float)rs;

            return result >= 0;
        }

        /// <summary>
        /// Checks whether the current <see cref="Ray"/> intersects with a specified <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to check for intersection with the current <see cref="Ray"/>.</param>
        /// <returns>Distance value if intersects, null otherwise.</returns>
        public float? Intersects(in BoundingBox box) => box.Intersects(this);

        /// <summary>
        /// Checks whether the current <see cref="Ray"/> intersects with a specified <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to check for intersection with the current <see cref="Ray"/>.</param>
        /// <returns>Distance value if intersects, null otherwise.</returns>
        public float? Intersects(in Plane plane)
        {
            //Source: Real-Time Collision Detection by Christer Ericson
            //Reference: Page 175

            float direction = Vector3.Dot(plane.Normal, Direction);

            if (Math.Abs(direction) < float.Epsilon)
            {
                return null;
            }

            float position = Vector3.Dot(plane.Normal, Position);
            float distance = (-plane.D - position) / direction;

            if (distance < 0f)
            {
                if (distance < -float.Epsilon)
                {
                    return null;
                }

                distance = 0f;
            }

            return distance;
        }

        /// <summary>
        /// Checks whether the current <see cref="Ray"/> intersects with a specified <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to check for intersection with the current <see cref="Ray"/>.</param>
        /// <param name="result">Distance of normalised vector to intersection if >= 0 </param>
        /// <returns>bool returns true if intersection with plane</returns>
        public bool Intersects(in Plane plane, out float result)
        {
            float? rs = Intersects(plane);

            result = rs == null ? -1 : (float)rs;

            return result >= 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Ray value && Equals(value);

        /// <summary>
        /// Determines whether the specified <see cref="Ray"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Int4"/> to compare with this instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ray other)
        {
            return Position.Equals(other.Position)
                && Direction.Equals(other.Direction);
        }

        /// <summary>
        /// Compares two <see cref="Ray"/> objects for equality.
        /// </summary>
        /// <param name="left">The <see cref="Ray"/> on the left hand of the operand.</param>
        /// <param name="right">The <see cref="Ray"/> on the right hand of the operand.</param>
        /// <returns>
        /// True if the current left is equal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ray left, Ray right) => left.Equals(right);

        /// <summary>
        /// This does a ray cast on a triangle to see if there is an intersection.
        /// This ONLY works on CW wound triangles.
        /// </summary>
        /// <param name="v0">Triangle Corner 1</param>
        /// <param name="v1">Triangle Corner 2</param>
        /// <param name="v2">Triangle Corner 3</param>
        /// <param name="pointInTriangle">Intersection point if boolean returns true</param>
        /// <returns></returns>
        public bool Intersects(in Vector3 v0, in Vector3 v1, in Vector3 v2, out Vector3 pointInTriangle)
        {
            // Code origin can no longer be determined.
            // was adapted from C++ code.

            pointInTriangle = Vector3.Zero;

            // compute normal
            Vector3 edgeA = v1 - v0;
            Vector3 edgeB = v2 - v0;

            Vector3 normal = Vector3.Cross(Direction, edgeB);

            // find determinant
            float det = Vector3.Dot(edgeA, normal);

            // if perpendicular, exit
            if (det < float.Epsilon)
            {
                return false;
            }
            det = 1.0f / det;

            // calculate distance from vertex0 to ray origin
            Vector3 s = Position - v0;
            float u = det * Vector3.Dot(s, normal);

            if (u < -float.Epsilon || u > 1.0f + float.Epsilon)
            {
                return false;
            }

            Vector3 r = Vector3.Cross(s, edgeA);
            float v = det * Vector3.Dot(Direction, r);
            if (v < -float.Epsilon || u + v > 1.0f + float.Epsilon)
            {
                return false;
            }

            // distance from ray to triangle
            det *= Vector3.Dot(edgeB, r);

            // Vector3 endPosition;
            // we dont want the point that is behind the ray cast.
            if (det < 0.0f)
            {
                return false;
            }

            pointInTriangle.X = Position.X + Direction.X * det;
            pointInTriangle.Y = Position.Y + Direction.Y * det;
            pointInTriangle.Z = Position.Z + Direction.Z * det;

            return true;
        }

        /// <summary>
        /// This does a ray cast on a triangle to see if there is an intersection.
        /// This ONLY works on CW wound triangles.
        /// </summary>
        /// <param name="v0">Triangle Corner 1</param>
        /// <param name="v1">Triangle Corner 2</param>
        /// <param name="v2">Triangle Corner 3</param>
        /// <param name="pointInTriangle">Intersection point if boolean returns true</param>
        /// <returns></returns>
        public bool Intersects2(in Vector3 v0, in Vector3 v1, in Vector3 v2, out Vector3 pointInTriangle)
        {
            pointInTriangle = default;
            const float EPSILON = 0.0000001f;

            Vector3 edge1, edge2, h, s, q;
            float a, f, u, v;
            edge1 = v1 - v0;
            edge2 = v2 - v0;
            h = Vector3.Cross(Direction, edge2);
            a = Vector3.Dot(edge1, h);

            if (a > -EPSILON && a < EPSILON)
            {
                return false;    // This ray is parallel to this triangle.
            }

            f = 1.0f / a;
            s = Position - v0;
            u = f * Vector3.Dot(s, h);

            if (u < 0.0 || u > 1.0)
            {
                return false;
            }

            q = Vector3.Cross(s, edge1);
            v = f * Vector3.Dot(Direction, q);

            if (v < 0.0 || u + v > 1.0)
            {
                return false;
            }

            // At this stage we can compute t to find out where the intersection point is on the line.
            float t = f * Vector3.Dot(edge2, q);

            if (t > EPSILON) // ray intersection
            {
                pointInTriangle = Position + Direction * t;
                return true;
            }
            else // This means that there is a line intersection but not a ray intersection.
            {
                return false;
            }
        }

        /// <summary>
        /// Compares two <see cref="Ray"/> objects for inequality.
        /// </summary>
        /// <param name="left">The <see cref="Ray"/> on the left hand of the operand.</param>
        /// <param name="right">The <see cref="Ray"/> on the right hand of the operand.</param>
        /// <returns>
        /// True if the current left is unequal to the <paramref name="right"/> parameter; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ray left, Ray right) => !left.Equals(right);

        /// <inheritdoc/>
		public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Position);
                hashCode.Add(Direction);
            }
            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return $"Position:{Position.ToString(format, formatProvider)} Direction:{Direction.ToString(format, formatProvider)}";
        }
    }
}