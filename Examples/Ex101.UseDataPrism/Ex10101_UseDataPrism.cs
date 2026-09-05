using TDA;
using TDA.FileIO;

namespace Ex101.UseDataPrism
{
    /// <summary>
    /// DataPrism 最简用法演示：通过弹窗选择 STDF 文件，加载并打印基本信息。
    /// 流程：弹窗选文件 → Load → DoStatisticAnalysis → 打印 MIR/SDR 元数据。
    /// </summary>
    public static class Ex10101_UseDataPrism
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 弹窗选择并打开 STDF 文件 ────────────────────────
            DataPrism? dataPrism = TDAFileDialog.OpenDataPrism();
            if (dataPrism is null)
                throw new ArgumentNullException("invalid data prism input");

            // ── Step 2: 加载测试数据 ───────────────────────────────────
            dataPrism.Load();

            // ── Step 3: 执行统计分析 ───────────────────────────────────
            dataPrism.DoStatisticAnalysis();

            // ── Step 4: 访问并打印 MIR / SDR 元数据 ─────────────────────
            var info = dataPrism.DataSourcesInfoList[0];
            string setupT = info.Mir.SETUP_T.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string startT = info.Mir.START_T.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"File: {dataPrism.Setup.PrimaryFile}");
            Console.WriteLine($"LotID:       {info.Mir.LOT_ID}");
            Console.WriteLine($"SBLOT_ID:    {info.Mir.SBLOT_ID}");
            Console.WriteLine($"SETUP_T:     {setupT}");
            Console.WriteLine($"START_T:     {startT}");
            Console.WriteLine($"TSTR_TYP:    {info.Mir.TSTR_TYP}");
            Console.WriteLine($"Part count:  {dataPrism.PartsDescription.Length}");
            Console.WriteLine($"Test items:  {dataPrism.TestItems.Count}");
            Console.Write("Sites:       ");
            foreach (byte s in info.Sdr.SITE_NUM)
                Console.Write($"{s} ");
            Console.WriteLine();

            dataPrism.Dispose();
        }
    }
}
