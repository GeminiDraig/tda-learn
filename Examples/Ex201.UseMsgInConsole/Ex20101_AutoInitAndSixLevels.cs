using TDA.Utilities;

namespace Ex201.UseMsgInConsole
{
    /// <summary>
    /// 演示 Msg 零配置自动初始化 + 六个日志级别的完整输出。
    /// 无需显式调用 Init()，首次调用日志方法自动完成默认配置。
    /// </summary>
    public static class Ex20101_AutoInitAndSixLevels
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 零配置——直接写日志，Msg 自动初始化 ────────────────
            Msg.Information("Msg 零配置演示开始 —— 首次调用自动初始化");
            Msg.BlankLine();

            // ── Step 2: 输出六个级别的日志 ─────────────────────────────────
            Msg.Verbose("这是 Trace 级别日志（最详细，通常用于跟踪）");
            Msg.Debug("这是 Debug 级别日志（调试信息）");
            Msg.Information("这是 Info 级别日志（常规信息）");
            Msg.Warning("这是 Warning 级别日志（警告信息）");
            Msg.Error("这是 Error 级别日志（错误信息）");
            Msg.Fatal("这是 Critical 级别日志（严重错误）");

            // ── Step 3: 说明默认最小级别为 Info ───────────────────────────
            Msg.BlankLine();
            Msg.Information("注意：默认最小级别为 Info，因此 Trace/Debug 在默认配置下不会输出到文件。");
            Msg.Information("若需查看 Trace/Debug，请调用 Msg.Init(new MsgConfig { MinLevel = LogEventLevel.Verbose })。");
            Msg.BlankLine();

            // ── Step 4: 带参数的结构化日志 ─────────────────────────────────
            Msg.Information("用户 {UserName} 今日登录 {Count} 次", "张三", 42);

            // ── Step 5: 释放资源 ──────────────────────────────────────────
            Msg.Close();
        }
    }
}
