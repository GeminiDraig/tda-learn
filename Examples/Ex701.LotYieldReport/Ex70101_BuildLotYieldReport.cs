using System.Globalization;
using System.Text;
using TDA;
using TDA.Report;
using TDA.Report.Adapters;
using TDA.Report.Charts;
using TDA.Report.Document;
using TDA.Report.Document.Blocks;
using TDA.Report.Templates;

namespace Ex701.LotYieldReport
{
    /// <summary>
    /// Ex70101：LotYieldReport 全流程 —— 加载测试数据 → 适配为结构化摘要 → 渲染参数图表 → 模板装配 → 渲染 HTML。
    /// 图表由 <see cref="ChartService"/> 直出 PNG，HtmlRenderer 以 base64 data URI 内嵌，报告单文件自包含。
    /// </summary>
    internal static class Ex70101_BuildLotYieldReport
    {
        /// <summary>
        /// 执行演示。
        /// </summary>
        /// <param name="inputPath">STDF 或 NEDA CSV 路径；null 或 "--demo" 时生成演示数据。</param>
        /// <param name="outputPath">HTML 输出路径；null 时由模板解析默认文件名。</param>
        public static void Execute(string? inputPath, string? outputPath)
        {
            var options = new ReportOptions
            {
                Title = "LotYieldReport Demo",
                TopFailCount = 3,
            };

            // 1. 加载数据
            DataPrism dataPrism = null!;
            string source;
            if (string.IsNullOrWhiteSpace(inputPath) || inputPath == "--demo")
            {
                var demoPath = Path.Combine(Environment.CurrentDirectory, "Ex701_Demo.neda_raw.csv");
                WriteDemoNedaCsv(demoPath);
                dataPrism = DataPrism.ImportFromNEDA(demoPath);
                source = demoPath;
                Console.WriteLine($"[1/4] Loaded demo NEDA data: {demoPath}");
            }
            else
            {
                dataPrism = inputPath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)
                    ? DataPrism.ImportFromNEDA(inputPath)
                    : DataPrism.FromFile(inputPath);
                source = inputPath;
                Console.WriteLine($"[1/4] Loaded data: {source}");
            }

            // 2. 适配为结构化摘要（含异常规则求值）
            var summary = DataPrismAdapter.ToReportSummary(dataPrism, options.Title, options);
            Console.WriteLine($"[2/4] Summary: Total={summary.Lot.TotalQty} Pass={summary.Lot.PassQty} " +
                              $"FinalYield={summary.Lot.FinalYield:P1} Anomalies={summary.Anomalies.Count}");

            // 3. 渲染参数图表：ChartService → PNG 文件 → ChartBlock 列表
            // TestID 键 = TestID.ToString() = "{TestNumber}_{TestText}_{TestSequence}"
            // VDD=1000（含 fail 值 2.5 → 直方图可见异常簇）、VREF=1001
            var chartDir = Path.Combine(Environment.CurrentDirectory, "chart_out");
            var chartService = new ChartService(dataPrism);
            var charts = new List<ChartBlock>
            {
                new("VDD 直方图", new ChartSpec(ChartKind.Histogram, "1000_VDD_0", 800, 450)),
                new("VDD 趋势图", new ChartSpec(ChartKind.Trend, "1000_VDD_0", 800, 450)),
                new("VREF 箱线图", new ChartSpec(ChartKind.BoxPlot, "1001_VREF_0", 800, 450)),
                new("VREF 累积概率图", new ChartSpec(ChartKind.Cdf, "1001_VREF_0", 800, 450)),
            };
            foreach (var chart in charts)
            {
                chart.ImagePath = chartService.RenderToPng(chart.Spec, chartDir);
            }
            Console.WriteLine($"[3/4] Rendered {charts.Count} charts to: {chartDir}");

            // 4. 模板装配 + 渲染 HTML（图表以 base64 data URI 内嵌，单文件自包含）
            // 完全限定名：示例命名空间 Ex701.LotYieldReport 与模板类同名，避免解析歧义
            var htmlPath = TDA.Report.Templates.LotYieldReport.RenderToFile(summary, options, outputPath, charts);
            Console.WriteLine($"[4/4] Report written: {htmlPath}");
        }

        /// <summary>
        /// 生成演示 NEDA CSV：2 个参数型测试（VDD / VREF）+ 300 颗芯片，90% 良率。
        /// VREF 均值 ≈1.4（规格中心 1.0）→ 命中 MeanShift 异常，便于观察报告异常总览。
        /// 注：NEDA CSV 无 HBR/SBR 记录，导入后 HBin/SBin 分布表为空；真实 STDF 数据不受此限制。
        /// </summary>
        private static void WriteDemoNedaCsv(string path)
        {
            const string rowHeaderNames = "ID,LID,FileName,StartTime,LOT_ID,WAFER_ID,PART_ID,HEAD_NUM,SITE_NUM,X_COORD,Y_COORD,HARD_BIN,SOFT_BIN,PART_FLG,TEST_T,PART_TEXT,CHIP_TEXT,FIRST_FAIL";
            const string pad = ",,,,,,,,,,,,,,,,,,"; // 17 个空列 → 补齐 18 个 RowHeader 占位

            var sb = new StringBuilder();
            sb.AppendLine($"Program:{pad},TEST,TEST");
            sb.AppendLine($"Test Name{pad},VDD,VREF");
            sb.AppendLine($"Test Number{pad},1000,1001");
            sb.AppendLine($"Lower Limit{pad},0,0.5");
            sb.AppendLine($"Upper Limit{pad},2,1.5");
            sb.AppendLine($"Unit{pad},V,V");
            sb.AppendLine($"{rowHeaderNames},,");

            var rng = new Random(42);
            for (int i = 0; i < 300; i++)
            {
                bool fail = i % 10 == 0;                                  // 每 10 颗 1 颗 fail → 90% 良率
                double vdd = fail ? 2.5 : 0.9 + rng.NextDouble() * 0.5;   // fail 超上限 2.0；pass 在 0.9..1.4
                double vref = 1.4 + rng.NextDouble() * 0.05;              // 1.40..1.45（规格中心 1.0 → 偏移）
                int hbin = fail ? 2 : 1;
                sb.Append(CultureInfo.InvariantCulture,
                    $"{i},0,fixture,2026-08-21 10:00:00,DEMO-LOT,W0,PART{i:D4},True,0,0,0,{hbin},{hbin},0,100,,,0,");
                sb.Append(CultureInfo.InvariantCulture, $"{vdd},{vref}");
                sb.AppendLine();
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
