using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDProfileSet
{
    /// <summary>
    /// 从现有的 TestIDSet 升级为 TestIDProfileSet（无损转换，可选字段均为 null），
    /// 再选择性补充 Tag/Limits，导出为新的 CSV。
    /// 适用于已有 TestIDSet 配置、需要扩展功能的场景。
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

            // ── Step 2: 升级为 TestIDProfileSet（可选字段全为 null）──────
            var profile = new TestIDProfileSet(tidSet);
            Msg.Information($"从 TestIDSet 升级，共 {profile.Count} 条记录");

            // ── Step 3: 批量设置 Tag（示例：按 TestText 关键字自动分类）──
            foreach (var entry in profile)
            {
                if (entry.TestID.TestText.Contains("RF", StringComparison.OrdinalIgnoreCase))
                    entry.Tag = "RF";
                else if (entry.TestID.TestText.Contains("DC", StringComparison.OrdinalIgnoreCase)
                      || entry.TestID.TestText.Contains("IDD", StringComparison.OrdinalIgnoreCase))
                    entry.Tag = "DC";
            }

            // ── Step 4: 导出升级后的 CSV ───────────────────────────────
            var outCsvpath = csvPath + "_UpgradedToProfile.csv";
            profile.ExportToFile(outCsvpath);
            Msg.Information($"已升级的 TestIDProfileSet 导出至: {outCsvpath}");

            // 同时演示降级回 TestIDSet
            var tidSetBack = profile.ToTestIDSet();
            Msg.Information($"降级回 TestIDSet: {tidSetBack.Count} 条（可选字段已丢弃）");
        }
    }
}
