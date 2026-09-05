using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;
using static TDA.DataPrism;

namespace Ex103.ExportRawData
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex10301_ExportDataWithSpecifiedTestIDs.Execute();
            //Ex10302_ExportDataWithTestIDSelector.Execute();
            //Ex10311_ExportDataWithPartFilter.Execute();
            Console.WriteLine("Job Done!");
        }
    }
}
