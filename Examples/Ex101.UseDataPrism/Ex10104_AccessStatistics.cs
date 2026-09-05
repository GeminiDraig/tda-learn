using TDA;
using TDA.FileIO;
using TDA.TypeDef;

namespace Ex101.UseDataPrism
{
    /// <summary>
    /// 演示 Load + DoStatisticAnalysis 后可访问的统计数据：
    /// 1) TestItem.Statistic — 均值、标准差、Cp/Cpk、百分位数等
    /// 2) TestItem.Statistic 按 Site 统计 — Site 间 Gap (SGM/SGI)
    /// 3) DataPrism.Summary — Lot 级别 Bin 汇总（Yield、HBin/SBin 计数）
    ///
    /// 关键点：统计分析必须在 Load 之后执行，才能访问 Statistic 和 Summary。
    /// </summary>
    public static class Ex10104_AccessStatistics
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 弹窗选文件 → Load → Analyze ─────────────────
            DataPrism? dataPrism = TDAFileDialog.OpenDataPrism();
            if (dataPrism is null)
                throw new ArgumentNullException("invalid data prism input");

            dataPrism.Load();
            dataPrism.DoStatisticAnalysis();

            // ── Step 2: 访问 TestItem.Statistic ──────────────────────
            Console.WriteLine("══════ TestItem Statistic (前 5 个参数测试项) ══════");
            int tidCount = 0;
            foreach (var kvp in dataPrism.TestItems)
            {
                var item = kvp.Value;
                if (!item.IsParametric)
                    continue;

                if (tidCount++ >= 5)
                    break;

                TestID testID = kvp.Key;
                var stat = item.Statistic;

                Console.WriteLine($"  T{testID.TestNumber} [{item.Info.TestText}]");
                Console.WriteLine($"    Tested={stat.TestedCount}  Pass={stat.PassCount}  " +
                    $"Fail={stat.FailCount}  FailRate={stat.FailRate:F2}%");
                Console.WriteLine($"    Mean={stat.MeanValue:F4}  Median={stat.MedianValue:F4}  " +
                    $"Sigma={stat.Sigma:F4}");
                Console.WriteLine($"    Min={stat.MinValue:F4}  Max={stat.MaxValue:F4}");
                Console.WriteLine($"    Cp={stat.Cp:F3}  Cpk={stat.Cpk:F3}  " +
                    $"(Cpl={stat.Cpl:F3}, Cpu={stat.Cpu:F3})");

                // ── 按 Site 统计与 Site 间 Gap ─────────────────────
                if (stat.Sites is { Count: > 1 })
                {
                    Console.WriteLine($"    Site Gap: SGM={stat.SGM:F4}  SGI={stat.SGI:F4}");
                    for (int i = 0; i < stat.Sites.Count; i++)
                    {
                        Console.WriteLine($"      Site {stat.Sites[i]}:  " +
                            $"Tested={stat.SitesTestedCount[i]}  " +
                            $"Pass={stat.SitesPassCount[i]}  " +
                            $"Fail={stat.SitesFailCount[i]}  " +
                            $"Mean={stat.SitesMeanValue[i]:F4}");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"  ... 共 {dataPrism.TestItems.Count} 个 TestID");
            Console.WriteLine();

            // ── Step 3: 访问 DataPrism.Summary (Bin 汇总) ───────────
            Console.WriteLine("══════ Summary (Lot 级别 Bin 汇总) ══════");
            var summary = dataPrism.Summary;
            double yield = summary.TotalQty > 0
                ? summary.PassQty * 100.0 / summary.TotalQty
                : 0;
            Console.WriteLine($"  Total Parts:    {summary.TotalQty}");
            Console.WriteLine($"  Pass:           {summary.PassQty}");
            Console.WriteLine($"  Fail:           {summary.FailQty}");
            Console.WriteLine($"  Invalid:        {summary.InvalidQty}");
            Console.WriteLine($"  Yield:          {yield:F2}%");

            Console.WriteLine();
            Console.WriteLine("  HardBin 分布:");
            foreach (var kvp in summary.HBinCnt.OrderBy(x => x.Key))
            {
                double pct = summary.TotalQty > 0
                    ? kvp.Value * 100.0 / summary.TotalQty
                    : 0;
                // 尝试获取 Bin 名称
                string binName = summary.HBins.TryGetValue(kvp.Key, out var bi)
                    ? bi.Name ?? ""
                    : "";
                Console.WriteLine($"    Bin {kvp.Key,3} [{binName}]: {kvp.Value,6} ({pct:F2}%)");
            }

            Console.WriteLine();
            Console.WriteLine("  SoftBin 分布:");
            foreach (var kvp in summary.SBinCnt.OrderBy(x => x.Key))
            {
                double pct = summary.TotalQty > 0
                    ? kvp.Value * 100.0 / summary.TotalQty
                    : 0;
                string binName = summary.SBins.TryGetValue(kvp.Key, out var bi)
                    ? bi.Name ?? ""
                    : "";
                Console.WriteLine($"    Bin {kvp.Key,3} [{binName}]: {kvp.Value,6} ({pct:F2}%)");
            }

            dataPrism.Dispose();
        }
    }
}
