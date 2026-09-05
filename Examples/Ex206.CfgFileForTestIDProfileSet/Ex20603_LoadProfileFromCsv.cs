using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDProfileSet
{
    /// <summary>
    /// 从 CSV 文件加载 TestIDProfileSet，展示读写 round-trip。
    /// 支持两种格式：纯 TestIDSet 退化格式 + 完整 TestIDProfileSet 格式。
    /// </summary>
    public static class Ex20603_LoadProfileFromCsv
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 打开已保存的 TestIDProfile CSV 文件 ─────────────
            var profile = TDAFileDialog.OpenTestIDProfileSet();

            if (profile is null)
            {
                Msg.Warning("未选择 CSV 文件或加载失败！");
                return;
            }

            // ── Step 2: 打印加载结果概览 ───────────────────────────────
            Msg.Information("从配置文件加载 TestIDProfileSet");
            Msg.Information($"共 {profile.Count} 条记录，{profile.UniqueCount} 个唯一 TestID");

            // 打印所有 Tag 分组
            var tags = profile.GetTags();
            if (tags.Count > 0)
            {
                Msg.Information($"包含 Tag 分组: {string.Join(", ", tags)}");
            }

            // ── Step 3: 打印全部条目详情 ───────────────────────────────
            foreach (var entry in profile)
            {
                var tag    = entry.Tag       ?? "";
                var ren    = entry.HasRename ? entry.ResolvedTestID.ToString() : "(无)";
                var lsl    = entry.ULsl?.ToString() ?? "";
                var hsl    = entry.UHsl?.ToString() ?? "";
                Msg.Debug($"  {(string.IsNullOrEmpty(tag) ? "" : $"[{tag}]")} {entry.TestID} → {ren} | LSL={lsl} HSL={hsl}");
            }

            // ── Step 4: 按 Tag 筛选演示 ───────────────────────────────
            foreach (var tag in profile.GetTags())
            {
                var filtered = profile.FilterByTag(tag);
                Msg.Information($"Tag [{tag}] 筛选结果: {filtered.Count} 条");
            }
        }
    }
}
