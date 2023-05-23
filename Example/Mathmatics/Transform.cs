namespace HexaEngine.Mathematics
{
    using System;
    using System.Numerics;
    using System.Text.Json.Serialization;

    public struct TransformSnapshot
    {
        public Transform? Parent;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public Quaternion Orientation;
        public Vector3 GlobalPosition;
        public Quaternion GlobalOrientation;
        public Vector3 GobalScale;
        public Vector3 Forward;
        public Vector3 Backward;
        public Vector3 Left;
        public Vector3 Right;
        public Vector3 Up;
        public Vector3 Down;
        public Matrix4x4 GlobalPrevious;
        public Matrix4x4 Global;
        public Matrix4x4 GlobalInverse;
        public Matrix4x4 Local;
        public Matrix4x4 LocalInverse;
        public Matrix4x4 ViewPrevious;
        public Matrix4x4 View;
        public Matrix4x4 ViewInv;
        public Vector3 Velocity;
        public Vector3 OldPos;
    }

    /// <summary>
    /// <see cref="Transform"/> is used for hierachical matrix calculation.
    /// </summary>
    public class Transform
    {
        protected TransformSnapshot initial;
        protected Transform? parent;
        protected Vector3 position;
        protected Vector3 rotation;
        protected Vector3 scale = Vector3.One;
        protected Quaternion orientation;
        protected Vector3 globalPosition;
        protected Quaternion globalOrientation;
        protected Vector3 globalScale;
        protected Vector3 forward;
        protected Vector3 backward;
        protected Vector3 left;
        protected Vector3 right;
        protected Vector3 up;
        protected Vector3 down;
        protected Matrix4x4 globalPrevious;
        protected Matrix4x4 global;
        protected Matrix4x4 globalInverse;
        protected Matrix4x4 local;
        protected Matrix4x4 localInverse;
        protected Matrix4x4 viewPrevious;
        protected Matrix4x4 view;
        protected Matrix4x4 viewInv;
        protected Vector3 velocity;
        protected Vector3 oldpos;
        protected bool dirty = true;

        public Transform()
        {
            Recalculate();
            SaveState();
        }

        [JsonIgnore]
        public Transform? Parent
        {
            get => parent;
            set
            {
                parent = value;
                if (parent != null)
                {
                    parent.Updated += ParentUpdated;
                }

                OnChanged();
            }
        }

        private void ParentUpdated(object? sender, EventArgs e)
        {
            dirty = true;
            Recalculate();
        }

        /// <summary>
        /// Gets or sets the local position.
        /// </summary>
        public Vector3 Position
        {
            get => position;
            set
            {
                if (position == value)
                {
                    return;
                }

                oldpos = position;
                position = value;
                velocity = position - oldpos;
                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the local rotation.
        /// </summary>
        /// <remarks>The rotation is in space euler from 0° to 360°(359°)</remarks>
        public Vector3 Rotation
        {
            get => rotation;
            set
            {
                if (rotation == value)
                {
                    return;
                }

                rotation = value;
                orientation = value.NormalizeEulerAngleDegrees().ToRad().GetQuaternion();
                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the local scale.
        /// </summary>
        public Vector3 Scale
        {
            get => scale;
            set
            {
                if (scale == value)
                {
                    return;
                }

                scale = value;
                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the local orienataion.
        /// </summary>
        [JsonIgnore]
        public Quaternion Orientation
        {
            get => orientation;
            set
            {
                if (orientation == value)
                {
                    return;
                }

                orientation = value;
                rotation = value.GetRotation().ToDeg().NormalizeEulerAngleDegrees();
                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the global (world space) position.
        /// </summary>
        [JsonIgnore]
        public Vector3 GlobalPosition
        {
            get => globalPosition;
            set
            {
                if (globalPosition == value)
                {
                    return;
                }

                if (parent == null)
                {
                    position = value;
                }
                else
                {
                    // Transform because the rotation could modify the position of the child.
                    position = Vector3.Transform(value, parent.globalInverse);
                }

                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the global (world space) orientation.
        /// </summary>
        [JsonIgnore]
        public Quaternion GlobalOrientation
        {
            get => globalOrientation;
            set
            {
                if (globalOrientation == value)
                {
                    return;
                }

                if (parent == null)
                {
                    orientation = value;
                }
                else
                {
                    // Divide because quaternions are like matrices.
                    orientation = value / parent.globalOrientation;
                }

                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the global (world space) scale.
        /// </summary>
        [JsonIgnore]
        public Vector3 GlobalScale
        {
            get => globalScale;
            set
            {
                if (globalScale == value)
                {
                    return;
                }

                if (parent == null)
                {
                    scale = value;
                }
                else
                {
                    // divide because scale is a factor.
                    scale = value / parent.globalScale;
                }

                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the position and orientation.
        /// </summary>
        [JsonIgnore]
        public (Vector3, Quaternion) PositionRotation
        {
            get => (position, orientation);
            set
            {
                if ((position, orientation) == value)
                {
                    return;
                }

                oldpos = position;
                (position, orientation) = value;
                velocity = position - oldpos;
                rotation = orientation.GetRotation().ToDeg().NormalizeEulerAngleDegrees();
                OnChanged();
            }
        }

        /// <summary>
        /// Gets or sets the local the position orientation and scale.
        /// </summary>
        [JsonIgnore]
        public (Vector3, Quaternion, Vector3) PositionRotationScale
        {
            get => (position, orientation, scale);
            set
            {
                if ((position, orientation, scale) == value)
                {
                    return;
                }

                oldpos = position;
                (position, orientation, scale) = value;
                velocity = position - oldpos;
                rotation = orientation.GetRotation().ToDeg().NormalizeEulerAngleDegrees();
                OnChanged();
            }
        }

        /// <summary>
        /// The forward vector in global orientation space
        /// </summary>
        [JsonIgnore]
        public Vector3 Forward => forward;

        /// <summary>
        /// The backward vector in global orientation space
        /// </summary>
        [JsonIgnore]
        public Vector3 Backward => backward;

        /// <summary>
        /// The left vector in global orientation space
        /// </summary>
        [JsonIgnore]
        public Vector3 Left => left;

        /// <summary>
        /// The right vector in global orientation space
        /// </summary>
        [JsonIgnore]
        public Vector3 Right => right;

        /// <summary>
        /// The up vector in global orientation space
        /// </summary>
        [JsonIgnore]
        public Vector3 Up => up;

        /// <summary>
        /// The down vector in global orientation space
        /// </summary>
        [JsonIgnore]
        public Vector3 Down => down;

        /// <summary>
        /// The global transformation matrix of the previous frame.
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 GlobalPrevious => globalPrevious;

        /// <summary>
        /// The global transformation matrix
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 Global { get => global; }

        /// <summary>
        /// The inverse global transformation matrix
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 GlobalInverse => globalInverse;

        /// <summary>
        /// The local transformation matrix
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 Local { get => local; set => SetMatrix(value); }

        /// <summary>
        /// The local inverse transformation matrix
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 LocalInverse => localInverse;

        /// <summary>
        /// The view matrix in world space of the last frame
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 ViewPrevious => viewPrevious;

        /// <summary>
        /// The view matrix in world space
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 View => view;

        /// <summary>
        /// The inverse view matrix in world space
        /// </summary>
        [JsonIgnore]
        public Matrix4x4 ViewInv => viewInv;

        /// <summary>
        /// The velocity of the object only useful for 3d sound.
        /// </summary>
        [JsonIgnore]
        public Vector3 Velocity => velocity;

        /// <summary>
        /// Notifies that the transform has changed. Used internally for parent child updates.
        /// </summary>
        public event EventHandler? Updated;

        public event EventHandler? Changed;

        /// <summary>
        /// Invokes the <see cref="Updated"/> event.
        /// </summary>
        protected virtual void OnUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        protected void OnChanged()
        {
            dirty = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Recalculates all values of the <see cref="Transform"/>.
        /// </summary>
        public virtual bool Recalculate()
        {
            globalPrevious = global;
            viewPrevious = view;
            if (!dirty)
            {
                return false;
            }

            local = Matrix4x4.CreateScale(scale) * Matrix4x4.CreateFromQuaternion(orientation) * Matrix4x4.CreateTranslation(position);
            Matrix4x4.Invert(local, out localInverse);
            if (parent == null)
            {
                global = local;
            }
            else
            {
                global = local * parent.global;
            }

            Matrix4x4.Invert(global, out globalInverse);

            Matrix4x4.Decompose(global, out globalScale, out globalOrientation, out globalPosition);
            forward = Vector3.Transform(Vector3.UnitZ, globalOrientation);
            backward = Vector3.Transform(-Vector3.UnitZ, globalOrientation);
            right = Vector3.Transform(Vector3.UnitX, globalOrientation);
            left = Vector3.Transform(-Vector3.UnitX, globalOrientation);
            up = Vector3.Transform(Vector3.UnitY, globalOrientation);
            down = Vector3.Transform(-Vector3.UnitY, globalOrientation);

            view = MathUtil.LookAtLH(globalPosition, forward + globalPosition, up);
            Matrix4x4.Invert(view, out viewInv);
            OnUpdated();
            dirty = false;
            return true;
        }

        /// <summary>
        /// Reverse calculates the local params from a local transform matrix.
        /// </summary>
        /// <param name="matrix">Local space transform matrix</param>
        private void SetMatrix(Matrix4x4 matrix)
        {
            local = matrix;
            Matrix4x4.Invert(local, out localInverse);
            Matrix4x4.Decompose(local, out scale, out orientation, out position);
            rotation = orientation.GetRotation().ToDeg();
            OnChanged();
        }

        /// <summary>
        /// Stores the current state. Used internally for the editor.
        /// </summary>
        public void SaveState()
        {
            initial = CreateSnapshot();
        }

        /// <summary>
        /// Stores the current state. Used internally for the editor.
        /// </summary>
        public void RestoreState()
        {
            backward = initial.Backward;
            down = initial.Down;
            forward = initial.Forward;
            left = initial.Left;
            globalPrevious = initial.GlobalPrevious;
            global = initial.Global;
            globalInverse = initial.GlobalInverse;
            oldpos = initial.OldPos;
            orientation = initial.Orientation;
            parent = initial.Parent;
            position = initial.Position;
            right = initial.Right;
            rotation = initial.Rotation;
            scale = initial.Scale;
            up = initial.Up;
            velocity = initial.Velocity;
            viewPrevious = initial.ViewPrevious;
            view = initial.View;
            viewInv = initial.ViewInv;
        }

        public TransformSnapshot CreateSnapshot()
        {
            return new TransformSnapshot()
            {
                Backward = backward,
                Down = down,
                Forward = forward,
                Left = left,
                GlobalPrevious = globalPrevious,
                Global = global,
                GlobalInverse = globalInverse,
                OldPos = oldpos,
                Orientation = orientation,
                Parent = parent,
                Position = position,
                Right = right,
                Rotation = rotation,
                Scale = scale,
                Up = up,
                Velocity = velocity,
                ViewPrevious = viewPrevious,
                View = view,
                ViewInv = viewInv
            };
        }

        /// <summary>
        /// Clones this <see cref="Transform"/> instance.
        /// </summary>
        /// <returns>Returns a new instance of <see cref="Transform"/> with the values of this <see cref="Transform"/></returns>
        public virtual object Clone()
        {
            return new Transform()
            {
                backward = backward,
                down = down,
                forward = forward,
                left = left,
                globalPrevious = globalPrevious,
                global = global,
                globalInverse = globalInverse,
                oldpos = oldpos,
                orientation = orientation,
                parent = parent,
                position = position,
                right = right,
                rotation = rotation,
                scale = scale,
                up = up,
                velocity = velocity,
                viewPrevious = viewPrevious,
                view = view,
                viewInv = viewInv
            };
        }

        public static implicit operator Matrix4x4(Transform transform) => transform.global;
    }
}