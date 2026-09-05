namespace Ex202.UseMsgInWinform
{
    /// <summary>
    /// Ex202：演示在 WinForm 程序中使用 Msg 消息系统的各种场景。
    /// 包含六级日志按钮、Tag 输入、异常模拟、日志级别过滤、配置管理等。
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// WinForm 应用程序主入口。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
