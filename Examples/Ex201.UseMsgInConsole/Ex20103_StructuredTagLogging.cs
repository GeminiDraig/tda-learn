using Serilog.Events;
using TDA.Utilities;

namespace Ex201.UseMsgInConsole
{
    /// <summary>
    /// 演示 Msg 自动 TDA Tag 的日志输出。
    /// 所有通过 Msg 输出的日志自动带有 [TDA] Tag 属性，
    /// 无需调用方手动传入 tag 参数。
    /// </summary>
    public static class Ex20103_StructuredTagLogging
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            Msg.Init(new MsgConfig
            {
                MinLevel = LogEventLevel.Debug,
            });

            Msg.Information("自动 TDA Tag 日志演示开始");
            Msg.BlankLine();

            // ── Step 1: 框架内部日志 —— 自动带 [TDA] Tag ──────────────────
            Msg.Information("=== 框架内部日志（自动带 [TDA] Tag）===");
            Msg.Information("数据库连接池已初始化");
            Msg.Debug("配置模块加载完成，耗时 {Elapsed}ms", 120);
            Msg.Warning("缓存命中率下降至 {Rate}", 0.72);
            Msg.BlankLine();

            // ── Step 2: 结构化对象日志 ────────────────────────────────────
            Msg.Information("=== 结构化对象日志 ===");
            Msg.Information("用户 {User} 登录成功", "张三");
            Msg.Information("订单 {OrderId} 已创建", 1001);
            Msg.Warning("订单 {OrderId} 库存不足", 2005);
            Msg.Error("支付接口返回异常，订单号 {OrderId}", 3008);
            Msg.BlankLine();

            // ── Step 3: Error/Critical + 异常 ─────────────────────────────
            Msg.Information("=== 异常日志 ===");
            try
            {
                throw new InvalidOperationException("模拟数据库连接超时");
            }
            catch (Exception ex)
            {
                Msg.Error(ex, "数据库操作失败，表名 {TableName}", "Users");
            }

            try
            {
                throw new ApplicationException("模拟关键服务崩溃");
            }
            catch (Exception ex)
            {
                Msg.Fatal(ex, "关键服务不可用，服务名 {ServiceName}", "PaymentGateway");
            }

            Msg.BlankLine();
            Msg.Information("提示：所有日志自动带有 [TDA] Tag 属性，可在日志平台中按 Tag = \"TDA\" 精确过滤。");

            Msg.Close();
        }
    }
}
