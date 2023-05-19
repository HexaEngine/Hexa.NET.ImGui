namespace HexaEngine.Core.Graphics
{
    public struct ComputePipelineDesc
    {
        public string? Path;
        public string Entry;

        public ComputePipelineDesc()
        {
            Path = null;
            Entry = "main";
        }

        public ComputePipelineDesc(string? path) : this()
        {
            Path = path;
        }

        public ComputePipelineDesc(string? path, string entry)
        {
            Path = path;
            Entry = entry;
        }
    }
}