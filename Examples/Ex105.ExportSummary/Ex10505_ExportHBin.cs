using TDA;
using TDA.FileIO;

namespace Ex105.ExportSummary
{
    /// <summary>
    /// 使用 <see cref="DataPrism.ExportHBin(DataPrism, CsvHelper.CsvWriter)"/> 单独导出 HardBin 统计区块。
    /// <para>
    /// 列：<c>HBin, ResultType, Name, Pct(%), All, S&lt;site&gt;…</c>；
    /// ResultType 标明该 Bin 是 Pass / Fail / Invalid。
    /// </para>
    /// </summary>
    public static class Ex10505_ExportHBin
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

            // ── Step 3: 只导出 HardBin 区块 ─────────────────────────────
            // 默认输出：<源文件 stem>.hbin.csv
            string outfile = DataPrism.ExportHBin(dataPrism);
            Console.WriteLine($"HardBin exported to: {outfile}");
        }
    }
}
