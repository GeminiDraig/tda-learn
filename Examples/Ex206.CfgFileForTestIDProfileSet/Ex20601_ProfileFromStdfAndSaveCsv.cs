using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDProfileSet
{
    /// <summary>
    /// 从 STDF 文件自动生成 TestIDProfileSet 并保存为 CSV。
    /// 流程：打开 STDF → DataPrism 提取 TestItems → 手动补充 Tag/Limits → 导出 CSV
    /// </summary>
    public static class Ex20601_ProfileFromStdfAndSaveCsv
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 打开 STDF，加载数据 ────────────────────────
            var dataprism = TDAFileDialog.OpenDataPrism();
            if (dataprism is null) return;

            dataprism.Load();
            dataprism.DoStatisticAnalysis();

            // ── Step 2: 将 TestItems 转换为 TestIDProfileSet ───────────
            var tidProfileSet = new TestIDProfileSet(dataprism.TestItems.Keys);

            // ── Step 3: 手动为感兴趣的 TestID 补充可选元数据 ──────
            // 示例：为部分参数打 Tag（实际使用时可根据业务逻辑批量赋值）
            foreach (var tidProfile in tidProfileSet)
            {
                if (Str.ContainsAny(tidProfile.TestID.TestText, "OSPS", "OS_VSS", "SPI_WRITE", "SPI_READ", "RunPattern"))
                    tidProfile.Tag = "Discard";
                else if (tidProfile.TestID.TestText.Contains("PUVM", StringComparison.OrdinalIgnoreCase))
                    tidProfile.Tag = "PUVM";
                else if (tidProfile.TestID.TestText.Contains("PMU", StringComparison.OrdinalIgnoreCase))
                    tidProfile.Tag = "PMU";
            }

            // ── Step 4: 导出 CSV ───────────────────────────────────
            // 默认输出位置：优先 DataPrismSetup 配置文件所在目录（若通过 .dp.json 加载），否则回退到首个数据文件目录
            var csvpath = dataprism.ResolveDefaultOutputPath("_TestIDProfileSet.csv");
            tidProfileSet.ExportToFile(csvpath);
            Msg.Information($"TestIDProfileSet 已保存至: {csvpath}");
        }
    }
}
