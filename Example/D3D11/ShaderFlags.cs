namespace HexaEngine.D3D11
{
    /// <unmanaged>D3DCOMPILE_SHADER_FLAGS</unmanaged>
    /// <unmanaged-short>D3DCOMPILE_SHADER_FLAGS</unmanaged-short>
    [Flags]
    public enum ShaderFlags : int
    {
        /// <unmanaged>D3DCOMPILE_DEBUG</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_DEBUG</unmanaged-short>
        Debug = unchecked(1),

        /// <unmanaged>D3DCOMPILE_SKIP_VALIDATION</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_SKIP_VALIDATION</unmanaged-short>
        SkipValidation = unchecked(2),

        /// <unmanaged>D3DCOMPILE_SKIP_OPTIMIZATION</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_SKIP_OPTIMIZATION</unmanaged-short>
        SkipOptimization = unchecked(4),

        /// <unmanaged>D3DCOMPILE_PACK_MATRIX_ROW_MAJOR</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_PACK_MATRIX_ROW_MAJOR</unmanaged-short>
        PackMatrixRowMajor = unchecked(8),

        /// <unmanaged>D3DCOMPILE_PACK_MATRIX_COLUMN_MAJOR</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_PACK_MATRIX_COLUMN_MAJOR</unmanaged-short>
        PackMatrixColumnMajor = unchecked(16),

        /// <unmanaged>D3DCOMPILE_PARTIAL_PRECISION</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_PARTIAL_PRECISION</unmanaged-short>
        PartialPrecision = unchecked(32),

        /// <unmanaged>D3DCOMPILE_FORCE_VS_SOFTWARE_NO_OPT</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_FORCE_VS_SOFTWARE_NO_OPT</unmanaged-short>
        ForceVsSoftwareNoOpt = unchecked(64),

        /// <unmanaged>D3DCOMPILE_FORCE_PS_SOFTWARE_NO_OPT</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_FORCE_PS_SOFTWARE_NO_OPT</unmanaged-short>
        ForcePsSoftwareNoOpt = unchecked(128),

        /// <unmanaged>D3DCOMPILE_NO_PRESHADER</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_NO_PRESHADER</unmanaged-short>
        NoPreshader = unchecked(256),

        /// <unmanaged>D3DCOMPILE_AVOID_FLOW_CONTROL</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_AVOID_FLOW_CONTROL</unmanaged-short>
        AvoidFlowControl = unchecked(512),

        /// <unmanaged>D3DCOMPILE_PREFER_FLOW_CONTROL</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_PREFER_FLOW_CONTROL</unmanaged-short>
        PreferFlowControl = unchecked(1024),

        /// <unmanaged>D3DCOMPILE_ENABLE_STRICTNESS</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_ENABLE_STRICTNESS</unmanaged-short>
        EnableStrictness = unchecked(2048),

        /// <unmanaged>D3DCOMPILE_ENABLE_BACKWARDS_COMPATIBILITY</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_ENABLE_BACKWARDS_COMPATIBILITY</unmanaged-short>
        EnableBackwardsCompatibility = unchecked(4096),

        /// <unmanaged>D3DCOMPILE_IEEE_STRICTNESS</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_IEEE_STRICTNESS</unmanaged-short>
        IeeeStrictness = unchecked(8192),

        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL0</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL0</unmanaged-short>
        OptimizationLevel0 = unchecked(16384),

        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL1</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL1</unmanaged-short>
        OptimizationLevel1 = unchecked(0),

        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL2</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL2</unmanaged-short>
        OptimizationLevel2 = unchecked(49152),

        /// <unmanaged>D3DCOMPILE_OPTIMIZATION_LEVEL3</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_OPTIMIZATION_LEVEL3</unmanaged-short>
        OptimizationLevel3 = unchecked(32768),

        /// <unmanaged>D3DCOMPILE_RESERVED16</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_RESERVED16</unmanaged-short>
        Reserved16 = unchecked(65536),

        /// <unmanaged>D3DCOMPILE_RESERVED17</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_RESERVED17</unmanaged-short>
        Reserved17 = unchecked(131072),

        /// <unmanaged>D3DCOMPILE_WARNINGS_ARE_ERRORS</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_WARNINGS_ARE_ERRORS</unmanaged-short>
        WarningsAreErrors = unchecked(262144),

        /// <unmanaged>D3DCOMPILE_DEBUG_NAME_FOR_SOURCE</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_DEBUG_NAME_FOR_SOURCE</unmanaged-short>
        DebugNameForSource = unchecked(4194304),

        /// <unmanaged>D3DCOMPILE_DEBUG_NAME_FOR_BINARY</unmanaged>
        /// <unmanaged-short>D3DCOMPILE_DEBUG_NAME_FOR_BINARY</unmanaged-short>
        DebugNameForBinary = unchecked(8388608),

        /// <summary>
        /// Synthetic NONE value
        /// </summary>
        /// <unmanaged>None</unmanaged>
        /// <unmanaged-short>None</unmanaged-short>
        None = unchecked(0)
    }
}