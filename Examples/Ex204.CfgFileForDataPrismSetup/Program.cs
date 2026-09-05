namespace Ex204.CfgFileForDataPrismSetup
{
    /// <summary>
    /// Ex204：演示如何通过代码构建 DataPrismSetup 配置，并序列化/反序列化为 JSON 配置文件。
    /// <para>
    /// DataPrismSetup 是 DataPrism 解析 STDF 数据的核心输入配置，描述了一个 Lot 的
    /// 所有 Sublot、测试轮次（初测/复测）、STDF 文件路径以及复测 HardBin 范围。
    /// </para>
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex20401_DataPrismSetupWithAbsolutePaths.Execute();
            Ex20402_DataPrismSetupWithRelativePaths.Execute();
            Console.WriteLine("Job done!");
        }
    }
}
