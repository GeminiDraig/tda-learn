using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex206.CfgFileForTestIDRenameSet
{
    /// <summary>
    /// 从 CSV 文件加载 TestIDRenameSet，展示读写 round-trip。
    /// 支持两种格式：纯 TestIDSet 退化格式 + 完整 TestIDRenameSet 格式
    /// （亦兼容旧 9 列 TestIDProfileSet 文件，多余的 Tag / ULsl / UHsl 列被忽略）。
    /// </summary>
    public static class Ex20603_LoadProfileFromCsv
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 打开已保存的 TestIDRename CSV 文件 ─────────────
            var renameSet = TDAFileDialog.OpenTestIDRenameSet();

            if (renameSet is null)
            {
                Msg.Warning("未选择 CSV 文件或加载失败！");
                return;
            }

            // ── Step 2: 打印加载结果概览 ───────────────────────────────
            Msg.Information("从配置文件加载 TestIDRenameSet");
            Msg.Information($"共 {renameSet.Count} 条记录，{renameSet.UniqueCount} 个唯一 TestID");

            // ── Step 3: 打印全部条目详情 ───────────────────────────────
            foreach (var entry in renameSet)
            {
                var ren = entry.HasRename ? entry.ResolvedTestID.ToString() : "(无)";
                Msg.Debug($"  {entry.TestID} → {ren}");
            }

            // ── Step 4: 重命名投影演示 ─────────────────────────────────
            // ToResolvedTestIDSet：全部条目按 REN 目标侧投影为 TestIDSet（无 REN 的取原 TestID）
            var resolved = renameSet.ToResolvedTestIDSet();
            var renameCount = renameSet.Count(e => e.HasRename);
            Msg.Information($"其中 {renameCount} 条设置了重命名映射，投影后 TestIDSet 共 {resolved.Count} 个");
        }
    }
}
