namespace Ex401.UseStdfReader
{
    /// <summary>
    /// Ex401：演示 StdfReader 核心用法 —— 打开 STDF 文件、逐条读取记录、类型转换与文件生命周期管理。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex40101_ReadAndPrintAsMarkdown.Execute();
            //Ex40102_ReadRecordsAsGeneralRecordType.Execute();
            //Ex40103_ReadAndProcessAsSpecificRecord.Execute();
            //Ex40104_HowToCloseRecordReader.Execute();
            Console.WriteLine("Job done!");
            Console.ReadKey();
        }
    }
}
