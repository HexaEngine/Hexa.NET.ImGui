namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Core.CSharp;
    using HexaGen.FunctionGeneration;

    public class ManualFunctionGenRuleSpan : FunctionGenRule
    {
        public override CsParameterInfo CreateParameter(CppParameter cppParameter, string csParamName, CppPrimitiveKind kind, Direction direction, CsCodeGeneratorConfig settings, IList<CppParameter> cppParameters, CsParameterInfo[] csParameterList, int paramIndex, CsFunctionVariation variation)
        {
            if (cppParameter.Type is CppArrayType arrayType)
            {
                if (arrayType.Size > 0)
                {
                    return new(csParamName, cppParameter.Type, new($"ReadOnlySpan<{settings.GetCsTypeName(arrayType.ElementType, false)}>", kind), direction);
                }
            }
            else if (cppParameter.Type.IsString())
            {
                switch (kind)
                {
                    case CppPrimitiveKind.Char:
                        if (direction == Direction.InOut || direction == Direction.Out) break;
                        return new(csParamName, cppParameter.Type, new("ReadOnlySpan<byte>", kind), direction);

                    case CppPrimitiveKind.WChar:
                        if (direction == Direction.InOut || direction == Direction.Out) break;
                        return new(csParamName, cppParameter.Type, new("ReadOnlySpan<char>", kind), direction);
                }
            }

            return CreateDefaultWrapperParameter(cppParameter, csParamName, kind, direction, settings);
        }
    }
}