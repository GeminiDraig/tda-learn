namespace Ex102.ExportStdfSummaryToFile
{
    /// <summary>
    /// Ex102：导出 STDF 测试参数统计结果到 CSV。
    /// 支持宽格式（Ex10201）、长格式直接匹配（Ex10202）、
    /// 基于 TestText 匹配（Ex10203）、基于 TestNumber 匹配（Ex10204）。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Ex10201_ExportParamsStatistics.Execute();
            //Ex10202_ExportParamsStatisticsLongFormat.Execute();
            Ex10203_ExportStatisticsByTestText.Execute();
            //Ex10204_ExportStatisticsByTestNumber.Execute();
            Console.WriteLine("Job done!");
        }
    }
}
