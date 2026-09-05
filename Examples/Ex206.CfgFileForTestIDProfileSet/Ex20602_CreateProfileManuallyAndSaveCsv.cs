using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDProfileSet
{
    /// <summary>
    /// 手动创建 TestIDProfileSet 并保存为 CSV，展示所有可选字段的用法。
    /// </summary>
    public static class Ex20602_CreateProfileManuallyAndSaveCsv
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 手动构造 TestIDProfileSet ───────────────────────
            var profile = new TestIDProfileSet();

            // 第 1 条：含 Tag + 重命名映射 + 用户限规
            profile.Add(new TestIDProfile
            {
                TestID = new TestID(1001, "VDD_Current", 0),
                Tag = "DC",
                TestID_REN = new TestID(1001, "IDD_mA", 0),   // 关联重命名
                ULsl = -0.5,
                UHsl = 50.0
            });

            // 第 2 条：仅含 Tag
            profile.Add(new TestIDProfile
            {
                TestID = new TestID(1002, "RF_Output_Power", 0),
                Tag = "RF"
            });

            // 第 3 条：仅含用户限规
            profile.Add(new TestIDProfile
            {
                TestID = new TestID(1003, "Phase_Noise", 0),
                ULsl = -100.0,
                UHsl = -30.0
            });

            // 第 4 条：仅有核心 TestID（最简格式，等同 TestIDSet）
            profile.Add(new TestIDProfile
            {
                TestID = new TestID(1004, "Leakage_GND", 0)
            });

            // ── Step 2: 导出 CSV ───────────────────────────────────
            var csvpath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "TestIDProfile_Manual.csv");
            profile.ExportToFile(csvpath);
            Msg.Information($"手动创建的 TestIDProfileSet 已保存至: {csvpath}");

            // 打印内容概览
            foreach (var entry in profile)
            {
                var tag    = entry.Tag       ?? "(无Tag)";
                var ren    = entry.HasRename ? entry.TestID_REN.ToString() : "(无重命名)";
                var limits = entry.HasUserLimits ? $"{entry.ULsl}~{entry.UHsl}" : "(无限规)";
                Msg.Debug($"[{tag}] {entry.TestID} → REN:{ren} | Limits:{limits}");
            }
        }
    }
}
