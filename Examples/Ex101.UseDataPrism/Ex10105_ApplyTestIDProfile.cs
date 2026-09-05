using TDA;
using TDA.FileIO;
using TDA.TypeDef;

namespace Ex101.UseDataPrism
{
    /// <summary>
    /// 演示 ApplyTestIDProfiles 的 TestID 过滤与重命名功能。
    /// 流程：弹窗选文件 → Load → DoStatisticAnalysis
    ///       → 打印原始 TestID 数量和统计
    ///       → 构建 TestIDProfileSet（选取前 3 个参数测试项并重命名）
    ///       → ApplyTestIDProfiles
    ///       → 打印过滤/重命名后的 TestID 列表
    ///
    /// 关键点：ApplyTestIDProfiles 不是必须步骤，仅在需要过滤特定 TestID
    ///         或进行跨程序 TestID 重命名时才调用。
    /// </summary>
    public static class Ex10105_ApplyTestIDProfile
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

            // ── Step 2: 打印原始 TestID 列表 ────────────────────────
            Console.WriteLine("══════ ApplyTestIDProfiles 前 ══════");
            Console.WriteLine($"  TestID 总数: {dataPrism.TestItems.Count}");
            Console.WriteLine("  TestID 列表 (前 10 个):");
            int count = 0;
            foreach (var kvp in dataPrism.TestItems)
            {
                if (count++ >= 10)
                    break;
                TestID tid = kvp.Key;
                Console.WriteLine($"    T{tid.TestNumber}  [{tid.TestText}]  " +
                    $"{(kvp.Value.IsParametric ? "参数" : "功能")}");
            }
            if (dataPrism.TestItems.Count > 10)
                Console.WriteLine($"    ... 等共 {dataPrism.TestItems.Count} 个");
            Console.WriteLine();

            // ── Step 3: 选出前 3 个参数测试项构建 TestIDProfileSet ──
            var profiles = new List<TestIDProfile>();
            int picked = 0;
            foreach (var kvp in dataPrism.TestItems)
            {
                if (!kvp.Value.IsParametric)
                    continue;

                TestID tid = kvp.Key;
                profiles.Add(new TestIDProfile
                {
                    TestID = tid,
                    Tag = "selected",
                    // 重命名：T{tid} → T{tid + 1000}
                    TestID_REN = new TestID(tid.TestNumber + 1000, tid.TestText),
                });

                if (++picked >= 3)
                    break;
            }

            if (profiles.Count == 0)
            {
                Console.WriteLine("未找到参数测试项，无法演示 ApplyTestIDProfile。");
                dataPrism.Dispose();
                return;
            }

            var profileSet = new TestIDProfileSet(profiles);
            Console.WriteLine($"  构建 TestIDProfileSet: {profiles.Count} 个 Profile");
            foreach (var p in profiles)
                Console.WriteLine($"    T{p.TestID.TestNumber} → " +
                    $"T{p.TestID_REN!.Value.TestNumber}  (Tag={p.Tag})");
            Console.WriteLine();

            // ── Step 4: ApplyTestIDProfiles ─────────────────────────
            Console.WriteLine("  执行 ApplyTestIDProfiles(profileSet)...");
            dataPrism.ApplyTestIDProfiles(profileSet);

            // ── Step 5: 打印过滤/重命名后的 TestID 列表 ────────────
            Console.WriteLine();
            Console.WriteLine("══════ ApplyTestIDProfiles 后 ══════");
            Console.WriteLine($"  TestID 总数: {dataPrism.TestItems.Count}");
            Console.WriteLine("  TestID 列表:");
            foreach (var kvp in dataPrism.TestItems)
            {
                TestID tid = kvp.Key;
                Console.WriteLine($"    T{tid.TestNumber}  [{tid.TestText}]  " +
                    $"{(kvp.Value.IsParametric ? "参数" : "功能")}");

                // 打印过滤后的统计量
                var stat = kvp.Value.Statistic;
                if (kvp.Value.IsParametric)
                    Console.WriteLine($"      Stat -> Mean={stat.MeanValue:F4}  " +
                        $"Sigma={stat.Sigma:F4}  Cpk={stat.Cpk:F3}");
            }

            Console.WriteLine();
            Console.WriteLine("  可以看到：");
            Console.WriteLine("  1) 不在 Profile 中的 TestID 已被移除；");
            Console.WriteLine("  2) 保留的 TestID 已按 TestID_REN 配置重命名（T+1000）；");
            Console.WriteLine("  3) 若 DataPrismSetup.DiscardUnprofiledTestIDs=false（默认），");
            Console.WriteLine("     未在 Profile 中的 TestID 在 ApplyTestIDProfiles 后才会被移除。");

            dataPrism.Dispose();
        }
    }
}
