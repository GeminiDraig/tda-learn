namespace Ex301.RecordReaderBasics
{
    /// <summary>
    /// Ex301：演示 RecordReader 核心用法 —— 初始化、读取 STDF 记录、配置编解码与校验等级。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex30101_BasicWorkflow.Execute();
            //Ex30102_ConfigurationDemo.Execute();
            Console.WriteLine("Job done!");
            Console.ReadKey();
        }
    }
}
