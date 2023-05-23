namespace HexaEngine.Mathematics
{
    using System.Numerics;

    public class CameraTransform : Transform
    {
        protected Matrix4x4 projection;
        protected Matrix4x4 projectionInv;
        protected Matrix4x4 viewProjection;
        protected ProjectionType projectionType;
        protected float width = 16;
        protected float height = 9;
        protected float aspectRatio;
        protected float fov = 90;
        protected float near = 0.01f;
        protected float far = 100f;
        protected BoundingFrustum frustum = new();

        public CameraTransform()
        {
            Recalculate();
        }

        public Matrix4x4 Projection { get => projection; }

        public Matrix4x4 ProjectionInv => projectionInv;

        public ProjectionType ProjectionType
        {
            get => projectionType;
            set
            {
                if (value == projectionType)
                {
                    return;
                }

                projectionType = value;
                dirty = true;
            }
        }

        public float Width
        {
            get => width;
            set
            {
                if (value == width)
                {
                    return;
                }

                width = value;
                dirty = true;
            }
        }

        public float Height
        {
            get => height;
            set
            {
                if (value == height)
                {
                    return;
                }

                height = value;
                dirty = true;
            }
        }

        public float AspectRatio => aspectRatio;

        public float Fov
        {
            get => fov;
            set
            {
                if (value == fov)
                {
                    return;
                }

                fov = value;
                if (projectionType == ProjectionType.Othro)
                {
                    return;
                }

                dirty = true;
            }
        }

        public float Near
        {
            get => near;
            set
            {
                if (value == near)
                {
                    return;
                }

                near = value;
                dirty = true;
            }
        }

        public float Far
        {
            get => far;
            set
            {
                if (value == far)
                {
                    return;
                }

                far = value;
                dirty = true;
            }
        }

        public BoundingFrustum Frustum => frustum;

        public Matrix4x4 ViewProjection => viewProjection;

        public override bool Recalculate()
        {
            if (!dirty)
            {
                return false;
            }

            base.Recalculate();
            aspectRatio = width / height;
            switch (projectionType)
            {
                case ProjectionType.Othro:
                    projection = MathUtil.OrthoLH(width, height, near, far);
                    break;

                case ProjectionType.Perspective:
                    projection = MathUtil.PerspectiveFovLH(fov.ToRad(), aspectRatio, near, far);
                    break;
            }
            Matrix4x4.Invert(projection, out projectionInv);

            OnUpdated();
            dirty = false;
            return true;
        }

        protected override void OnUpdated()
        {
            viewProjection = view * projection;
            frustum.Initialize(viewProjection);
            base.OnUpdated();
        }
    }
}