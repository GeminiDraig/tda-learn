using TDA;
using TDA.Utilities;

namespace Ex204.CfgFileForDataPrismSetup
{
    /// <summary>
    /// 使用绝对路径构建 DataPrismSetup，序列化为 JSON 文件后再反序列化验证。
    /// <para>
    /// 构建流程：
    /// 1. new DataPrismSetup
    /// 2. new SublotSetup + AddFirstTestRoundDataFiles / AddRetestRoundDataFiles 构建每个 Sublot
    /// 3. 将 SublotSetup 添加到 DataPrismSetup.Sublots
    /// 4. SerializeToJsonFile 保存配置 → DeserializeFromJsonFile 加载验证
    /// </para>
    /// <para>
    /// <b>场景说明：</b>一个 Lot 被拆分为 2 个 Sublot，
    /// Sublot1 有初测(2个文件)+1轮复测(指定HardBin 2,3)，
    /// Sublot2 有初测(1个文件)+1轮指定HardBin复测(4,5,6)+1轮全Bin复测。
    /// </para>
    /// </summary>
    public static class Ex20401_DataPrismSetupWithAbsolutePaths
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 创建 DataPrismSetup ─────────────────────────────
            string docsPath = TDA.FileIO.Paths.DefaultWkspsDir;
            var setup = new DataPrismSetup();

            // ── Step 2: 构建 Sublot 1（初测 + 指定 HBin 复测）─────────────
            var sublot1 = new SublotSetup();

            // first test round of sublot 1
            sublot1.AddFirstTestRoundDataFiles(
                Path.Combine(docsPath, "Lot1", "InitTest1.stdf"),
                Path.Combine(docsPath, "Lot1", "InitTest2.stdf")
            );

            // retest round 1 of sublot 1 (仅复测上一轮 HBin=2 和 HBin=3 的 Fail 芯片)
            sublot1.AddRetestRoundDataFiles(
                new[] { Path.Combine(docsPath, "Lot1", "Retest1.stdf") },
                new[] { (ushort)2, (ushort)3 }
            );
            setup.Sublots.Add(sublot1);

            // ── Step 3: 构建 Sublot 2（初测 + 指定 HBin 复测 + 全 Bin 复测）─
            var sublot2 = new SublotSetup();
            // first test round of sublot 2
            sublot2.AddFirstTestRoundDataFiles(
                Path.Combine(docsPath, "Lot2", "InitTest1.stdf")
            );
            // retest round 1 of sublot 2 (仅复测上一轮 HBin=4,5,6 的 Fail 芯片)
            sublot2.AddRetestRoundDataFiles(
                new[] { Path.Combine(docsPath, "Lot2", "Retest1.stdf") },
                new[] { (ushort)4, (ushort)5, (ushort)6 }
            );
            // retest round 2 of sublot 2 (复测上一轮所有 Fail Bin，不限定 HardBin)
            sublot2.AddRetestRoundDataFiles(
                new[] { Path.Combine(docsPath, "Lot2", "Retest2.stdf") }
            );
            setup.Sublots.Add(sublot2);

            // ── Step 4: 序列化 + 反序列化验证 ────────────────────────────
            string cfgfilePath = Path.Combine(TDA.FileIO.Paths.DefaultWkspsDir, "DataPrismSetup_AbsolutePaths.json");

            DataPrismSetup.SerializeToJsonFile(setup, cfgfilePath);

            DataPrismSetup newStdfSourceSetup = DataPrismSetup.DeserializeFromJsonFile(cfgfilePath) ?? new DataPrismSetup();

            Msg.BlankLine();
            Msg.Debug($"DataPrismSetup with Absolute Paths generated: {cfgfilePath}");
            newStdfSourceSetup.Print();
        }
    }
}
