using TDA;
using TDA.FileIO;

namespace Ex105.ExportSummary
{
    /// <summary>
    /// 使用 <see cref="DataPrism.ExportSummary(System.Collections.Generic.IEnumerable{TDA.DataPrism}, string)"/>
    /// 多选多个 STDF 文件，将各批次的 HBin/SBin 统计摘要合并导出到同一个 CSV。
    /// <para>
    /// 流程：多选 STDF → 逐个加载解析 → 调用 IEnumerable 版本 ExportSummary 一次导出。
    /// 每个批次输出一个区块（Qty / HBin / SBin Statistic），区块前以「==== 数据源名称 ====」标识行分隔。
    /// 仅选择一个文件时输出与单 DataPrism 版本完全一致。
    /// </para>
    /// </summary>
    public static class Ex10503_ExportSummaryMultiLot
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 多选 STDF 文件 ──────────────────────────────────
            List<DataPrism>? dataPrisms = TDAFileDialog.OpenDataPrisms();
            if (dataPrisms is null || dataPrisms.Count == 0)
            {
                Console.WriteLine("未选择任何文件，退出。");
                return;
            }
            Console.WriteLine($"已选择 {dataPrisms.Count} 个文件。");

            // ── Step 2: 逐个加载解析数据 ────────────────────────────────
            foreach (var dataPrism in dataPrisms)
            {
                dataPrism.Load();
                dataPrism.DoStatisticAnalysis();
            }

            // ── Step 3: 调用 IEnumerable 版本 ExportSummary 合并导出 ────
            // 不传 outfile 时，默认输出到第一个文件所在目录 + .sum.csv
            string outfile = DataPrism.ExportSummary(dataPrisms);
            if (string.IsNullOrEmpty(outfile))
            {
                Console.WriteLine("导出失败：没有可导出的数据。");
                return;
            }
            Console.WriteLine($"Summary exported to: {outfile}");
        }
    }
}
