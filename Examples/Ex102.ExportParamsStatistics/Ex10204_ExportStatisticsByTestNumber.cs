using System.Text;
using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex102.ExportStdfSummaryToFile
{
    /// <summary>
    /// 将多个 STDF 文件的测试参数统计结果导出为长格式（Long Format）CSV。
    /// 每条记录为 "FileName,WaferID,LotID,SubLotID,TestNumber,TestText,StatisticItem,Value"，
    /// 每个测试项输出 19 个统计指标（TestedCount、PassCount、FailCount、FailRate、
    /// MeanValue、Sigma、P0.1/P0.5/P1/P5/P10/P25/P50/P75/P90/P95/P99/P99.5/P99.9）以及 LowLimit、HighLimit 测试限值。
    ///
    /// <para>
    /// 与 <see cref="Ex10202_ExportParamsStatisticsLongFormat"/> 的区别：
    /// 本范例不直接使用 TestIDSet 中的 TestID 作为字典键查询参数，
    /// 而是从 TestIDSet 中提取 TestNumber，
    /// 通过 <see cref="TestItemDictionary.GetTestID(TestNum, out TestID)"/>
    /// 获取解析后的 STDF 数据中第一个匹配的 TestID，再据此获取统计数据。
    /// </para>
    /// </summary>
    public static class Ex10204_ExportStatisticsByTestNumber
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── 统计指标定义 ──────────────────────────────────────
            var statisticItems = new List<(string ItemName, Func<TestItemStatistics, string> GetValue)>
            {
                ("TestedCount", s => s.TestedCount.ToString()),
                ("PassCount",   s => s.PassCount.ToString()),
                ("FailCount",   s => s.FailCount.ToString()),
                ("FailRate",    s => double.IsNaN(s.FailRate) ? "" : s.FailRate.ToString("F4")),
                ("MeanValue",   s => s.MeanValue.ToString("G")),
                ("Sigma",       s => s.Sigma.ToString("G")),
                ("P0.1",        s => s.P0_1.ToString("G")),
                ("P0.5",        s => s.P0_5.ToString("G")),
                ("P1",          s => s.P1.ToString("G")),
                ("P5",          s => s.P5.ToString("G")),
                ("P10",         s => s.P10.ToString("G")),
                ("P25",         s => s.P25.ToString("G")),
                ("P50",         s => s.P50.ToString("G")),
                ("P75",         s => s.P75.ToString("G")),
                ("P90",         s => s.P90.ToString("G")),
                ("P95",         s => s.P95.ToString("G")),
                ("P99",         s => s.P99.ToString("G")),
                ("P99.5",       s => s.P99_5.ToString("G")),
                ("P99.9",       s => s.P99_9.ToString("G")),
            };

            // ── Step 1: 浏览并获取 STDF 文件列表 ─────────────────
            var dataPrisms = TDAFileDialog.OpenDataPrisms(multiBrowse: true);
            if (dataPrisms is null)
            {
                Msg.Debug("No valid file selected");
                return;
            }

            Msg.Information($"共选择 {dataPrisms.Count} 个 STDF 文件。");

            // ── Step 2: 获取 TestIDSet 配置文件（提取 TestNumber） ─
            TestIDSet? testIdSet = TDAFileDialog.OpenTestIDSet();
            if (testIdSet is null || testIdSet.Count == 0)
            {
                Msg.Debug("No valid TestIDSet selected");
                return;
            }
            Msg.Information($"TestIDSet 加载完成，共 {testIdSet.Count} 个测试项。");

            // 从 TestIDSet 中提取唯一的 TestNumber 列表
            var targetTestNumbers = testIdSet
                .Select(tid => tid.TestNumber)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
            Msg.Information($"提取到 {targetTestNumbers.Count} 个唯一 TestNumber。");

            // ── Step 3: 选择输出 CSV 路径 ────────────────────────
            string? outputPath = TDAFileDialog.SelectSaveFile(
                filter: "CSV 文件 (*.csv)|*.csv|所有文件 (*.*)|*.*",
                title: "保存统计结果 CSV",
                initialFile: $"ParametricStatistic_ByTestNumber_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            if (outputPath is null) return;

            // ── Step 4: 逐文件解析并导出统计 ─────────────────────
            using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);
            writer.WriteLine("FileName,WaferID,LotID,SubLotID,TestNumber,TestText,StatisticItem,Value");

            foreach (var dp in dataPrisms)
            {
                try
                {
                    dp.Load();
                    dp.DoStatisticAnalysis();
                    var stdfFileName = dp.Setup.PrimaryFile;
                    Msg.Information($"{stdfFileName} ...");
                
                    // 获取 WaferID / LotID / SubLotID
                    string waferId = string.Empty;
                    string lotId = string.Empty;
                    string subLotId = string.Empty;
                    if (dp.DataSourcesInfoList.Count > 0)
                    {
                        waferId = dp.DataSourcesInfoList[0].Wir?.WAFER_ID ?? string.Empty;
                        lotId = dp.DataSourcesInfoList[0].Mir?.LOT_ID ?? string.Empty;
                        subLotId = dp.DataSourcesInfoList[0].Mir?.SBLOT_ID ?? string.Empty;
                    }

                    // 对每个目标 TestNumber，通过 GetTestID() 获取解析后数据中
                    // 第一个匹配的 TestID，再据此获取统计数据
                    int matchedCount = 0;
                    foreach (var testNumber in targetTestNumbers)
                    {
                        if (!dp.TestItems.GetTestID(testNumber, out TestID tid))
                        {
                            Msg.Warning($"  TestNumber {testNumber} 在 {stdfFileName} 中未找到");
                            continue;
                        }

                        var testItem = dp.TestItems[tid];
                        matchedCount++;
                        var stat = testItem.Statistic;

                        foreach (var (itemName, getValue) in statisticItems)
                        {
                            string value = getValue(stat);
                            writer.WriteLine($"{stdfFileName},{waferId},{lotId},{subLotId},{tid.TestNumber},\"{tid.TestText}\",{itemName},{value}");
                        }

                        // 输出上下限（来自 TestItem.Info，与统计指标独立）
                        writer.WriteLine($"{stdfFileName},{waferId},{lotId},{subLotId},{tid.TestNumber},\"{tid.TestText}\",LowLimit,{testItem.Info.LowLimit}");
                        writer.WriteLine($"{stdfFileName},{waferId},{lotId},{subLotId},{tid.TestNumber},\"{tid.TestText}\",HighLimit,{testItem.Info.HighLimit}");
                    }

                    Msg.Information($"  OK (matched {matchedCount}/{targetTestNumbers.Count} tests, WaferID={waferId})");
                }
                catch (Exception ex)
                {
                    Msg.Error($"  ERROR: {ex.Message}");
                }
                finally
                {
                    dp?.Dispose();
                }
            }

            // ── Step 5: 输出汇总信息 ──────────────────────────────
            Msg.Information($"输出文件: {outputPath}");
        }
    }
}
