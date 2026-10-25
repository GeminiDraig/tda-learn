using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDRenameSet
{
    /// <summary>
    /// 从 STDF 文件自动生成 TestIDRenameSet 并保存为 CSV。
    /// 流程：打开 STDF → DataPrism 提取 TestItems → 补充 REN 重命名映射 → 导出 CSV
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

            // ── Step 2: 将 TestItems 转换为 TestIDRenameSet ───────────
            var tidRenameSet = new TestIDRenameSet(dataprism.TestItems.Keys);

            // ── Step 3: 手动为需要跨程序对齐的 TestID 补充 REN 映射 ──
            // 示例：为部分参数设置重命名（实际使用时按新旧程序的 TestID 对照表赋值）
            foreach (var entry in tidRenameSet)
            {
                if (Str.ContainsAny(entry.TestID.TestText, "PUVM", "PMU"))
                    entry.TestID_REN = new TestID(entry.TestID.TestNumber + 1000, entry.TestID.TestText);
            }

            // ── Step 4: 导出 CSV ───────────────────────────────────
            // 默认输出位置：优先 DataPrismSetup 配置文件所在目录（若通过 .dp.json 加载），否则回退到首个数据文件目录
            var csvpath = dataprism.ResolveDefaultOutputPath("_TestIDRenameSet.csv");
            tidRenameSet.ExportToFile(csvpath);
            Msg.Information($"TestIDRenameSet 已保存至: {csvpath}");
        }
    }
}
