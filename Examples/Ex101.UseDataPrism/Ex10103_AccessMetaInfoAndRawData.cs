using TDA;
using TDA.FileIO;
using TDA.TypeDef;

namespace Ex101.UseDataPrism
{
    /// <summary>
    /// 演示仅 Load（无需 DoStatisticAnalysis）后可直接访问的数据：
    /// 1) DataSourcesInfoList 中的 MIR / SDR / WIR / WRR 元信息
    /// 2) PartsDescription 中各 Part 的 Bin 信息和测试结果
    /// 3) TestItems 中各 TestID 的原始测试值
    ///
    /// 关键点：Load 完成后即可访问原始数据，统计分析不是前置要求。
    /// </summary>
    public static class Ex10103_AccessMetaInfoAndRawData
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 弹窗选文件并 Load（不执行 Analyze） ────────────
            DataPrism? dataPrism = TDAFileDialog.OpenDataPrism();
            if (dataPrism is null)
                throw new ArgumentNullException("invalid data prism input");

            dataPrism.Load();
            // 注意：仅 Load，不调用 DoStatisticAnalysis

            // ── Step 2: 打印 MIR 文件级元信息 ──────────────────────────
            var info = dataPrism.DataSourcesInfoList[0];
            Console.WriteLine("══════ MIR (Master Information Record) ══════");
            Console.WriteLine($"  LOT_ID:       {info.Mir.LOT_ID}");
            Console.WriteLine($"  SBLOT_ID:     {info.Mir.SBLOT_ID}");
            Console.WriteLine($"  SETUP_T:      {info.Mir.SETUP_T:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  START_T:      {info.Mir.START_T:yyyy-MM-dd HH:mm:ss}");
            // FINISH_T 在 MRR (Master Results Record) 中，不是 MIR
            // Console.WriteLine($"  FINISH_T:     {info.Mir.FINISH_T:...}");
            Console.WriteLine($"  TSTR_TYP:     {info.Mir.TSTR_TYP}");
            Console.WriteLine($"  JOB_NAM:      {info.Mir.JOB_NAM}");
            Console.WriteLine($"  NODE_NAM:     {info.Mir.NODE_NAM}");

            // ── Step 3: 打印 SDR 站点信息 ──────────────────────────
            Console.WriteLine();
            Console.WriteLine("══════ SDR (Site Description Record) ══════");
            Console.Write($"  Sites ({info.Sdr.SITE_NUM.Length}): ");
            foreach (byte s in info.Sdr.SITE_NUM)
                Console.Write($"{s} ");
            Console.WriteLine();

            // ── Step 4: 打印 WIR / WRR 晶圆级信息 ───────────────────
            if (info.Wir is not null)
            {
                Console.WriteLine();
                Console.WriteLine("══════ WIR (Wafer Information Record) ══════");
                Console.WriteLine($"  WAFER_ID:     {info.Wir.WAFER_ID}");
                Console.WriteLine($"  HEAD_NUM:     {info.Wir.HEAD_NUM}");
                Console.WriteLine($"  SITE_GRP:     {info.Wir.SITE_GRP}");
            }
            if (info.Wrr is not null)
            {
                Console.WriteLine();
                Console.WriteLine("══════ WRR (Wafer Results Record) ══════");
                Console.WriteLine($"  WAFER_ID:     {info.Wrr.WAFER_ID}");
                Console.WriteLine($"  PART_CNT:     {info.Wrr.PART_CNT}");
                Console.WriteLine($"  GOOD_CNT:     {info.Wrr.GOOD_CNT}");
                double yld = info.Wrr.GOOD_CNT * 100.0 / info.Wrr.PART_CNT;
                Console.WriteLine($"  Yield:        {yld:F2}%");
            }

            // ── Step 5: 打印 Part 信息（前 10 颗） ──────────────────
            Console.WriteLine();
            Console.WriteLine("══════ PartsDescription (前 10 颗) ══════");
            int showCount = Math.Min(10, dataPrism.PartsDescription.Length);
            for (int i = 0; i < showCount; i++)
            {
                var part = dataPrism.PartsDescription[i];
                Console.Write($"  [{i}] SITE={part.SiteNumber}");
                Console.Write($"  HBIN={part.HardBinNumber}");
                Console.Write($"  SBIN={part.SoftBinNumber}");
                Console.Write($"  PASS={part.IsPass}");
                Console.WriteLine($"  IS_FINAL={part.IsFinalData}");
            }
            Console.WriteLine($"  ... 共 {dataPrism.PartsDescription.Length} 颗 Part");

            // ── Step 6: 打印 TestItems 原始值（前 5 个参数测试项） ──
            Console.WriteLine();
            Console.WriteLine("══════ TestItems 原始值 (前 5 个 TestID，各取前 3 个 Part) ══════");
            int tidCount = 0;
            foreach (var kvp in dataPrism.TestItems)
            {
                if (tidCount++ >= 5)
                    break;

                TestID testID = kvp.Key;
                TestItem testItem = kvp.Value;
                Console.Write($"  T{testID.TestNumber} [{testItem.Info.TestText}]");
                if (!string.IsNullOrEmpty(testItem.Info.Units))
                    Console.Write($" ({testItem.Info.Units})");
                if (testItem.Info.LowLimit.HasValue || testItem.Info.HighLimit.HasValue)
                    Console.Write($"  LSL={testItem.Info.LowLimit} USL={testItem.Info.HighLimit}");
                Console.WriteLine();

                int valCount = Math.Min(3, testItem.Length);
                for (int i = 0; i < valCount; i++)
                {
                    if (testItem.TryGetValue(i, out float val, out byte flag))
                        Console.WriteLine($"    Part[{i}] = {val}  (flag={flag})");
                }
                Console.WriteLine($"    有效测试数: {testItem.TestedCount}  Fail: {testItem.FailCount}");
                Console.WriteLine();
            }
            Console.WriteLine($"  ... 共 {dataPrism.TestItems.Count} 个 TestID");
            Console.WriteLine();
            Console.WriteLine("  (仅 Load，未执行统计分析，TestItem.Statistic 为空)");

            dataPrism.Dispose();
        }
    }
}
