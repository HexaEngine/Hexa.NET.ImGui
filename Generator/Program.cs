using Generator.Targets;
using HexaGen.BuildSystems;

var builder =
   BuildSystemBuilder.Create()
    .WithArgs(args)
    .AddImGui()
    .AddImPlot()
    .AddImNodes()
    .AddImGuizmo()
    .AddImPlot3D()
    .AddImGuiBackends()
    .AddImGuiNodeEditor(false);

var context = builder.Build();
context.Execute();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("All done! Have fun!");
Console.ForegroundColor = ConsoleColor.White;