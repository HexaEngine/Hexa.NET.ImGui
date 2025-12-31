using Generator.Targets;
using HexaGen.BuildSystems;

var start = DateTime.Now;
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
var end = DateTime.Now;
var duration = end - start;
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine($"Generation completed in {duration.TotalSeconds:F2} seconds.");

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("All done! Have fun!");
Console.ForegroundColor = ConsoleColor.White;