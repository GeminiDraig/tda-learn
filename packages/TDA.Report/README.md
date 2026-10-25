# TDA.Report

> English | [简体中文](https://github.com/GeminiDraig/tda-learn/blob/master/packages/TDA.Report/README.zh-CN.md)

**TDA.Report** is the report-generation package of **TDA**, a **.NET 10** framework for **STDF (Standard Test Data Format) semiconductor test data** (**CP/FT**) analysis. It converts `DataPrism` analysis results — yield, bin distributions, parameter statistics (mean/sigma, percentiles, Cp/Cpk, …), anomaly screening, **Wafer Map** / histogram charts — into human-readable reports, with **HTML** as the core format and Excel as a data attachment.

## Install

```bash
dotnet add package TDA.Report
```

TDA.Report consumes the [TDA](https://www.nuget.org/packages/TDA) core package; [TDA.GUI](https://www.nuget.org/packages/TDA.GUI) provides the interactive counterpart.

## Features

- **HTML as the core format** — a single self-contained file, zero JavaScript, native `<details>/<summary>` folding, charts embedded as base64 PNG data URIs.
- **Excel (.xlsx) output** — one sheet per report section plus a Charts sheet with embedded PNGs (MiniExcel-based).
- **Built-in anomaly screening** — 4 default rules (Cpk < 1.33, OOS rate > 0.1%, Mean shift > 50%, distribution skew > 0.3), severity-sorted, extensible via `ReportOptions.AnomalyRules`.
- **Offscreen chart rendering** — `ChartService` renders histograms, box plots, trend charts and CDF plots to PNG via ScottPlot, no UI required.
- **Lot yield report template** — anomaly overview first, then lot/yield summary, HBin/SBin distributions (Top Fail highlighted), parameter statistics, optional charts.
- Pdf / Markdown output is planned (not yet implemented).

## Platform Requirements

- Target framework: `net10.0-windows10.0.19041` (Windows only, follows the TDA core library).
- Depends on the [TDA](https://www.nuget.org/packages/TDA) core package, MiniExcel and ScottPlot (core, offscreen rendering only — no WinForms dependency).

## Quick Start

```csharp
using TDA;
using TDA.Report;
using TDA.Report.Adapters;
using TDA.Report.Charts;
using TDA.Report.Document.Blocks;
using TDA.Report.Templates;

// 1. Load and analyze STDF data with the TDA core
using var dp = DataPrism.FromFile(@"C:\data\lot001_ft1.stdf.gz");
dp.Load();
dp.DoStatisticAnalysis();

// 2. Adapt to a structured summary (anomaly rules evaluated here)
var options = new ReportOptions { Title = "Lot001 Yield Report" };
var summary = DataPrismAdapter.ToReportSummary(dp, options.Title, options);

// 3. Render parameter charts to PNG (optional)
var chartService = new ChartService(dp);
var charts = new List<ChartBlock>
{
    new("VDD Histogram", new ChartSpec(ChartKind.Histogram, "1000_VDD_0", 800, 450)),
    new("VREF BoxPlot",   new ChartSpec(ChartKind.BoxPlot,   "1001_VREF_0", 800, 450)),
};
foreach (var chart in charts)
    chart.ImagePath = chartService.RenderToPng(chart.Spec, @"C:\output\charts");

// 4. Assemble the template and render a self-contained HTML report
string htmlPath = LotYieldReport.RenderToFile(summary, options, @"C:\output\lot001.html", charts);
```

To render Excel instead/as well:

```csharp
using TDA.Report.Render;

var doc = LotYieldReport.Build(summary, options, charts);
new ExcelRenderer().RenderToFile(doc, @"C:\output\lot001.xlsx");
```

> TestID key format for `ChartSpec`: `"{TestNumber}_{TestText}_{TestSequence}"`.

## Architecture

```
DataPrism (TDA core)
   └─ DataPrismAdapter → ReportSummary (+ anomaly rules)
        └─ LotYieldReport (template) → ReportDocument (renderer-agnostic model)
             └─ IRenderer: HtmlRenderer (core) · ExcelRenderer · PdfRenderer (Phase 2)
ChartService (ScottPlot offscreen PNG) → ChartBlock
```

Key types: `DataPrismAdapter`, `LotYieldReport`, `ReportSummary`, `ReportOptions`, `AnomalyRules`, `ChartService`, `ChartSpec`/`ChartKind`, `ReportDocument`, `HtmlRenderer`, `ExcelRenderer`.

## Documentation

- Runnable example in the [tda-learn](https://github.com/GeminiDraig/tda-learn) repository: `Examples/Ex701.LotYieldReport`.
- Full design doc (roadmap: LotYieldReport implemented; ParametricReport in Phase 2) is maintained in the private source repository and is not published.

## Links

- GitHub: https://github.com/GeminiDraig/tda-learn
- GitCode: https://gitcode.com/GeminiDraig/tda-learn
- NuGet: https://www.nuget.org/packages/TDA.Report

## License

© TDA Project Contributors
