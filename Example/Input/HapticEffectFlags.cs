namespace HexaEngine.Core.Input
{
    public enum HapticEffectFlags : uint
    {
        Constant = 1u << 0,
        Sine = 1u << 1,
        LeftRight = 1u << 2,
        Triangle = 1u << 3,
        SawToothUp = 1u << 4,
        SawToothDown = 1u << 5,
        Ramp = 1u << 6,
        Spring = 1u << 7,
        Damper = 1u << 8,
        Inertia = 1u << 9,
        Friction = 1u << 10,
        Custom = 1u << 11,
        Gain = 1u << 12,
        AutoCenter = 1u << 13,
        Status = 1u << 14,
        Pause = 1u << 15,

        Infinity = 4294967295U,
    }
}