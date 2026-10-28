namespace Ex105.ExportSummary
{
    /// <summary>
    /// Ex105：导出 HBin/SBin 统计摘要到 CSV 的范例项目。
    /// <para>Ex10501 调用 DataPrism.ExportSummary 导出摘要；Ex10502 展示自定义格式导出；Ex10503 展示多批次（IEnumerable）合并导出。</para>
    /// <para>Ex10504/10505/10506 分别单独导出 Yield / HBin / SBin 区块（10506 另演示 csvWriter 重载的流式组合用法）。</para>
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Ex10501_ExportSummary.Execute();
            //Ex10502_ExportSummaryCustomizeFormat.Execute();
            //Ex10503_ExportSummaryMultiLot.Execute();
            //Ex10504_ExportYield.Execute();
            //Ex10505_ExportHBin.Execute();
            Ex10506_ExportSBin.Execute();
            Console.WriteLine("Job Done!");
        }
    }
}
