namespace Ex205.CfgFileForTestIDSet
{
    /// <summary>
    /// Ex205：演示 TestIDSet 配置文件的读写。
    /// 流程：打开 STDF → 导出所有 TestID 到 CSV → 重新加载 CSV 并打印。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex20501_TestIDSetCfgFileWriteRead.Execute();
            Console.WriteLine("Job done!");
        }
    }
}
