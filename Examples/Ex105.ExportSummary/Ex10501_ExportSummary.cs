using TDA;
using TDA.FileIO;

namespace Ex105.ExportSummary
{
    /// <summary>
    /// 使用 <see cref="DataPrism.ExportSummary"/> 从 STDF 文件导出 HBin/SBin 统计摘要到 CSV。
    /// <para>流程：选择 STDF → 加载解析 → 调用 DataPrism.ExportSummary 导出摘要。</para>
    /// </summary>
    public static class Ex10501_ExportSummary
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 浏览并选择 STDF 文件 ────────────────────────────
            DataPrism? dataPrism = TDAFileDialog.OpenDataPrism();
            if (dataPrism is null) return;

            // ── Step 2: 加载并解析数据 ───────────────────────────────────
            dataPrism.Load();
            dataPrism.DoStatisticAnalysis();

            // ── Step 3: 调用 DataPrism 的导出函数，输出摘要到 CSV ───────
            string outfile = DataPrism.ExportSummary(dataPrism);
            Console.WriteLine($"Summary exported to: {outfile}");
        }
    }
}
