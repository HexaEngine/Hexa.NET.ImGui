namespace Generator
{
    using CppAst;
    using HexaGen;
    using HexaGen.Patching;

    public class NamingPatch : PrePatch
    {
        private readonly string[] prefixes;

        public NamingPatch(params string[] name)
        {
            prefixes = name;
        }

        protected override void PatchFunction(CsCodeGeneratorConfig settings, CppFunction cppFunction)
        {
            var name = settings.GetCsFunctionName(cppFunction.Name);
            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    name = name[prefix.Length..];
                }
            }

            if (!settings.TryGetFunctionMapping(cppFunction.Name, out var mapping))
            {
                mapping = new(cppFunction.Name, name, null, [], []);
                settings.FunctionMappings.Add(mapping);
            }

            mapping.FriendlyName ??= name;
        }
    }
}