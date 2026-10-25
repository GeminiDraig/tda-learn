using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;
using static TDA.DataPrism;

namespace Ex103.ExportRawData
{
    /// <summary>
    /// 从多个 STDF 文件导出 Final Data（仅最终测试结果）到 CSV。
    /// 通过 GUI 文件对话框选择 STDF 文件和 TestIDSet 配置，加载数据后批量导出。
    /// </summary>
    public static class Ex10301_ExportDataWithSpecifiedTestIDs
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 浏览并选择多个 STDF 文件 ────────────────────────
            var dataPrisms = TDAFileDialog.OpenDataPrisms(multiBrowse: true);
            if (dataPrisms is null)
            {
                Msg.Debug("No valid file selected");
                return;
            }

            // ── Step 2: 可选获取 TestIDSet 筛选配置文件 ────────────────────
            TestIDSet? tstIdList = TDAFileDialog.OpenTestIDSet();

            // ── Step 3: 逐个加载并解析 STDF 文件 ────────────────────────
            foreach (var dataprism in dataPrisms)
            {
                dataprism.Load();
                //dataprism.DoStatisticAnalysis();
                //只需要获取raw data的话，完全没必要执行DoStatisticAnalysis这个步骤
            }

            // ── Step 4: 批量导出 Final Data 到 CSV ──────────────────────
            var dataFilePath = DataPrism.ExportRawDatas(dataPrisms, tstIdList, isFinalDataOnly: true, RawDataLayout.NEDA);

            Console.WriteLine($"Raw data Exported to {dataFilePath} successfully!");
        }
    }
}
