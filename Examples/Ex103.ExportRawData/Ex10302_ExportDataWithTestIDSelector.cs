using System.Text.RegularExpressions;
using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;
using static TDA.DataPrism;

namespace Ex103.ExportRawData
{
    /// <summary>
    /// 从多个 STDF 文件导出 Final Data（仅最终测试结果）到 CSV。
    /// 通过 GUI 文件对话框选择 STDF 文件，加载数据后使用 TestIDSelector 委托动态过滤 TestID。
    /// </summary>
    public static class Ex10302_ExportDataWithTestIDSelector
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 浏览并选择多个 STDF 文件 ────────────────────────
            var dataPrisms = TDAFileDialog.OpenDataPrisms(multiBrowse: true);
            if (dataPrisms is null)
            {
                Msg.Debug("No valid file selected");
                return;
            }

            // ── Step 2: 逐个加载并解析 STDF 文件 ────────────────────────
            foreach (var dataprism in dataPrisms)
            {
                dataprism.Load();
                //dataprism.DoStatisticAnalysis();
                //只需要获取raw data的话，完全没必要执行DoStatisticAnalysis这个步骤
            }

            // ── Step 3: 使用显式 TestIDSelector 导出 Final Data ──────────
            //   按需从下面 8 个选择器中选一个传入 testIDSelector 参数（想换哪种筛选就改这一行）：
            //     SelectTestIDsByText                      → 按 TestText 关键字匹配（GUI 加载的 TestID 列表）
            //     SelectTestIDsByTestNumber                → 按 TestNumber 精确匹配（GUI 加载的 TestID 列表）
            //     SelectTestIDsByTextKeywordsOrNumberRange → 按 TestText 关键字 + TestNumber 区间（硬编码）
            //     SelectTestIDsByRegex                     → 按 TestText 正则匹配（硬编码）
            //     SelectTestIDsWithFailures                → 仅导出存在 Fail 记录的测试项
            //     SelectCommonTestIDs                      → 跨批次取交集
            //     SelectTestIDsByTestSequence              → 按 TestSequence 过滤（0 = 首次执行）
            var dataFilePath = ExportRawDatas(dataPrisms,
                testIDSelector: SelectTestIDsByText,
                isFinalDataOnly: true,
                layout: RawDataLayout.NEDA);

