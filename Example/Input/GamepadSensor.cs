namespace HexaEngine.Core.Input
{
    using HexaEngine.Core.Input.Events;
    using Silk.NET.SDL;
    using System.Numerics;

    public unsafe class GamepadSensor : IDisposable
    {
        private readonly Sdl sdl = Application.sdl;
        private readonly GameController* controller;
        private readonly GamepadSensorType type;
        private readonly float* buffer;
        private readonly int length = 3;

        private readonly GamepadSensorUpdateEventArgs sensorUpdateEventArgs = new();

        private bool disposedValue;

        public GamepadSensor(GameController* controller, GamepadSensorType sensorType)
        {
            this.controller = controller;
            this.type = sensorType;
            buffer = Alloc<float>(3);
            sdl.GameControllerGetSensorData(controller, Helper.ConvertBack(sensorType), buffer, length);
        }

        public bool Enabled
        {
            get => sdl.GameControllerIsSensorEnabled(controller, Helper.ConvertBack(type)) == SdlBool.True;
            set => sdl.GameControllerSetSensorEnabled(controller, Helper.ConvertBack(type), value ? SdlBool.True : SdlBool.False);
        }

        public GamepadSensorType Type => type;

        public Span<float> Data => new(buffer, length);

        public Vector3 Vector => *(Vector3*)buffer;

        public event EventHandler<GamepadSensorUpdateEventArgs>? SensorUpdate;

        internal void OnSensorUpdate(ControllerSensorEvent even)
        {
            MemoryCopy(even.Data, buffer, length);
            sensorUpdateEventArgs.Data = buffer;
            sensorUpdateEventArgs.Length = length;
            sensorUpdateEventArgs.Type = type;
            SensorUpdate?.Invoke(this, sensorUpdateEventArgs);
        }

        public void Flush()
        {
            sdl.GameControllerGetSensorData(controller, Helper.ConvertBack(type), buffer, length);
            sensorUpdateEventArgs.Data = buffer;
            sensorUpdateEventArgs.Length = length;
            sensorUpdateEventArgs.Type = type;
            SensorUpdate?.Invoke(this, sensorUpdateEventArgs);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Free(buffer);
                disposedValue = true;
            }
        }

        ~GamepadSensor()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}