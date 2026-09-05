using CsvHelper;
using TDA;
using TDA.FileIO;

namespace Ex105.ExportSummary
{
    /// <summary>
    /// 展示如何自定义格式从 STDF 文件导出 HBin/SBin 统计摘要到 CSV。
    /// <para>与 Ex10501 的区别：手动控制 CsvWriter 的列布局与写入逻辑，
    /// 便于按需调整输出格式。包含 Qty Statistic、HBin Statistic、SBin Statistic 三部分。</para>
    /// </summary>
    public static class Ex10502_ExportSummaryCustomizeFormat
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 浏览并选择 STDF 文件 ────────────────────────────
            DataPrism? dataPrism = TDAFileDialog.OpenDataPrism();
            if (dataPrism is null) return;

            // ── Step 2: 加载并解析数据 ───────────────────────────────────
            dataPrism.Load();
            dataPrism.DoStatisticAnalysis();

            // ── Step 3: 确定输出文件路径 ─────────────────────────────────
            var filepath = dataPrism.DataSourcesInfoList[0].FilePath;
            string dir = Path.GetDirectoryName(filepath) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            filepath = Path.Join(dir, $"{Path.GetFileNameWithoutExtension(filepath)}.sum.custom.csv");
            using var writer = new StreamWriter(filepath);
            using CsvWriter csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

            // ── Step 4: 写入 Qty Statistic ──────────────────────────────
            var sitelist = dataPrism.Summary.SiteCnt.Keys.ToList();
            csv.WriteField("Qty Statistic:"); csv.NextRecord();
            csv.WriteField("Site");        /**/ csv.WriteField("All");                        /**/ foreach (var site in sitelist) csv.WriteField(site); csv.NextRecord();
            csv.WriteField("Total Qty");   /**/ csv.WriteField(dataPrism.Summary.TotalQty);   /**/ foreach (var kvp in dataPrism.Summary.SiteCnt) csv.WriteField(kvp.Value); csv.NextRecord();
            csv.WriteField("Pass Qty");    /**/ csv.WriteField(dataPrism.Summary.PassQty);    /**/ foreach (var kvp in dataPrism.Summary.PassCntBySite) csv.WriteField(kvp.Value); csv.NextRecord();
            csv.WriteField("Fail Qty");    /**/ csv.WriteField(dataPrism.Summary.FailQty);    /**/ foreach (var kvp in dataPrism.Summary.FailCntBySite) csv.WriteField(kvp.Value); csv.NextRecord();
            csv.WriteField("Invalid Qty"); /**/ csv.WriteField(dataPrism.Summary.InvalidQty); /**/ foreach (var kvp in dataPrism.Summary.InvalidCntBySite) csv.WriteField(kvp.Value); csv.NextRecord();

            // ── Step 5: 写入 HBin Statistic ─────────────────────────────
            var hbins = dataPrism.Summary.HBinCnt.Keys.ToList();
            hbins.Sort();
            sitelist = dataPrism.Summary.HBinCntBySite.Keys.ToList();
            sitelist.Sort();
            csv.WriteField("HBin Statistic:"); csv.NextRecord();
            csv.WriteField("HBin"); csv.WriteField("Name"); csv.WriteField("All"); foreach (var site in sitelist) csv.WriteField(site); csv.NextRecord();
            foreach (var binNo in hbins)
            {
                var binName = dataPrism.Summary.HBins[binNo].Name;
                csv.WriteField(binNo); csv.WriteField(binName); csv.WriteField(dataPrism.Summary.HBinCnt[binNo]);
                foreach (var site in sitelist)
                {
                    if (dataPrism.Summary.HBinCntBySite[site].TryGetValue(binNo, out int cnt))
                        csv.WriteField(cnt);
                    else
                        csv.WriteField(0);
                }
                csv.NextRecord();
            }

            // ── Step 6: 写入 SBin Statistic ─────────────────────────────
            var sbins = dataPrism.Summary.SBinCnt.Keys.ToList();
            sbins.Sort();
            sitelist = dataPrism.Summary.SBinCntBySite.Keys.ToList();
            sitelist.Sort();
            csv.WriteField("SBin Statistic:"); csv.NextRecord();
            csv.WriteField("SBin"); csv.WriteField("Name"); csv.WriteField("All"); foreach (var site in sitelist) csv.WriteField(site); csv.NextRecord();
            foreach (var binNo in sbins)
            {
                var binName = dataPrism.Summary.SBins[binNo].Name;
                csv.WriteField(binNo); csv.WriteField(binName); csv.WriteField(dataPrism.Summary.SBinCnt[binNo]);

                foreach (var site in sitelist)
                {
                    if (dataPrism.Summary.SBinCntBySite[site].TryGetValue(binNo, out int cnt))
                        csv.WriteField(cnt);
                    else
                        csv.WriteField(0);
                }
                csv.NextRecord();
            }

            csv.Flush();
        }
    }
}
