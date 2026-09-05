namespace Ex105.ExportSummary
{
    /// <summary>
    /// Ex105：导出 HBin/SBin 统计摘要到 CSV 的范例项目。
    /// <para>Ex10501 调用 DataPrism.ExportSummary 导出摘要；Ex10502 展示自定义格式导出；Ex10503 展示多批次（IEnumerable）合并导出。</para>
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Ex10501_ExportSummary.Execute();
            //Ex10502_ExportSummaryCustomizeFormat.Execute();
            Ex10503_ExportSummaryMultiLot.Execute();
            Console.WriteLine("Job Done!");
        }
    }
}
