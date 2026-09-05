using Serilog.Events;
using TDA.Utilities;

namespace Ex201.UseMsgInConsole
{
    /// <summary>
    /// 演示 Msg 显式配置的三种方式：Init() 默认配置、Init(string) 自定义路径、Init(MsgConfig) 完整配置。
    /// 并展示不同 MinLevel 设置对日志输出的影响。
    /// </summary>
    public static class Ex20102_ExplicitInitWithConfig
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: Init() —— 使用默认配置显式初始化 ──────────────────
            Msg.Init();
            Msg.Information("已通过 Msg.Init() 显式初始化（使用默认配置）");
            Msg.Information("默认 MinLevel=Info, LogPath=./Logs/app-.log, EnableConsole=true");
            Msg.BlankLine();
            Msg.Close();

            // ── Step 2: Init(string) —— 指定日志输出目录 ──────────────────
            Msg.Init("./Logs/Ex20102/");
            Msg.Information("已通过 Msg.Init(\"./Logs/Ex20102/\") 指定日志目录");
            Msg.Debug("这条 Debug 日志在默认 MinLevel=Info 下不会输出到文件");
            Msg.BlankLine();
            Msg.Close();

            // ── Step 3: Init(MsgConfig) —— 完整自定义配置 ─────────────────
            Msg.Init(new MsgConfig
            {
                LogPath       = "./Logs/Ex20102/app-.log",
                MinLevel      = LogEventLevel.Debug,
                EnableConsole = true,
            });
            Msg.Information("已通过 Msg.Init(MsgConfig) 完整配置：MinLevel=Debug");
            Msg.Debug("现在 Debug 级别也会输出了！");
            Msg.Verbose("Trace 仍然不会输出（MinLevel=Debug 时 Verbose 被过滤）");
            Msg.BlankLine();

            // ── Step 4: 关闭控制台输出 ────────────────────────────────────
            Msg.Close();

            Msg.Init(new MsgConfig
            {
                LogPath       = "./Logs/Ex20102/file-only-.log",
                MinLevel      = LogEventLevel.Debug,
                EnableConsole = false,
            });
            Msg.Information("已配置 EnableConsole=false，以下日志仅写入文件，控制台不显示：");
            Msg.Debug("这条 Debug 消息只出现在日志文件中");
            Msg.Warning("这条 Warning 消息也只出现在日志文件中");
            Msg.Information("请打开 ./Logs/Ex20102/file-only-*.log 验证文件输出");
            Msg.BlankLine();

            Msg.Close();
        }
    }
}
