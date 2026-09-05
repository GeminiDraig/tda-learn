using CsvHelper;
using System.Globalization;
using TDA;
using TDA.FileIO;
using TDA.TypeDef;
using TDA.Utilities;

namespace Ex106.ExportPartSummary
{
    /// <summary>
    /// <b>Ex106.ExportPartSummary</b> — 导出 Part/Device 信息列表到 CSV 文件。
    /// <para>
    /// 对应 CLI 命令 <c>dp export psum</c>（别名 <c>dsum</c>），
    /// 输出每颗芯片的 Part 信息，包含 Pass/Fail 状态、HardBin/SoftBin、坐标、数据来源等字段。
    /// </para>
    /// </summary>
    /// <remarks>
    /// 使用步骤：
    /// <list type="number">
    ///   <item><b>选择数据源</b>：通过文件对话框选择 STDF 文件或 JSON setup 配置文件。</item>
    ///   <item><b>加载与解析</b>：调用 DataPrism.Load() 和 DoStatisticAnalysis()。</item>
    ///   <item><b>导出 CSV</b>：遍历 PartsDescription，将每颗芯片的信息写入 CSV 文件。</item>
    /// </list>
    /// </remarks>
    public class ExportPartSummary
    {
        /// <summary>
        /// 导出 Part/Device 信息的主流程。
        /// </summary>
        public static void Test()
        {
            // ── 1. 选择数据源 ──────────────────────────────────
            DataPrism? dataPrism = TDAFileDialog.OpenDataPrism();
            if (dataPrism is null)
            {
                Msg.Warning("No valid data source selected.");
                return;
            }

            // ── 2. 加载并解析数据 ─────────────────────────────
            dataPrism.Load();
            dataPrism.DoStatisticAnalysis();

            // ── 3. 生成输出文件路径（保存到第一个数据源同目录）─
            var filePath = dataPrism.DataSourcesInfoList[0].FilePath;
            string dir = Path.GetDirectoryName(filePath)
                         ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outFilePath = Path.Combine(dir,
                $"{Path.GetFileNameWithoutExtension(filePath)}.psum.csv");

            // ── 4. 导出 CSV ──────────────────────────────────
            ExportToCsv(dataPrism, outFilePath);

            Msg.Information($"Part summary exported to: {outFilePath}");
        }

        /// <summary>
        /// 将 DataPrism 中的 Part 信息导出到指定 CSV 文件。
        /// 输出列（与 CLI design 6.3 保持一致）：
        /// PartIndex / PartID / HardBin / SoftBin / IsPass / IsFinalData /
        /// TestRound / Site / X / Y / FirstFailTest / DataSourceID / FilePath / Time
        /// </summary>
        /// <param name="dataPrism">已加载并分析完毕的 DataPrism 实例。</param>
        /// <param name="outFilePath">目标 CSV 文件路径。</param>
        public static void ExportToCsv(DataPrism dataPrism, string outFilePath)
        {
            using var writer = new StreamWriter(outFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // ── 写入表头 ──────────────────────────────────────
            csv.WriteField("PartIndex");
            csv.WriteField("PartID");
            csv.WriteField("HardBin");
            csv.WriteField("SoftBin");
            csv.WriteField("IsPass");
            csv.WriteField("IsFinalData");
            csv.WriteField("TestRound");
            csv.WriteField("Site");
            csv.WriteField("X");
            csv.WriteField("Y");
            csv.WriteField("FirstFailTest");
            csv.WriteField("DataSourceID");
            csv.WriteField("FilePath");
            csv.WriteField("Time");
            csv.NextRecord();

            // ── 写入每颗芯片数据 ──────────────────────────────
            int partCnt = dataPrism.PartsDescription.Length;
            var dataSources = dataPrism.DataSourcesInfoList;

            for (int i = 0; i < partCnt; i++)
            {
                ref PartInfo part = ref dataPrism.PartsDescription.GetRef(i);

                csv.WriteField(i);                          // PartIndex
                csv.WriteField(part.PartID ?? string.Empty); // PartID
                csv.WriteField(part.HardBinNumber);          // HardBin
                csv.WriteField(part.SoftBinNumber);          // SoftBin
                csv.WriteField(part.IsPass ? "PASS" : "FAIL"); // IsPass
                csv.WriteField(part.IsFinalData);            // IsFinalData
                csv.WriteField(part.TestRoundNumber);        // TestRound
                csv.WriteField(part.SiteNumber);             // Site
                csv.WriteField(part.X);                      // X
                csv.WriteField(part.Y);                      // Y

                // FirstFailTest: 全通过时输出 "PASS"，否则输出测试编号
                if (part.FirstFailTestNum == CONST.TESTNUM4PASS)
                    csv.WriteField("PASS");
                else
                    csv.WriteField(part.FirstFailTestNum.ToString());

                csv.WriteField(part.DataSourceInfoID);       // DataSourceID

                // FilePath: 通过 DataSourceInfoID 索引获取来源文件路径
                if (part.DataSourceInfoID >= 0 && part.DataSourceInfoID < dataSources.Count)
                    csv.WriteField(dataSources[part.DataSourceInfoID].FilePath);
                else
                    csv.WriteField(string.Empty);

                csv.WriteField(part.TestTime);               // Time (ms)

                csv.NextRecord();
            }

            csv.Flush();
        }
    }
}
