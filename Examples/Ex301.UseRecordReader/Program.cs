namespace Ex301.UseRecordReader
{
    /// <summary>
    /// Ex301：演示 STDF 记录级读取（RecordReader）—— 打开 STDF 文件、逐条读取记录、类型转换、解压模式与文件生命周期管理。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex30101_BasicWorkflow.Execute();
            //Ex30102_HowToCloseRecordReader.Execute();
            //Ex30103_ReadRecordsAsGeneralRecordType.Execute();
            //Ex30104_ReadRecordAsSpecificRecordType.Execute();
            //Ex30105_ConfigurationDemo.Execute();
            Console.WriteLine("Job done!");
            Console.ReadKey();
        }
    }
}
