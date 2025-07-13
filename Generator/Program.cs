namespace Generator
{
    using Generator.Targets;
    using HexaGen.BuildSystems;

    internal unsafe class Program
    {
        private static void Main(string[] args)
        {
            var builder = BuildSystemBuilder.Create()
                .WithArgs(args)
                .AddImGui()
                .AddImPlot()
                .AddImNodes()
                .AddImGuizmo()
                .AddImGuiBackends()
                .AddImGuiNodeEditor(false);

            var context = builder.Build();
            context.Execute();
        }
    }
}