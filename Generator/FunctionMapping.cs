namespace Generator
{
    public class FunctionMapping
    {
        public FunctionMapping(string exportedName, string friendlyName, Dictionary<string, string> defaults)
        {
            ExportedName = exportedName;
            FriendlyName = friendlyName;
            Defaults = defaults;
        }

        public string ExportedName { get; set; }

        public string FriendlyName { get; set; }

        public Dictionary<string, string> Defaults { get; set; }
    }
}