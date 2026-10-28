using TDA;
using TDA.FileIO;

namespace Ex105.ExportSummary
{
    /// <summary>
    /// 使用 <see cref="DataPrism.ExportYield(DataPrism, CsvHelper.CsvWriter)"/> 单独导出良率（Qty Statistic）区块。
    /// <para>内容：Total / Pass / Fail / Invalid 四行的全局值与各 Site 值，含 Pct(%) 占比列。</para>
    /// </summary>
    public static class Ex10504_ExportYield
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

            // ── Step 3: 只导出良率区块 ──────────────────────────────────
            // 默认输出：<源文件 stem>.yield.csv
            string outfile = DataPrism.ExportYield(dataPrism);
            Console.WriteLine($"Yield exported to: {outfile}");
        }
    }
}
