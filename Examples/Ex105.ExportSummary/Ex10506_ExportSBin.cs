using CsvHelper;
using TDA;
using TDA.FileIO;

namespace Ex105.ExportSummary
{
    /// <summary>
    /// 使用 <see cref="DataPrism.ExportSBin(DataPrism, CsvHelper.CsvWriter)"/> 单独导出 SoftBin 统计区块，
    /// 并演示 <c>csvWriter</c> 重载：把多个区块自主组合写进同一个 CSV。
    /// </summary>
    public static class Ex10506_ExportSBin
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

            // ── Step 3a: 独立导出 SoftBin 区块 ──────────────────────────
            // 默认输出：<源文件 stem>.sbin.csv
            string sbinFile = DataPrism.ExportSBin(dataPrism);
            Console.WriteLine($"SoftBin exported to: {sbinFile}");

            // ── Step 3b: 使用 csvWriter 重载，把多个区块写进同一个文件 ──
            // 这种方式不与具体输出路径绑定，调用方自己管理 writer 生命周期，
            // 可在同一个流里自由拼接 Yield / HBin / SBin 的任意组合。
            string combined = dataPrism.ResolveDefaultOutputPath(".yield_bins.csv");
            using (var writer = new StreamWriter(combined))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                DataPrism.ExportYield(dataPrism, csv);
                csv.NextRecord(); // 区块之间补一个空行，与 ExportSummary 的排版一致
                DataPrism.ExportHBin(dataPrism, csv);
                csv.NextRecord();
                DataPrism.ExportSBin(dataPrism, csv);
            }
            Console.WriteLine($"Combined (Yield+HBin+SBin) exported to: {combined}");
        }
    }
}
