namespace Generator
{
    using HexaGen;
    using HexaGen.Core.CSharp;
    using HexaGen.Core.Mapping;
    using HexaGen.CppAst.Model.Declarations;
    using HexaGen.CppAst.Model.Types;
    using HexaGen.FunctionGeneration;

    public class ManualFunctionGenRuleSpan : FunctionGenRule
    {
        public override CsParameterInfo CreateParameter(CppParameter cppParameter, ParameterMapping? mapping, string csParamName, CppPrimitiveKind kind, Direction direction, CsCodeGeneratorConfig settings, IList<CppParameter> cppParameters, CsParameterInfo[] csParameters, int paramIndex, CsFunctionVariation variation)
        {
            if (cppParameter.Type is CppArrayType arrayType)
            {
                if (arrayType.Size > 0)
                {
                    return new(csParamName, cppParameter.Type, new($"ReadOnlySpan<{settings.GetCsTypeName(arrayType.ElementType)}>", kind), direction);
                }
            }
            else if (cppParameter.Type.IsString(settings, out var stringKind))
            {
                switch (stringKind)
                {
                    case CppPrimitiveKind.Char:
                        if (direction == Direction.InOut || direction == Direction.Out) break;
                        return new(csParamName, cppParameter.Type, new("ReadOnlySpan<byte>", kind), direction);

                    case CppPrimitiveKind.WChar:
                        if (direction == Direction.InOut || direction == Direction.Out) break;
                        return new(csParamName, cppParameter.Type, new("ReadOnlySpan<char>", kind), direction);
                }
            }

            return CreateDefaultWrapperParameter(cppParameter, mapping, csParamName, kind, direction, settings);
        }
    }
}