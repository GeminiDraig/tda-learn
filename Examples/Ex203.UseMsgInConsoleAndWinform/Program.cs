namespace Ex203.UseMsgInConsoleAndWinform
{
    internal static class Program
    {
        /// <summary>
        /// Ex203：演示在控制台+WinForm 组合程序中使用 Msg 消息系统。
        /// 控制台输出日志的同时，WinForm 通过工具条按钮触发消息输出。
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
