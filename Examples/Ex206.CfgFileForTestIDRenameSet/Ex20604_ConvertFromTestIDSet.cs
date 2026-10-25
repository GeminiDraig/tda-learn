using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDRenameSet
{
    /// <summary>
    /// 从现有的 TestIDSet 升级为 TestIDRenameSet（无损转换，可选字段均为 null），
    /// 再选择性补充 REN 重命名映射，导出为新的 CSV。
    /// 适用于已有 TestIDSet 配置、需要扩展 rename 功能的场景。
    /// </summary>
    public static class Ex20604_ConvertFromTestIDSet
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 加载已有的 TestIDSet CSV ───────────────────────
            var csvPath = TDAFileDialog.SelectFile(FileType.csv);
            if (csvPath is null) return;
            var tidSet = TestIDSet.ImportFromFile(csvPath);
            if (tidSet is null)
            {
                Msg.Warning("未选择 TestIDSet 文件或加载失败！");
                return;
            }

            // ── Step 2: 升级为 TestIDRenameSet（可选字段全为 null）──────
            var renameSet = new TestIDRenameSet(tidSet);
            Msg.Information($"从 TestIDSet 升级，共 {renameSet.Count} 条记录");

            // ── Step 3: 批量补充 REN 映射（示例：RF 类参数编号整体平移到新号段）──
            foreach (var entry in renameSet)
            {
                if (entry.TestID.TestText.Contains("RF", StringComparison.OrdinalIgnoreCase))
                    entry.TestID_REN = new TestID(entry.TestID.TestNumber + 5000000, entry.TestID.TestText);
            }

            // ── Step 4: 导出升级后的 CSV ───────────────────────────────
            var outCsvpath = csvPath + "_UpgradedToRenameSet.csv";
            renameSet.ExportToFile(outCsvpath);
            Msg.Information($"已升级的 TestIDRenameSet 导出至: {outCsvpath}");

            // 同时演示降级回 TestIDSet
            var tidSetBack = renameSet.ToTestIDSet();
            Msg.Information($"降级回 TestIDSet: {tidSetBack.Count} 条（REN 字段已丢弃）");
        }
    }
}
