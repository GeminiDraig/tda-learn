using Serilog.Events;
using TDA.Utilities;

namespace Ex201.UseMsgInConsole
{
    /// <summary>
    /// 演示 Msg 结构化对象日志（{@Object} 解构）与异常日志的完整用法。
    /// 包含 Serilog 的 {@Destructure} 操作符、异常堆栈输出、以及业务场景模拟。
    /// </summary>
    public static class Ex20104_StructuredObjectAndException
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            Msg.Init(new MsgConfig { MinLevel = LogEventLevel.Debug });

            // ── Step 1: 结构化对象日志（使用 {@} 解构操作符） ────────────
            Msg.Information("=== 结构化对象日志 ===");
            var order = new { Id = 456, Customer = "张三", Total = 99.99m, Status = "已支付" };
            Msg.Information("新订单创建成功 {@Order}", order);

            var config = new { Host = "localhost", Port = 8080, Timeout = 30, RetryCount = 3 };
            Msg.Debug("服务配置加载完成 {@Config}", config);
            Msg.BlankLine();

            // ── Step 2: 结构化对象 + 自动 [TDA] Tag ────────────────────────
            Msg.Information("=== 结构化对象（自动带 [TDA] Tag）===");
            Msg.Information("订单详情 {@Order}", order);
            Msg.Debug("系统配置 {@Config}", config);
            Msg.BlankLine();

            // ── Step 3: 异常日志——Error 级别 ─────────────────────────────
            Msg.Information("=== Error 级别异常日志 ===");

            try
            {
                SimulateFileReadError();
            }
            catch (Exception ex)
            {
                Msg.Error(ex, "读取文件失败，路径 {FilePath}", @"C:\Data\input.csv");
            }

            try
            {
                SimulateNetworkError();
            }
            catch (Exception ex)
            {
                Msg.Error(ex, "网络请求超时，URL {Url}", "https://api.example.com/orders");
            }
            Msg.BlankLine();

            // ── Step 4: 异常日志——Critical 级别 ──────────────────────────
            Msg.Information("=== Critical 级别异常日志 ===");

            try
            {
                SimulateCriticalFailure();
            }
            catch (Exception ex)
            {
                Msg.Fatal(ex, "应用程序致命错误：{Reason}", "内存分配失败");
            }

            try
            {
                SimulateDatabaseCrash();
            }
            catch (Exception ex)
            {
                Msg.Fatal(ex, "数据库连接池耗尽，当前连接数 {Count}", 100);
            }
            Msg.BlankLine();

            // ── Step 5: 多层异常链 ────────────────────────────────────────
            Msg.Information("=== 多层异常链（InnerException）===");

            try
            {
                throw new AggregateException("批量处理失败",
                    new IOException("文件未找到"),
                    new InvalidOperationException("数据格式错误"));
            }
            catch (Exception ex)
            {
                Msg.Error(ex, "批量任务执行失败，任务数 {TaskCount}", 10);
            }

            Msg.BlankLine();
            Msg.Information("提示：异常日志会自动包含完整的 StackTrace 和 InnerException 链。");

            Msg.Close();
        }

        #region --- 模拟方法 ---

        private static void SimulateFileReadError()
        {
            throw new IOException("拒绝访问文件（模拟）");
        }

        private static void SimulateNetworkError()
        {
            throw new TimeoutException("连接超时（模拟）");
        }

        private static void SimulateCriticalFailure()
        {
            throw new OutOfMemoryException("内存不足（模拟）");
        }

        private static void SimulateDatabaseCrash()
        {
            throw new InvalidOperationException("连接池已耗尽（模拟）");
        }

        #endregion
    }
}
