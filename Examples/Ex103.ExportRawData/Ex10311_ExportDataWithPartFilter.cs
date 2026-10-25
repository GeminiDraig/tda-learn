using System.Text.RegularExpressions;
using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;
using static TDA.DataPrism;

namespace Ex103.ExportRawData
{
    /// <summary>
    /// 通过 <see cref="PartFilter"/> 委托实现多种芯片级数据筛选，并将筛选结果导出为 NEDA 布局 CSV。
    /// 本范例内置 16 种筛选器：全部 / SoftBin / HardBin / XY 坐标 / 首个失败测试项 / Pass-Fail /
    /// Site / 数据来源 / 测试轮次 / 坐标区域 / 测试时间 / PartID / 抽样 / 超出规格（Out of Spec, OOS）/
    /// FirstFail TestText 正则 / 按测试值范围（TestID 实测值落在指定区间才保留）。
    /// 需要切换筛选方式时，只需修改 <see cref="DataPrism.ExportRawDatas"/> 调用中传入的委托即可。
    /// <para>
    /// 所有筛选器的委托签名均为 <c>(int partIndex, DataPrism dp)</c>：
    /// <c>partIndex</c> 是当前 Part 在当前 DataPrism 内的索引（与测试值数组对齐），
    /// <c>dp</c> 是当前 Part 所属的 DataPrism。经 <see cref="DataPrism.PartsDescription"/> 索引器
    /// 读取该 Part 的元数据（SoftBin/HardBin/Site/Pass-Fail/坐标等），经
    /// <see cref="DataPrism.TryGetValue(int, TestNum, out float)"/> 系列方法直接读取该 Part 的测试值，
    /// 因此"根据某个参数的测试值范围决定 Part 去留"这类需求无需在委托外捕获数据引用
    /// （旧写法用静态字段缓存 <c>dataPrisms[0].TestItems</c>，在多 Lot 场景下是错误的）。
    /// </para>
    /// </summary>
    public static class Ex10311_ExportDataWithPartFilter
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 通过 GUI 对话框同时选中多个 Lot ────────────
            var dataPrisms = TDAFileDialog.OpenDataPrisms(multiBrowse: true);
            if (dataPrisms is null || dataPrisms.Count == 0)
            {
                Msg.Debug("No valid file selected");
                return;
            }

            // ── Step 2: 逐个 Lot 加载并解析 STDF 文件 ─────────────────
            foreach (var dp in dataPrisms)
            {
                dp.Load();
                //dp.DoStatisticAnalysis();
                //只需要获取raw data的话，完全没必要执行DoStatisticAnalysis这个步骤
            }

            // ── Step 3: 选择 PartFilter 并导出 NEDA 布局 ─────────────
            //  按需从下面 16 个筛选器中选一个传入 partFilter 参数（想换哪种筛选就改这一行）：
            //    PartFilterAll             → 全部导出
            //    SoftBinFilter             → 仅导出 TargetSoftBins 指定的 SoftBin
            //    HardBinFilter             → 仅导出 TargetHardBins 指定的 HardBin
            //    XYFilter                  → 仅导出 TargetXYs 指定的 X/Y 坐标
            //    FirstFailNumFilter        → 仅导出 TargetFirstFailNums 指定的首败测试项
            //    FirstFailTextMatchFilter  → 仅导出首败项 TestText 匹配正则的芯片
            //    PassFailFilter            → 仅导出通过（Pass）芯片
            //    SiteFilter                → 仅导出 TargetSites 指定的 Site
            //    DataSourceFilter          → 仅导出 TargetDataSourceIDs 指定的数据来源
            //    CenterDieFilter           → 仅导出晶圆中心区域（X/Y 区间）的芯片
            //    SampleFilter              → 按 PartIndex 每隔 SampleStep 个取 1 个（抽样）
            //    OutOfSpecFilter           → 仅导出某测试项值超出规格上下限（Out of Spec）的芯片
            //    TestValueRangeFilter      → 仅导出某测试项（TargetTestNumberForValueRange）实测值落在指定区间的芯片

