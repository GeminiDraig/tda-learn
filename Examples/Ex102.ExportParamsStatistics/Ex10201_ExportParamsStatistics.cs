using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex102.ExportStdfSummaryToFile
{
    /// <summary>
    /// 导出 STDF 测试参数的统计摘要到 CSV 文件（宽格式）。
    /// 支持多文件批量处理、TestIDSet 筛选，导出逻辑委托给
    /// <see cref="DataPrism.ExportStatistics(IEnumerable{DataPrism}, TestIDSet?, string?)"/>。
    /// </summary>
    public static class Ex10201_ExportParamsStatistics
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 浏览并获取 DataPrismSource 文件列表 ────────────────────────
            List<string>? stdfFileList = TDAFileDialog.SelectFiles(FileType.DataPrismSource)?.ToList();
            if (stdfFileList is null || stdfFileList.Count == 0) 
            {
                Msg.Warning("No valid files selected.");
                return;
            }

            // ── Step 2: 确定输出文件路径 ─────────────────────────────────
            string dir = Path.GetDirectoryName(stdfFileList[0]) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var csvPath = Path.Combine(dir, "TestItemsSummaries.csv");
            Msg.Information($"Job start under directory {dir} ....");

            // ── Step 3: 获取 TestIDSet 筛选配置文件 ──────────────────────
            TestIDSet? tstIdList = TDAFileDialog.OpenTestIDSet();

            // ── Step 4: 打开并解析 STDF 文件 ────────────────────────────
            var dpList = new List<DataPrism>();
            foreach (string filePath in stdfFileList)
            {
                var dp = DataPrism.FromFile(filePath);
                if (dp is null)
                {
                    Msg.Warning($"Skipped invalid file: {filePath}");
                    continue;
                }

                dp.Load();
                dp.DoStatisticAnalysis();
                Msg.Information($"Statistic done @ {Path.GetFileName(filePath)} ....");
                dpList.Add(dp);
            }

            // ── Step 5: 调用 DataPrism 标准导出接口 ─────────────────────
            string actualPath = DataPrism.ExportStatistics(dpList, tstIdList, csvPath);
            Msg.Information($"Statistics exported to: {actualPath}");

            // 释放资源
            foreach (var dp in dpList)
                dp.Dispose();
        }
    }
}
