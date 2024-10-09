namespace Hexa.NET.ImPlot
{
    public static unsafe partial class ImPlot
    {
        static ImPlot()
        {
            InitApi();
        }

        public static string GetLibarayName()
        {
            return "cimplot";
        }
    }
}