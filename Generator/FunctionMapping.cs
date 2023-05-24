namespace Generator
{
    public class FunctionMapping
    {
        public FunctionMapping(string exportedName, string friendlyName, Dictionary<string, string> defaults, List<Dictionary<string, string>> customVariations)
        {
            ExportedName = exportedName;
            FriendlyName = friendlyName;
            Defaults = defaults;
            CustomVariations = customVariations;
        }

        public string ExportedName { get; set; }

        public string FriendlyName { get; set; }

        public Dictionary<string, string> Defaults { get; set; }

        public List<Dictionary<string, string>> CustomVariations { get; set; }
    }
}