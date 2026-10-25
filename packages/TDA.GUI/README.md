# TDA.GUI

> English | [简体中文](https://github.com/GeminiDraig/tda-learn/blob/master/packages/TDA.GUI/README.zh-CN.md)

**TDA.GUI** is the WinForms visualization package of **TDA**, a **.NET 10** framework for **STDF (Standard Test Data Format) semiconductor test data** (**CP/FT**) analysis. It provides interactive trend charts, histograms with statistics annotation, box plots, cumulative probability plots (Normal/LogNormal/Weibull), color-coded **Wafer Maps**, chart linking, and a high-performance virtual data grid — built on ScottPlot and SkiaSharp.

## Install

```bash
dotnet add package TDA.GUI
```

TDA.GUI consumes the [TDA](https://www.nuget.org/packages/TDA) core package; [TDA.Report](https://www.nuget.org/packages/TDA.Report) adds report generation on top of it.

## Controls

All charts inherit from `TDABaseChart` (title/axis labels, spec-limit lines, `SaveImage` / `CopyImage` / `ExportCsv`, auto title & legend) and consume the UI-agnostic data model `TDADataCollection` / `ITDADataSeries` — not `DataPrism` directly.

| Control | Namespace | Purpose |
|---|---|---|
| `TrendChart` | `TDA.GUI.Controls` | Run chart of values vs. test order, marker scatter + USL/LSL lines |
| `HistogramChart` | `TDA.GUI.Controls` | Histogram with switchable binning, Normal-fit curve, N/Mean/StdDev/Cp/Cpk annotation |
| `BoxPlotChart` | `TDA.GUI.Controls` | Tukey box plot per series (box, median, mean diamond, outliers) |
| `CumulativeProbabilityChart` | `TDA.GUI.Controls` | Probability-paper CDF: Normal / LogNormal / Weibull scales, optional tail stretching |
| `WaferMapChart` | `TDA.GUI.Controls` | Wafer map with bin color coding, legend panel, hover die info (XY/Idx/HBIN/SBIN/Site), retest display modes |
| `TDADataGrid` | `TDA.GUI.Controls` | Read-only virtualized grid rendered via SkiaSharp (GPU with CPU fallback): frozen panes, rotated headers, sorting, conditional formatting, fail-cell navigation |
| `EasyPlotForm` / `FlexiblePlotForm` | `TDA.GUI` | Ready-made plot windows: fixed 5-tab layout, or 1–4 charts with auto layout |

Supporting pieces: `BinCalculator` (Sturges / Scott / Freedman-Diaconis / custom), `ProbabilityScale` (Benard median-rank ECDF), `StatisticsHelper` (mean/sigma, quantiles, Cp/Cpk, box-plot stats), `WaferCoordinateHelper`, `ChartLinkingManager` (selection sync across charts).

## Platform Requirements

- Target framework: `net10.0-windows10.0.19041` (Windows only), `UseWindowsForms` enabled — the consuming project must do the same.
- Depends on the [TDA](https://www.nuget.org/packages/TDA) core package, ScottPlot.WinForms and SkiaSharp.

## Quick Start

### Plot a histogram

```csharp
using TDA.GUI.Controls;
using TDA.GUI.DataModel;

var data = new TDADataCollection
{
    USL = 1.2, LSL = 0.8, Unit = "V",
    Series =
    {
        new TDADataSeries
        {
            Name = "LotA-Wafer01-Vth",
            Values = new[] { 0.52, 0.49, 0.55, 0.51 /* ... */ },
            Type = DataSeriesType.Raw,
            Metadata = new Dictionary<string, object>
            {
                ["LotId"] = "LotA", ["WaferId"] = "Wafer01", ["Unit"] = "V"
            }
        }
    }
};

var histogram = new HistogramChart { BinMethod = BinMethod.Scott };
histogram.SetData(data);      // bins + Normal fit + spec lines + statistics annotation
histogram.Show();
```

### One-line plot windows

```csharp
using TDA.GUI;

var form = new EasyPlotForm();        // Trend / Histogram / CDF / BoxPlot / WaferMap tabs
form.PlotHistogram(data);
form.PlotTrend(data);
Application.Run(form);

// or pick a layout dynamically:
var flex = new FlexiblePlotForm(ChartLayout.Trend | ChartLayout.Histogram);
flex.LoadData(data);
flex.Show();
```

### Wafer map (CP data)

```csharp
using TDA.GUI.Controls;

var waferMap = new WaferMapChart { RtDataMode = MapRtDataMode.OverWrite }; // retest: last result wins
waferMap.SetData(waferMapData);     // WaferMapData: dies + bin info, pass bins bright green
waferMap.Show();
```

### Link selections across charts

```csharp
using TDA.GUI.Linking;

var linker = new ChartLinkingManager();
linker.Register(trendChart);
linker.Register(histogramChart);    // selecting a range in one highlights it in the other
```

## Documentation

- Runnable demos in the [tda-learn](https://github.com/GeminiDraig/tda-learn) repository: `Examples/Ex601.GUICharts`, `Examples/Ex602.GUIEasyPlotForm`, `Examples/Ex603.FlexiblePlotForm`.
- Design notes (TDADataGrid virtualization, chart linking, …) are maintained in the private source repository and are not published.
- API documentation is embedded as XML doc comments — hover in your IDE.

## Links

- GitHub: https://github.com/GeminiDraig/tda-learn
- GitCode: https://gitcode.com/GeminiDraig/tda-learn
- NuGet: https://www.nuget.org/packages/TDA.GUI

## License

© TDA Project Contributors
