using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDRenameSet
{
    /// <summary>
    /// 手动创建 TestIDRenameSet 并保存为 CSV，展示所有可选字段的用法。
    /// </summary>
    public static class Ex20602_CreateProfileManuallyAndSaveCsv
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 手动构造 TestIDRenameSet ───────────────────────
            var renameSet = new TestIDRenameSet();

            // 第 1 条：含重命名映射（旧程序 TestID → 新程序 TestID）
            renameSet.Add(new TestIDRename
            {
                TestID = new TestID(1001, "VDD_Current", 0),
                TestID_REN = new TestID(1001, "IDD_mA", 0)   // 关联重命名
            });

            // 第 2 条：含重命名映射（TestNumber 变化的场景）
            renameSet.Add(new TestIDRename
            {
                TestID = new TestID(1002, "RF_Output_Power", 0),
                TestID_REN = new TestID(6100002, "RF_Output_Power", 0)
            });

            // 第 3 条：仅有核心 TestID（最简格式，等同 TestIDSet——仅作保留清单，不重命名）
            renameSet.Add(new TestIDRename
            {
                TestID = new TestID(1003, "Phase_Noise", 0)
            });

            // ── Step 2: 导出 CSV ───────────────────────────────────
            var csvpath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "TestIDRename_Manual.csv");
            renameSet.ExportToFile(csvpath);
            Msg.Information($"手动创建的 TestIDRenameSet 已保存至: {csvpath}");

            // 打印内容概览
            foreach (var entry in renameSet)
            {
                var ren = entry.HasRename ? entry.TestID_REN.ToString() : "(无重命名)";
                Msg.Debug($"{entry.TestID} → REN:{ren}");
            }
        }
    }
}
