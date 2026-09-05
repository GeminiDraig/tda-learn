using TDA.Utilities;

namespace Ex201.UseMsgInConsole
{
    /// <summary>
    /// 演示 Msg 的辅助方法：BlankLine（空行输出）、SetupTextWriter（旧版兼容 API）。
    /// 展示如何在日志中插入空行分隔段落，以及旧版 API 如何映射到新版配置。
    /// </summary>
    public static class Ex20105_BlankLineAndLegacyApi
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 使用旧版 SetupTextWriter 配置日志 ─────────────────
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string dir = Path.Combine(desktopPath, "TDATest");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            Msg.Information("已通过旧版 API SetupTextWriter 配置日志输出");
            Msg.Information("日志文件位于：{Path}", Path.Combine(dir, "Ex20105.LegacyDemo.log"));
            Msg.BlankLine();

            // ── Step 2: BlankLine() 单行空行 ─────────────────────────────
            Msg.Information("=== BlankLine() 单行空行演示 ===");
            Msg.Information("这是一条 Info 消息");
            Msg.BlankLine();         // 输出一个空行（通过 Debug 管道）
            Msg.Information("这条消息上方有一个空行");
            Msg.BlankLine();
            Msg.Warning("Warning 消息前后各有一个空行");
            Msg.BlankLine();
            Msg.BlankLine();

            // ── Step 3: BlankLine(n) 多行空行 ────────────────────────────
            Msg.Information("=== BlankLine(n) 多行空行演示 ===");
            Msg.Information("以下内容前将有 3 个空行：");
            Msg.BlankLine(3);
            Msg.Information("上方有 3 个空行");
            Msg.BlankLine(2);
            Msg.Information("上方有 2 个空行");
            Msg.BlankLine();

            // ── Step 4: 空行与日志段落分隔 ────────────────────────────────
            Msg.Information("=== 日志段落分隔实战 ===");
            Msg.BlankLine(2);

            Msg.Information("【第一阶段：数据加载】");
            Msg.Information("正在加载 STDF 文件...");
            Msg.Information("文件加载完成，共 {Count} 条记录", 5000);
            Msg.BlankLine();

            Msg.Information("【第二阶段：统计分析】");
            Msg.Information("正在进行参数统计...");
            Msg.Warning("发现 {Count} 个异常值", 12);
            Msg.BlankLine();

            Msg.Information("【第三阶段：结果输出】");
            Msg.Information("统计完成，结果已导出至 CSV");
            Msg.BlankLine(2);

            Msg.Information("提示：BlankLine 内部调用 Debug(\"\")，空行会同时输出到控制台和日志文件。");
            Msg.Information("      这意味着空行也会受 MinLevel 影响（Debug 级别以上才可见）。");

            Msg.Close();
        }
    }
}
