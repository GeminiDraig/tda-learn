using System.Text;
using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;
using static TDA.DataPrism;

namespace Ex103.ExportRawData
{
    /// <summary>
    /// 按晶圆坐标（X / Y）经 <see cref="DataPrism.DeviceID2PartIndexesMap"/> 直接反查指定 die，
    /// 配合 <see cref="TestIDSet"/> 圈选感兴趣参数，导出 EAV（Entity-Attribute-Value）长表 CSV。
    /// <para>
    /// <b>与 Ex10311 的区别（两者互补，按场景选用）：</b>
    /// <list type="bullet">
    ///   <item><b>定位方式</b>：Ex10311 用 <see cref="PartFilter"/> 遍历全部 Part 逐个比对坐标（O(N) 全表扫描）；
    ///         本范例用 DeviceID 字典做 O(1) 反查，适合「整片 wafer 几万颗 die、只挑几十颗」的场景。</item>
    ///   <item><b>输出布局</b>：Ex10311 是 NEDA 宽表（一行一颗 die，一列一个参数）；
    ///         本范例是 EAV 长表（一行一个 die×参数），适合直接喂给 Excel 透视表 / pandas 做参数间交叉比对。</item>
    ///   <item><b>参数圈选</b>：本范例由 TestIDSet CSV 显式指定感兴趣参数，并按 TestText 与实测数据对齐。</item>
    /// </list>
    /// </para>
    /// <para><b>前置条件：</b>DeviceID 必须以 X/Y 坐标构建——即 CP 数据的默认方式
    /// （<see cref="DeviceIDParseConfig.DeviceIDType"/> = <see cref="DeviceIDType.X_Y"/>）。
    /// 若数据是 FT（无坐标）或 DeviceIDType 被改成了 PartText / TestResults / NoDeviceID，
    /// 字典反查会全部落空——此时请改用 Ex10311 的 PartFilter（按 Part 元数据里的 X / Y 遍历比对）。
    /// </para>
    /// </summary>
    public static class Ex10312_ExportDataWithDeviceIDLookup
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 通过 GUI 对话框同时选中多个 Lot（约定：一个文件 = 一片 wafer）──────
            var dataPrisms = TDAFileDialog.OpenDataPrisms(multiBrowse: true);
            if (dataPrisms is null || dataPrisms.Count == 0)
            {
                Msg.Debug("No valid file selected");
                return;
            }

            // ── Step 2: 逐个 Lot 加载并解析 STDF 文件 ──────────────────────────
            //  本范例只读 raw data + Part 元数据，无需 DoStatisticAnalysis()（统计量用不上）
            foreach (var dp in dataPrisms)
            {
                dp.Load();
            }

            // ── Step 3: 圈选感兴趣参数（GUI 载入 TestIDSet CSV）──────────────────
            //  返回 null 表示用户取消选择 → 导出该 die 的全部参数
            TestIDSet? interestTestIDs = ResolveInterestTestIDs(dataPrisms);
            Console.WriteLine(interestTestIDs is null
                ? "No TestIDSet selected -> export ALL test items."
                : $"{interestTestIDs.Count} test items selected.");

            // ── Step 4: 组装目标 die 坐标集合 ────────────────────────────────
            //  离散坐标 ∪ 对角线区块展开结果
            var targets = new HashSet<(short X, short Y)>(TargetXYs);
            foreach (var xy in ExpandDiagonalBlocks(DiagonalBlocks))
                targets.Add(xy);
            Console.WriteLine($"Total target dies: {targets.Count}");

            // ── Step 5: 逐 wafer、逐坐标反查并写出长表 ────────────────────────
            string outputDir = Paths.TestDatasPath("Output", "Ex103.ExportRawData");
            Paths.CreateIfNotExist(outputDir);
            string outputPath = Path.Combine(outputDir, $"Ex10312_PickedData_{DateTime.Now:yyyyMMdd_HHmmss}.csv");

            int dieFound = 0, dieMissed = 0, rows = 0;

            using var writer = new StreamWriter(outputPath, false, Encoding.UTF8);
            writer.WriteLine("SourceFile,WaferId,Site,X,Y,TestText,TestNumber,Value,Unit");

