using TDA;
using TDA.FileIO;

namespace Ex101.UseDataPrism
{
    /// <summary>
    /// 演示 DataPrism 的三种不同构造方式：
    /// 1) 文件路径字符串构造（最简：单文件路径）
    /// 2) SublotSetup 构造（支持复测轮配置）
    /// 3) DataPrismSetup 构造（完整配置：多 Sublot + DeviceID + Profile）
    /// </summary>
    public static class Ex10102_DifferentInputFormats
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ──────────────────────────────────────────────────────────
            //  方式三：通过 DataPrismSetup 完整配置
            //  可以配置多个 Sublot、DeviceID 解析方式、TestIDProfile 等
            // ──────────────────────────────────────────────────────────
            Console.WriteLine("=== 方式一：DataPrismSetup 完整配置 ===");

            var setup = TDAFileDialog.OpenDataPrismSetup();
            if (setup is null) return;

            var dp3 = new DataPrism(setup);
            if (setup is null) return;

            using (dp3)
            {
                dp3.Load();
                dp3.DoStatisticAnalysis();
                Console.WriteLine($"  Sublot count:     {dp3.Setup.Sublots.Count}");
                Console.WriteLine($"  PrimaryFile:      {dp3.Setup.PrimaryFile}");
                Console.WriteLine($"  Test items count: {dp3.TestItems.Count}");

                var info = dp3.DataSourcesInfoList[0];
                Console.WriteLine($"  LotID:            {info.Mir.LOT_ID}");
                Console.WriteLine($"  Part count:       {dp3.PartsDescription.Length}");
            }

            Console.WriteLine();
            Console.WriteLine("三种构造方式演示完毕。");

            // ──────────────────────────────────────────────────────────
            //  方式二：直接传入文件路径字符串（最简单,支持 *.stf，*.stdf，*.stf.gz，*.stdf.gz， DataPrismSetup *.json文件）
            // ──────────────────────────────────────────────────────────
            Console.WriteLine("=== 方式二：文件路径字符串构造 ===");

            string? filePath = TDAFileDialog.SelectFile(FileType.DataPrismSource);
            if (filePath is null) return;

            var dp1 = DataPrism.FromFile(filePath);
            if (dp1 is null) throw new InvalidOperationException($"Failed to create DataPrism from file: {filePath}");

            using (dp1)
            {
                dp1.Load();
                dp1.DoStatisticAnalysis();
                Console.WriteLine($"  PrimaryFile:      {dp1.Setup.PrimaryFile}");
                Console.WriteLine($"  Sublot count:     {dp1.Setup.Sublots.Count}");
                Console.WriteLine($"  Test items count: {dp1.TestItems.Count}");
                Console.WriteLine($"  Part count:       {dp1.PartsDescription.Length}");
                Console.WriteLine($"  IsCP:             {dp1.IsCP}");
                Console.WriteLine();
            }
        }
    }
}
