using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex205.CfgFileForTestIDSet
{
    /// <summary>
    /// TestIDSet 配置文件读写演示。
    /// 流程：打开 STDF → 导出所有 TestID 到 CSV → 重新加载 CSV 并打印。
    /// </summary>
    public static class Ex20501_TestIDSetCfgFileWriteRead
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 打开 STDF，提取 TestID 并导出到 CSV ─────────────
            var dataprisms = TDAFileDialog.OpenDataPrisms();
            if (dataprisms is null || dataprisms.Count < 1)
            {
                Msg.Warning("TestIDSet export fail, Not any valid DataPrism created !");
                return;
            }

            foreach (var dp in dataprisms)
            {
                dp.Load();
                //dp.DoStatisticAnalysis();
            }

            // 导出 TestIDSet：默认输出位置优先 DataPrismSetup 配置文件所在目录（若通过 .dp.json 加载），否则回退到首个数据文件目录
            var csvpath = dataprisms[0].ResolveDefaultOutputPath("_TestIDSet.csv");
            DataPrism.ExportTestIDSet(dataprisms, csvpath);

            // ── Step 2: 重新加载 TestIDSet CSV 并打印内容 ───────────────
            var tstIdList = TestIDSet.ImportFromFile(csvpath);
            if (tstIdList is not null)
            {
                Msg.Information($"Load TestIDSet from config file: {csvpath}");
                foreach (var tst in tstIdList)
                    Msg.Debug(tst.ToString());
            }
            else
            {
                Msg.Warning("Fail to load TestIDset from config file !");
            }
        }
    }
}
