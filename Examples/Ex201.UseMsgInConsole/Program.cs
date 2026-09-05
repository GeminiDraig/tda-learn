namespace Ex201.UseMsgInConsole
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex20101_AutoInitAndSixLevels.Execute();
            Ex20102_ExplicitInitWithConfig.Execute();
            Ex20103_StructuredTagLogging.Execute();
            Ex20104_StructuredObjectAndException.Execute();
            Ex20105_BlankLineAndLegacyApi.Execute();
            Console.WriteLine("Job done!");
        }
    }
}
