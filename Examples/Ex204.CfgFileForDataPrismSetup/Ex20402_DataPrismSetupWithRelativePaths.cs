using TDA;
using TDA.Utilities;

namespace Ex204.CfgFileForDataPrismSetup
{
    /// <summary>
    /// 使用相对路径构建 DataPrismSetup，序列化为 JSON 文件后再反序列化验证。
    /// <para>
    /// 相对路径以 JSON 配置文件所在目录为基准。
    /// DeserializeFromJsonFile 会自动将相对路径规范化为绝对路径，适合配置文件分发共享场景。
    /// </para>
    /// </summary>
    public static class Ex20402_DataPrismSetupWithRelativePaths
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 创建 DataPrismSetup ─────────────────────────────
            var setup = new DataPrismSetup();

            // ── Step 2: 构建 Sublot 1（初测 + 指定 HBin 复测）─────────────
            var sublot1 = new SublotSetup();

            // first test round of sublot 1
            sublot1.AddFirstTestRoundDataFiles(
                "Lot1\\InitTest1.stdf",
                "Lot1\\InitTest2.stdf"
            );

            // retest round 1 of sublot 1 (仅复测 HBin=2,3)
            sublot1.AddRetestRoundDataFiles(
                new[] { "Lot1\\Retest1.stdf" },
                new[] { (ushort)2, (ushort)3 }
            );
            setup.Sublots.Add(sublot1);

            // ── Step 3: 构建 Sublot 2（初测 + 指定 HBin 复测 + 全 Bin 复测）─
            var sublot2 = new SublotSetup();
            // first test round of sublot 2
            sublot2.AddFirstTestRoundDataFiles(
                "Lot2\\InitTest1.stdf"
            );
            // retest round 1 of sublot 2 (仅复测 HBin=4,5,6)
            sublot2.AddRetestRoundDataFiles(
                new[] { "Lot2\\Retest1.stdf" },
                new[] { (ushort)4, (ushort)5, (ushort)6 }
            );
            // retest round 2 of sublot 2 (复测所有 Fail Bin)
            sublot2.AddRetestRoundDataFiles(
                new[] { "Lot2\\Retest2.stdf" }
            );
            setup.Sublots.Add(sublot2);

            // ── Step 4: 序列化 + 反序列化验证 ────────────────────────────
            string cfgfilePath = Path.Combine(TDA.FileIO.Paths.DefaultWkspsDir, "DataPrismSetup_RelativePaths.json");

            DataPrismSetup.SerializeToJsonFile(setup, cfgfilePath);

            DataPrismSetup newStdfSourceSetup = DataPrismSetup.DeserializeFromJsonFile(cfgfilePath) ?? new DataPrismSetup();

            Msg.BlankLine();
            Msg.Debug($"DataPrismSetup with Relative Paths generated: {cfgfilePath}");
            newStdfSourceSetup.Print();
        }
    }
}
