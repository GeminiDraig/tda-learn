# TDA.Report

> [English](https://github.com/GeminiDraig/tda-learn/blob/master/packages/TDA.Report/README.md) | 简体中文

**TDA.Report** 是 **TDA** 的报告生成包——TDA 是一个基于 **.NET 10** 的半导体测试数据框架，用于 **STDF（Standard Test Data Format）半导体测试数据（CP/FT）** 分析。它把 `DataPrism` 的分析结果——良率、Bin 分布、参数统计（均值/标准差、百分位、Cp/Cpk……）、异常筛查、**Wafer Map** / 直方图等图表——转换为可读报告，以 **HTML** 为核心格式，Excel 作为数据附件。

## 安装

```bash
dotnet add package TDA.Report
```

TDA.Report 依赖 [TDA](https://www.nuget.org/packages/TDA) 核心包；[TDA.GUI](https://www.nuget.org/packages/TDA.GUI) 是与之对应的交互式可视化包。

## 功能特性

- **HTML 为核心格式** —— 单文件自包含、零 JavaScript、原生 `<details>/<summary>` 折叠、图表以 base64 PNG data URI 内嵌。
- **Excel（.xlsx）输出** —— 每个报告 Section 一个 Sheet，另有 Charts 页内嵌 PNG（基于 MiniExcel）。
- **内置异常筛查** —— 4 条默认规则（Cpk < 1.33、OOS 率 > 0.1%、Mean 偏移 > 50%、分布偏斜 > 0.3），按严重度排序，可通过 `ReportOptions.AnomalyRules` 扩展。
- **离屏图表渲染** —— `ChartService` 用 ScottPlot 离屏直出 PNG（直方图、箱线图、趋势图、CDF），无需 UI。
- **Lot 良率报告模板** —— 异常总览优先展示，其后是 Lot/良率概要、HBin/SBin 分布（Top Fail 高亮）、参数统计与可选图表。
- Pdf / Markdown 输出规划中（尚未实现）。

## 平台要求

- 目标框架：`net10.0-windows10.0.19041`（仅 Windows，跟随 TDA 核心库）。
- 依赖 [TDA](https://www.nuget.org/packages/TDA) 核心包、MiniExcel 与 ScottPlot（核心包，仅离屏渲染——无 WinForms 依赖）。

## 快速上手

```csharp
using TDA;
using TDA.Report;
using TDA.Report.Adapters;
using TDA.Report.Charts;
using TDA.Report.Document.Blocks;
using TDA.Report.Templates;

// 1. 用 TDA 核心库加载并分析 STDF 数据
using var dp = DataPrism.FromFile(@"C:\data\lot001_ft1.stdf.gz");
dp.Load();
dp.DoStatisticAnalysis();

// 2. 适配为结构化摘要（异常规则在此求值）
var options = new ReportOptions { Title = "Lot001 良率报告" };
var summary = DataPrismAdapter.ToReportSummary(dp, options.Title, options);

// 3. 渲染参数图表为 PNG（可选）
var chartService = new ChartService(dp);
var charts = new List<ChartBlock>
{
    new("VDD 直方图", new ChartSpec(ChartKind.Histogram, "1000_VDD_0", 800, 450)),
    new("VREF 箱线图", new ChartSpec(ChartKind.BoxPlot,   "1001_VREF_0", 800, 450)),
};
foreach (var chart in charts)
    chart.ImagePath = chartService.RenderToPng(chart.Spec, @"C:\output\charts");

// 4. 模板装配 + 渲染自包含 HTML 报告
string htmlPath = LotYieldReport.RenderToFile(summary, options, @"C:\output\lot001.html", charts);
```

如需同时输出 Excel：

```csharp
using TDA.Report.Render;

var doc = LotYieldReport.Build(summary, options, charts);
new ExcelRenderer().RenderToFile(doc, @"C:\output\lot001.xlsx");
```

> `ChartSpec` 的 TestID 键格式：`"{TestNumber}_{TestText}_{TestSequence}"`。

## 架构

```
DataPrism（TDA 核心库）
   └─ DataPrismAdapter → ReportSummary（+ 异常规则）
        └─ LotYieldReport（模板）→ ReportDocument（渲染器无关模型）
             └─ IRenderer：HtmlRenderer（核心）· ExcelRenderer · PdfRenderer（Phase 2）
ChartService（ScottPlot 离屏 PNG）→ ChartBlock
```

核心类型：`DataPrismAdapter`、`LotYieldReport`、`ReportSummary`、`ReportOptions`、`AnomalyRules`、`ChartService`、`ChartSpec`/`ChartKind`、`ReportDocument`、`HtmlRenderer`、`ExcelRenderer`。

## 文档

- 可运行示例（[tda-learn](https://github.com/GeminiDraig/tda-learn) 仓库）：`Examples/Ex701.LotYieldReport`。
- 完整设计文档（报告路线图：LotYieldReport 已实现，ParametricReport 规划在 Phase 2）保留在内部源码仓，不公开发布。

## 链接

- GitHub：https://github.com/GeminiDraig/tda-learn
- GitCode：https://gitcode.com/GeminiDraig/tda-learn
- NuGet：https://www.nuget.org/packages/TDA.Report

## 许可证

© TDA Project Contributors
