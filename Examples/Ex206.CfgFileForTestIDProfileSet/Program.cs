namespace Ex206.CfgFileForTestIDProfileSet
{
    /// <summary>
    /// Ex206：演示 TestIDProfileSet 配置文件的创建、读写与转换。
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex20601_ProfileFromStdfAndSaveCsv.Execute();
            //Ex20602_CreateProfileManuallyAndSaveCsv.Execute();
            //Ex20603_LoadProfileFromCsv.Execute();
            //Ex20604_ConvertFromTestIDSet.Execute();
            Console.WriteLine("Job done!");
        }
    }
}
