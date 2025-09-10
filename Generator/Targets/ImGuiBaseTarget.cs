namespace Generator.Targets
{
    using HexaGen;
    using HexaGen.BuildSystems;
    using HexaGen.Metadata;
    using HexaGen.Patching;

    public abstract class BuildTargetBase : BuildTarget
    {
        public abstract void ExecuteCore(BuildContext context);

        public override void Execute(BuildContext context)
        {
            Console.WriteLine($"Generating: {Name}");
            ExecuteCore(context);
        }
    }

    public abstract class ImGuiBaseTarget : BuildTargetBase
    {
        public static readonly string ImGuiMetadataKey = "ImGuiMetadata";

        protected static CsCodeGeneratorMetadata GetImGuiMetadata(BuildContext ctx) => ctx.GetVar<CsCodeGeneratorMetadata>(ImGuiMetadataKey);

        public override void ExecuteCore(BuildContext context)
        {
            Execute(context);
        }

        public virtual new void Execute(BuildContext context)
        {
            throw new NotImplementedException();
        }

        protected void Generate(IEnumerable<string> headers, string settingsPath, string output, CsCodeGeneratorMetadata? lib, out CsCodeGeneratorMetadata metadata, InternalsGenerationType type)
        {
            var builder = GeneratorBuilder.Create<ImGuiCodeGenerator>(settingsPath)
                .AlterConfig(config =>
                {
                    config.WrapPointersAsHandle = true;
                })
                .WithPrePatch<ImVectorPatch>()
                .WithPrePatch<ImGuiDefinitionsPatch>(() => new(type))
                .WithPrePatch<ImGuizmoPrePatch>()
                .WithPrePatch<ImGuiPrePatch>()
                .WithPrePatch<NamingPatch>(() => new(["CImGui", "ImGui", "ImGuizmo", "ImNodes", "ImPlot", "ImplSDL2", "ImplSDL3", "ImplGlfw", "Impl"], NamingPatchOptions.MultiplePrefixes))
                .WithPostPatch<ImGuiPostPatch>()
                .WithPostPatch<ImGuiBackendsPostPatch>()
                .WithPostPatch<ImGuiGenQuirksPostPatch>()
                .CopyFromMetadata(lib);

            OnSetup(builder);

            builder
                .Generate([.. headers], output)
                .GetMetadata(out metadata);
        }

        protected virtual void OnSetup(GeneratorBuilder builder)
        {

        }
    }
}