namespace Ex302.UseRecordWriter
{
    /// <summary>
    /// Ex302：演示使用 RecordReader 逐条读取 STDF 文件，并通过 RecordWriter 写入新文件。
    /// <para>
    /// 核心流程：
    /// <list type="number">
    ///   <item>弹窗选取源 STDF 文件</item>
    ///   <item>用 RecordReader 逐条读取所有 Record</item>
    ///   <item>用 RecordWriter 将每条 Record 写入输出文件</item>
    ///   <item>输出写入统计摘要</item>
    /// </list>
    /// </para>
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex30201_CopyStdfFile.Execute();
            Console.WriteLine("Job done!");
            Console.ReadKey();
        }
    }
}