            foreach (var dp in dataPrisms)
            {
                string sourceName = dp.DataSourcesInfoList.Count > 0
                    ? Path.GetFileName(dp.DataSourcesInfoList[0].FilePath)
                    : "(unknown)";
                string waferId = dp.DataSourcesInfoList.Count > 0
                    ? dp.DataSourcesInfoList[0].WaferId ?? "(unknown)"
                    : "(unknown)";

                foreach (var (x, y) in targets)
                {
                    // DeviceID 字典 O(1) 反查 —— 无需遍历 PartsDescription
                    var deviceId = new DeviceID(x, y);
                    if (!dp.DeviceID2PartIndexesMap.TryGetValue(deviceId, out var partIndexList))
                    {
                        // 该 wafer 上不存在此坐标（可能在晶圆图有效区之外 / DeviceID 解析方式不匹配）
                        dieMissed++;
                        continue;
                    }

                    // PartIndexList 按测试轮次追加：最后一个条目即该 die 的最终结果。
                    // CP 复测时早期轮次的 Part 已被标记 IsFinalData = false（见 DataPrism.Parse.cs）。
                    int partIndex = partIndexList.Last;
                    PartInfo partInfo = dp.PartsDescription[partIndex];
                    dieFound++;

                    IEnumerable<TestID> testIDs = dp.TestItems.Keys;
                    if (interestTestIDs is { Count: > 0 })
                        testIDs = interestTestIDs;

                    foreach (var tid in testIDs)
                    {
                        // DataPrism.TryGetValue：内部已处理「该测试项不存在」的情况
                        bool hasValue = dp.TryGetValue(partIndex, tid, out float value);
                        string unit = hasValue ? dp.TestItems[tid].Info.Units ?? string.Empty : string.Empty;

                        // 该 die 未测此参数时仍输出一行（Value = NaN），保证长表的 die×参数网格完整，
                        // 下游做透视表时不会因为缺行而错位。
                        writer.WriteLine(
                            $"{sourceName},{waferId},{partInfo.SiteNumber},{partInfo.X},{partInfo.Y}," +
                            $"\"{tid.TestText}\",{tid.TestNumber}," +
                            $"{(hasValue ? value.ToString("F6") : "NaN")},\"{unit}\"");
                        rows++;
                    }
                }
            }

            // ── Step 6: 释放资源 ────────────────────────────────────────
            foreach (var dp in dataPrisms)
                dp.Dispose();

            Console.WriteLine($"Done! Dies found: {dieFound}, missed: {dieMissed}, rows: {rows}");
            Console.WriteLine($"Picked data exported to: {outputPath}");
        }

        #region ── 目标坐标定义（在此处修改挑 die 规则）──────────────────

        /// <summary>
        /// 目标 die 的晶圆坐标集合：X = die 列号，Y = die 行号。
        /// 适用于需要精确点名的零散 die（例如失效分析挑片、corner lot 抽样）。
        /// </summary>
        public static readonly HashSet<(short X, short Y)> TargetXYs = new()
        {
            (75, 75),
            (76, 76),
            (77, 77),
        };

        /// <summary>
        /// 对角线区块定义：(StartX, StartY, Count) —— 从起点沿对角线取 Count 颗 die，
        /// 即第 i 颗坐标为 (StartX + i, StartY + i)。
        /// <para>
        /// 晶圆图上同一 block 内的 die 通常沿对角线排布（X / Y 同步递增），
        /// 用「区块」描述可以一行顶几十行坐标。
        /// </para>
        /// </summary>
        public static readonly (short StartX, short StartY, int Count)[] DiagonalBlocks =
        {
            (118, 76, 7),   // block 1：7 颗 die，(118,76) ~ (124,82)
            (147, 76, 7),   // block 2
            (75, 3, 8),     // block 3：8 颗 die，(75,3) ~ (82,10)
            (75, 39, 8),    // block 4
        };

        /// <summary>
        /// 将 <see cref="DiagonalBlocks"/> 展开为离散坐标集合。
        /// </summary>
        /// <param name="blocks">对角线区块定义。</param>
        /// <returns>展开后的 (X, Y) 坐标序列。</returns>
        public static IEnumerable<(short X, short Y)> ExpandDiagonalBlocks(
            IEnumerable<(short StartX, short StartY, int Count)> blocks)
        {
            foreach (var (startX, startY, count) in blocks)
                for (int i = 0; i < count; i++)
                    yield return ((short)(startX + i), (short)(startY + i));
        }

        #endregion

        #region ── 感兴趣参数定义（在此处修改圈选规则）────────────────

        /// <summary>
        /// 通过 GUI 载入预先准备的 TestIDSet 清单（CSV），并按 TestText 与实际数据对齐。
        /// <para>
        /// <b>为什么要对齐：</b>清单常来自旧程序版本的导出，其中的 TestNumber 可能已经变化，
        /// 直接用 TestNumber 匹配会全部落空；而 TestText 语义通常保持稳定，
        /// 因此这里以 TestText 为键，从当前数据里挑出真实存在的 TestID 实例。
        /// </para>
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>对齐后的 TestIDSet；用户取消选择时返回 null（表示导出全部参数）。</returns>
        public static TestIDSet? ResolveInterestTestIDs(IEnumerable<DataPrism> dataPrisms)
        {
            TestIDSet? preliminary = TDAFileDialog.OpenTestIDSet();
            if (preliminary is null) return null;

            var set = new TestIDSet();
            foreach (var dp in dataPrisms)
                foreach (var tid in dp.TestItems.Keys)
                    if (preliminary.Contains(tid.TestText))
                        set.Add(tid);
            return set;
        }

        #endregion
    }
}
