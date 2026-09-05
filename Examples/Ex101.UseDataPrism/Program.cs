namespace Ex101.UseDataPrism
{
    /// <summary>
    /// Ex101.UseDataPrism：演示 DataPrism 的多种构造方式、数据处理流程及各阶段数据访问方法。
    /// 所有范例均直接打印数据到控制台，不导出文件。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex10101_UseDataPrism.Execute();                 // 弹窗选文件 → 打印元数据
            //Ex10102_DifferentInputFormats.Execute();      // 三种不同的 DataPrism 构造方式
            //Ex10103_AccessMetaInfoAndRawData.Execute();   // Load 后访问元信息与原始测试值
            //Ex10104_AccessStatistics.Execute();            // Analyzed 后访问统计数据与 Bin 汇总
            //Ex10105_ApplyTestIDProfile.Execute();          // ApplyTestIDProfile 过滤与重命名 TestID

            Console.WriteLine("Job done!");
        }
    }
}