            Console.WriteLine($"Raw data Exported to {dataFilePath} successfully!");
        }

        #region ── TestIDSelector 定义（在此处修改 TestID 选择逻辑）──────────

        /// <summary>
        /// 显式的 TestIDSelector 实现 —— 按 TestText 关键字过滤 TestID。
        /// prelimilaryTestIdList中的TestText是有效的，但是由于程序升级testnumber已经变了，完全按照之前的TestID匹配不上。
        /// 这里进行一次By TestText筛选，只要满足prelimilaryTestIdList中TestText匹配即可
        /// </summary>
        public static TestIDSet? SelectTestIDsByText(IEnumerable<DataPrism> dataPrisms)
        {
            TestIDSet? prelimilaryTestIdList = TDAFileDialog.OpenTestIDSet();
            if (prelimilaryTestIdList == null) return null;

            var set = new TestIDSet();
            foreach (var dprism in dataPrisms)
                foreach (var tid in dprism.TestItems.Keys)
                    if (prelimilaryTestIdList.Contains(tid.TestText))
                        set.Add(tid);
            return set;
        }

        /// <summary>
        /// 按 TestNumber 精确匹配过滤 TestID。
        /// 通过 GUI 对话框加载预先准备的 TestID 列表文件（CSV），
        /// 仅保留 dataPrisms 中 TestNumber 与该列表匹配的 TestID。
        /// 适用于：TestNumber 在跨程序间保持稳定的场景。
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>筛选后的 TestIDSet；用户取消文件选择时返回 null（表示导出全部）。</returns>
        public static TestIDSet? SelectTestIDsByTestNumber(IEnumerable<DataPrism> dataPrisms)
        {
            TestIDSet? prelimilaryTestIdList = TDAFileDialog.OpenTestIDSet();
            if (prelimilaryTestIdList == null) return null;

            var set = new TestIDSet();
            foreach (var dprism in dataPrisms)
                foreach (var tid in dprism.TestItems.Keys)
                    if (prelimilaryTestIdList.Contains(tid.TestNumber))
                        set.Add(tid);
            return set;
        }

        /// <summary>
        /// 按「TestText 关键字 + TestNumber 区间」组合规则过滤 TestID（硬编码示例）。
        /// 满足以下任一条件即保留：
        /// <list type="bullet">
        ///   <item>TestText 包含 "VPretrim"（Trim 前测试）</item>
        ///   <item>TestText 包含 "VPosttrim"（Trim 后测试）</item>
        ///   <item>TestNumber 落在 [600000, 699999] 区间（示例：模拟类测试编号段）</item>
        /// </list>
        /// 典型场景：测试程序升级后 TestNumber 变更但 TestText 语义保留时，
        /// 用关键字兜底匹配；已知某编号段固定承载特定类型测试时用区间覆盖。
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>筛选后的 TestIDSet（永不返回 null）。</returns>
        public static TestIDSet? SelectTestIDsByTextKeywordsOrNumberRange(IEnumerable<DataPrism> dataPrisms)
        {
            var set = new TestIDSet();
            foreach (var dprism in dataPrisms)
                foreach (var tid in dprism.TestItems.Keys)
                    if (tid.TestText.Contains("VPretrim")
                       || tid.TestText.Contains("VPosttrim")
                       || (600000 <= tid.TestNumber && tid.TestNumber <= 699999))
                        set.Add(tid);
            return set;
        }

        /// <summary>
        /// [TestIDSelector 示例] 按 TestText 正则表达式过滤 TestID（硬编码示例）。
        /// 正则能表达关键字 Contains 难以描述的复杂模式，例如：
        ///   <c>@"^PMU"</c>                — TestText 以 "PMU" 开头
        ///   <c>@"Trim.*V\d"</c>          — 含 "Trim"，其后任意字符再跟 "V" + 数字
        ///   <c>@"(VPretrim|VPosttrim)"</c> — 关键字多选一
        /// 典型场景：测试项命名规范统一，按命名模式批量圈选同类测试项（模拟/数字/电源项等）。
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>筛选后的 TestIDSet（永不返回 null）。</returns>
        public static TestIDSet? SelectTestIDsByRegex(IEnumerable<DataPrism> dataPrisms)
        {
            var set = new TestIDSet();
            foreach (var dprism in dataPrisms)
                foreach (var tid in dprism.TestItems.Keys)
                    if (Regex.IsMatch(tid.TestText, @"^PMU.*"))
                        set.Add(tid);
            return set;
        }

        /// <summary>
        /// [TestIDSelector 示例] 仅保留存在 FAIL 记录的测试项。
        /// 遍历所有批次的数据，只要某个测试项在任一颗芯片上 Fail 过（<see cref="TestItem.FailCount"/> &gt; 0）即保留。
        /// 典型场景：只关心出问题（有坏片）的测试项，便于快速定位失效源；配合导出结果做根因分析。
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>筛选后的 TestIDSet；无任何 Fail 项时返回 null（表示导出全部）。</returns>
        public static TestIDSet? SelectTestIDsWithFailures(IEnumerable<DataPrism> dataPrisms)
        {
            var set = new TestIDSet();
            foreach (var dprism in dataPrisms)
                foreach (var kvp in dprism.TestItems)
                    if (kvp.Value.FailCount > 0)
                        set.Add(kvp.Key);
            return set.Count == 0 ? null : set;
        }

        /// <summary>
        /// [TestIDSelector 示例] 跨批次取交集：只保留所有批次共有的 TestID。
        /// 典型场景：对比多个 Lot / 多个程序版本时，只导出大家都测过的测试项，
        /// 保证各批次导出的列完全一致，便于横向对比。
        /// <para><b>顺序行为</b>：以首个批次的 TestID 解析顺序为基准（<see cref="TestIDSet"/> 构造时保留
        /// <see cref="TestItemDictionary.Keys"/> 的顺序），后续 <see cref="TestIDSet.IntersectWith"/> 均为纯过滤，
        /// 保留元素维持该顺序——最终导出列的顺序 = 首个批次中所有批次共有项的解析顺序。</para>
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>筛选后的 TestIDSet；无公共项或集合为空时返回 null（表示导出全部）。</returns>
        public static TestIDSet? SelectCommonTestIDs(IEnumerable<DataPrism> dataPrisms)
        {
            var first = dataPrisms.FirstOrDefault();
            if (first is null) return null;

            // 以首个批次 Keys（有序）为基准，逐个与其余批次求交集；
            // IntersectWith 是纯过滤，保留元素维持当前集合原顺序，结果即首批次中的共有项顺序。
            var common = new TestIDSet(first.TestItems.Keys);
            foreach (var dp in dataPrisms.Skip(1))
                common.IntersectWith(dp.TestItems.Keys);

            return common.Count == 0 ? null : common;
        }

        /// <summary>
        /// [TestIDSelector 示例] 按 TestSequence 过滤 TestID。
        /// TestSequence = 0 表示该参数在同一次 testflow 内的首次执行；
        /// =1,2,... 表示同一 TestNumber+TestText 参数被重复测试的第 N 次（复测）。
        /// 典型场景：只导出首次执行结果，忽略程序内的重复测试项；
        /// 或反过来只挑出复测项（<c>TestSequence &gt; 0</c>）单独分析。
        /// </summary>
        /// <param name="dataPrisms">已 Load 的 DataPrism 集合。</param>
        /// <returns>筛选后的 TestIDSet（永不返回 null）。</returns>
        public static TestIDSet? SelectTestIDsByTestSequence(IEnumerable<DataPrism> dataPrisms)
        {
            var set = new TestIDSet();
            foreach (var dprism in dataPrisms)
                foreach (var tid in dprism.TestItems.Keys)
                    if (tid.TestSequence == 0)
                        set.Add(tid);
            return set;
        }

        #endregion
    }
}