            var dataFilePath = DataPrism.ExportRawDatas(
                dataPrisms,
                testIDSet: null,
                isFinalDataOnly: true,
                layout: RawDataLayout.NEDA,
                partFilter: SoftBinFilter); // ← 切换筛选器：修改这里传入的委托即可

            Console.WriteLine($"Filtered data exported to: {dataFilePath}");

            // ── Step 4: 释放资源 ────────────────────────────────
            foreach (var dp in dataPrisms)
                dp.Dispose();
        }

        #region ── PartFilter 定义（在此处修改过滤条件）──────────

        /// <summary>
        /// 目标 SoftBin 编号集合，由 <see cref="SoftBinFilter"/> 使用。
        /// 示例：
        ///   <code>new HashSet&lt;ushort&gt; { 6142 }</code>         — 仅导出 SoftBin 6142
        ///   <code>new HashSet&lt;ushort&gt; { 6112, 6113, 6142 }</code> — 导出多个 SoftBin
        /// </summary>
        public static readonly HashSet<ushort> TargetSoftBins = new()
        {
            6112,
            6113,
            6142,
        };

        /// <summary>
        /// 目标 HardBin 编号集合，由 <see cref="HardBinFilter"/> 使用。
        /// </summary>
        public static readonly HashSet<ushort> TargetHardBins = new()
        {
            1,   // 例如：良品 HardBin
            2,   // 例如：某类失效 HardBin
        };

        /// <summary>
        /// 目标 XY 坐标集合，由 <see cref="XYFilter"/> 使用。
        /// X / Y 为晶圆坐标（die 列号 / 行号），常用于 CP 晶圆图上定点挑选指定 die。
        /// </summary>
        public static readonly HashSet<(short X, short Y)> TargetXYs = new()
        {
            (2, 31),//1
            (1, 33),//2
            (9, 37),//3
            (3, 38),//4
            (9, 20),//5
            (8, 26)//6
        };

        /// <summary>
        /// 目标首个失败测试项编号集合，由 <see cref="FirstFailNumFilter"/> 使用。
        /// 注意：通过（Pass）的芯片其 <see cref="PartInfo.FirstFailTestNum"/> = <see cref="CONST.TESTNUM4PASS"/> (=1)，
        /// 如需连同 Pass 芯片一起导出，请把 <c>CONST.TESTNUM4PASS</c> 加入此集合。
        /// </summary>
        public static readonly HashSet<uint> TargetFirstFailNums = new()
        {
            101,   // 首个失败项为 TEST_NUM 101 的芯片
            203,   // 首个失败项为 TEST_NUM 203 的芯片
            //CONST.TESTNUM4PASS, // 取消注释可同时导出所有 Pass 芯片
        };

        /// <summary>
        /// 目标 Site 编号集合，由 <see cref="SiteFilter"/> 使用（1-based）。
        /// </summary>
        public static readonly HashSet<byte> TargetSites = new()
        {
            1,   // 例如：只关注 Site 1
            2,   // 例如：Site 2 疑似有探针接触问题
        };

        /// <summary>
        /// 目标数据来源编号集合，由 <see cref="DataSourceFilter"/> 使用。
        /// DataSourceInfoID 对应多源导入（如 Split Lot）时每个数据源的索引，0 表示第一个数据源。
        /// </summary>
        public static readonly HashSet<int> TargetDataSourceIDs = new()
        {
            0,   // 例如：只导出第一个数据源（原 Lot）的数据
        };

        /// <summary>
        /// 抽样步长，由 <see cref="SampleFilter"/> 使用（PartIndex % SampleStep == 0 时保留）。
        /// 示例：SampleStep = 10 表示每 10 颗取 1 颗（约 10% 抽样）。
        /// </summary>
        public static readonly int SampleStep = 10;

        /// <summary>
        /// 超出规格（Out of Spec, OOS）判定的目标测试项编号，由 <see cref="OutOfSpecFilter"/> 使用。
        /// 取该测试项首次执行的 TestID（TestSequence = 0），比对每颗芯片的实测值与上下限。
        /// </summary>
        public static readonly uint OutOfSpecTestNumber = 8001;

        /// <summary>
        /// 按测试值范围筛选的目标测试项编号，由 <see cref="TestValueRangeFilter"/> 使用。
        /// 例如：VDD1 工作电流测试项的 Test Number。
        /// </summary>
        public static readonly uint TargetTestNumberForValueRange = 1000;

        /// <summary>
        /// 测试值保留区间 [下界, 上界]，由 <see cref="TestValueRangeFilter"/> 使用。
        /// 实测值落在区间内（含边界）的芯片才被保留。
        /// </summary>
        public static readonly (float Min, float Max) TargetValueRange = (1.0f, 3.5f);

        /// <summary>
        /// [PartFilter 示例] 筛选器：导出全部芯片（默认，不筛选）。
        /// </summary>
        /// <param name="partIndex">当前 Part 在当前 DataPrism 内的索引（0-based）。</param>
        /// <param name="dp">当前 Part 所属的 DataPrism。</param>
        /// <returns>true 导出该芯片；false 跳过</returns>
        public static bool PartFilterAll(int partIndex, DataPrism dp) => true;

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出 SoftBin 属于 <see cref="TargetSoftBins"/> 的芯片。
        /// </summary>
        public static bool SoftBinFilter(int partIndex, DataPrism dp)
        {
            return TargetSoftBins.Contains(dp.PartsDescription[partIndex].SoftBinNumber);
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出 HardBin 属于 <see cref="TargetHardBins"/> 的芯片。
        /// </summary>
        public static bool HardBinFilter(int partIndex, DataPrism dp)
        {
            return TargetHardBins.Contains(dp.PartsDescription[partIndex].HardBinNumber);
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出 X/Y 坐标属于 <see cref="TargetXYs"/> 的芯片。
        /// </summary>
        public static bool XYFilter(int partIndex, DataPrism dp)
        {
            var partInfo = dp.PartsDescription[partIndex];
            return TargetXYs.Contains((partInfo.X, partInfo.Y));
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出首个失败测试项属于 <see cref="TargetFirstFailNums"/> 的芯片。
        /// </summary>
        public static bool FirstFailNumFilter(int partIndex, DataPrism dp)
        {
            return TargetFirstFailNums.Contains(dp.PartsDescription[partIndex].FirstFailTestNum);
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出首败测试项（FirstFail）的 TestText 匹配正则的芯片。
        /// 首败项决定了芯片的失效模式，其 TestText 即 STDF 的 TEST_TXT。
        /// 实现参考 NEDA 导出中 CHIP_TEXT 列的逻辑：失败芯片取
        /// <see cref="TestItemDictionary.GetTestIDs"/> 返回列表的首条 TestID 的 TestText。
        /// 典型场景：按失效模式关键词（如 "PMU.*Trim.*V.*"）圈出某一类失效的芯片做专项分析。
        /// </summary>
        public static bool FirstFailTextMatchFilter(int partIndex, DataPrism dp)
        {
            var partInfo = dp.PartsDescription[partIndex];
            if (partInfo.IsPass) return false;   // 通过芯片无 FirstFail

            var tidList = dp.TestItems.GetTestIDs(partInfo.FirstFailTestNum);
            var ffText = tidList is { Count: > 0 } ? tidList[0].TestText : string.Empty;
            return Regex.IsMatch(ffText, @"PMU.*Trim.*V.*");
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出通过（Pass）的芯片。
        /// 结合 <see cref="PartInfo.IsPass"/> 取反即可导出全部失效芯片，
        /// 常用于分析失效分布、统计各类失效模式。
        /// </summary>
        public static bool PassFailFilter(int partIndex, DataPrism dp)
        {
            return dp.PartsDescription[partIndex].IsPass;
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出 Site 属于 <see cref="TargetSites"/> 的芯片。
        /// 典型场景：怀疑某个 Site 探针接触不良导致系统性失效时，单独圈出该 Site 分析。
        /// </summary>
        public static bool SiteFilter(int partIndex, DataPrism dp)
        {
            return TargetSites.Contains(dp.PartsDescription[partIndex].SiteNumber);
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出数据来源属于 <see cref="TargetDataSourceIDs"/> 的芯片。
        /// 典型场景：Split Lot / 多源合并数据中，只想分析某一个数据源（某台测试机 / 某次导入）。
        /// </summary>
        public static bool DataSourceFilter(int partIndex, DataPrism dp)
        {
            return TargetDataSourceIDs.Contains(dp.PartsDescription[partIndex].DataSourceInfoID);
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出晶圆中心区域（X ∈ [2,8]，Y ∈ [25,40]）的芯片。
        /// 典型场景：边缘区域往往存在涂胶不均 / 工艺边缘效应，用中心区域做"干净"样本对比。
        /// </summary>
        public static bool CenterDieFilter(int partIndex, DataPrism dp)
        {
            var partInfo = dp.PartsDescription[partIndex];
            return partInfo.X >= 2 && partInfo.X <= 8
                && partInfo.Y >= 25 && partInfo.Y <= 40;
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：按 PartIndex 抽样，每隔 <see cref="SampleStep"/> 颗取 1 颗。
        /// 典型场景：数据量过大时先抽样快速摸底（趋势 / 良率粗估），确认方向后再全量分析。
        /// </summary>
        public static bool SampleFilter(int partIndex, DataPrism dp)
        {
            return partIndex % SampleStep == 0;
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：仅导出指定测试项（<see cref="OutOfSpecTestNumber"/>）实测值超出规格上下限的芯片。
        /// 超出规格（Out of Spec, 简称 OOS）即实测值越出 LowLimit / HighLimit 定义的规格边界。
        /// 读取该测试项每颗芯片的实测值，与 STDF 中记录的 <see cref="TestItemInfos.LowLimit"/>/<see cref="TestItemInfos.HighLimit"/> 比对；
        /// 未设置限值（null）的测试项不参与过滤。
        /// 典型场景：某个关键参数漂移（如电流/电压项超出规格），定位到具体芯片做失效分析。
        /// </summary>
        public static bool OutOfSpecFilter(int partIndex, DataPrism dp)
        {
            // 按 TestNumber 取出该测试项（同一 TestNumber 可能有多次执行，取首次）
            if (!dp.TestItems.GetTestID(OutOfSpecTestNumber, out TestID tid)) return false;
            var item = dp.TestItems[tid];
            if (!item.TryGetValue(partIndex, out float value)) return false;

            double lsl = item.Info.LowLimit ?? double.NegativeInfinity;
            double usl = item.Info.HighLimit ?? double.PositiveInfinity;
            return value < lsl || value > usl;
        }

        /// <summary>
        /// [PartFilter 示例] 筛选器：按测试值范围筛选——仅导出指定测试项
        /// （<see cref="TargetTestNumberForValueRange"/>）实测值落在 <see cref="TargetValueRange"/> 区间内的芯片。
        /// 这是 <see cref="PartFilter"/> 的核心价值：无需在委托外捕获数据引用，
        /// 直接经 <see cref="DataPrism.TryGetValue(int, TestNum, out float)"/> 读取该 Part 的测试值。
        /// 典型场景：某关键参数（如工作电流）实测值必须在规格范围内才保留该芯片做后续分析；
        /// 也可结合多个 TryGetValue 实现"多参数联合条件"筛选。
        /// </summary>
        public static bool TestValueRangeFilter(int partIndex, DataPrism dp)
        {
            // 该测试项未测 / 不存在 → 不保留
            if (!dp.TryGetValue(partIndex, TargetTestNumberForValueRange, out float value)) return false;
            return value >= TargetValueRange.Min && value <= TargetValueRange.Max;
        }

        #endregion
    }
}
