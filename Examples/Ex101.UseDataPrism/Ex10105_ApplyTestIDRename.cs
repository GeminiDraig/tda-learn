using TDA;
using TDA.FileIO;
using TDA.TypeDef;

namespace Ex101.UseDataPrism
{
    /// <summary>
    /// 演示 ApplyTestIDRenames 的 TestID 过滤与重命名功能。
    /// 流程：弹窗选文件 → Load → DoStatisticAnalysis
    ///       → 打印原始 TestID 数量和统计
    ///       → 构建 TestIDRenameSet（选取前 3 个参数测试项并重命名）
    ///       → ApplyTestIDRenames
    ///       → 打印过滤/重命名后的 TestID 列表
    ///
    /// 关键点：ApplyTestIDRenames 不是必须步骤，仅在需要过滤特定 TestID
    ///         或进行跨程序 TestID 重命名时才调用。
    /// </summary>
    public static class Ex10105_ApplyTestIDRename
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
            Console.WriteLine("══════ ApplyTestIDRenames 前 ══════");
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

            // ── Step 3: 选出前 3 个参数测试项构建 TestIDRenameSet ──
            var profiles = new List<TestIDRename>();
            int picked = 0;
            foreach (var kvp in dataPrism.TestItems)
            {
                if (!kvp.Value.IsParametric)
                    continue;

                TestID tid = kvp.Key;
                profiles.Add(new TestIDRename
                {
                    TestID = tid,
                    // 重命名：T{tid} → T{tid + 1000}
                    TestID_REN = new TestID(tid.TestNumber + 1000, tid.TestText),
                });

                if (++picked >= 3)
                    break;
            }

            if (profiles.Count == 0)
            {
                Console.WriteLine("未找到参数测试项，无法演示 ApplyTestIDRename。");
                dataPrism.Dispose();
                return;
            }

            var profileSet = new TestIDRenameSet(profiles);
            Console.WriteLine($"  构建 TestIDRenameSet: {profiles.Count} 个 Profile");
            foreach (var p in profiles)
                Console.WriteLine($"    T{p.TestID.TestNumber} → " +
                    $"T{p.TestID_REN!.Value.TestNumber}");
            Console.WriteLine();

            // ── Step 4: ApplyTestIDRenames ─────────────────────────
            // keepOnlyListed 参数默认 false → 未列入 TestIDRenameSet 的 TestID 全部保留（见结尾说明第 2 条）
            Console.WriteLine("  执行 ApplyTestIDRenames(profileSet)...  // keepOnlyListed 默认 false");
            dataPrism.ApplyTestIDRenames(profileSet);

            // ── Step 5: 打印过滤/重命名后的 TestID 列表 ────────────
            Console.WriteLine();
            Console.WriteLine("══════ ApplyTestIDRenames 后 ══════");
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
            Console.WriteLine("  1) TestIDRenameSet 中列出的 3 个 TestID 已按 TestID_REN 配置重命名（T+1000），");
            Console.WriteLine("     原 TestID 因数据全部搬走而被移除；");
            Console.WriteLine("  2) 其余未列入 TestIDRenameSet 的 TestID 保持原样（默认 KeepOnlyListedTestIDs=false 全部保留）；");
            Console.WriteLine("  3) 若 DataPrismSetup.KeepOnlyListedTestIDs=true，未列入 TestIDRenameSet 的 TestID");
            Console.WriteLine("     会在 ApplyTestIDRenames 后被移除并释放内存。");

            dataPrism.Dispose();
        }
    }
}
