# TDA.GUI

> [English](https://github.com/GeminiDraig/tda-learn/blob/master/packages/TDA.GUI/README.md) | 简体中文

**TDA.GUI** 是 **TDA** 的 WinForms 可视化包——TDA 是一个基于 **.NET 10** 的半导体测试数据框架，用于 **STDF（Standard Test Data Format）半导体测试数据（CP/FT）** 分析。提供交互式趋势图、带统计量标注的直方图、箱线图、累积概率图（Normal/LogNormal/Weibull）、彩色 Bin 渲染的 **Wafer Map**、图表联动，以及高性能虚拟数据表格——基于 ScottPlot 与 SkiaSharp 构建。

## 安装

```bash
dotnet add package TDA.GUI
```

TDA.GUI 依赖 [TDA](https://www.nuget.org/packages/TDA) 核心包；[TDA.Report](https://www.nuget.org/packages/TDA.Report) 在其之上提供报告生成能力。

## 控件一览

所有图表继承 `TDABaseChart`（标题/轴标签、规格线、`SaveImage` / `CopyImage` / `ExportCsv`、自动标题与图例），消费与 UI 无关的数据模型 `TDADataCollection` / `ITDADataSeries`——不直接依赖 `DataPrism`。

| 控件 | 命名空间 | 用途 |
|---|---|---|
| `TrendChart` | `TDA.GUI.Controls` | 趋势图：按芯片测试顺序展示测试值，散点 + USL/LSL 规格线 |
| `HistogramChart` | `TDA.GUI.Controls` | 直方图：可切换分箱算法，叠加正态拟合曲线与 N/Mean/StdDev/Cp/Cpk 标注 |
| `BoxPlotChart` | `TDA.GUI.Controls` | 箱线图（Tukey 须线）：箱体、中位线、均值菱形、离群点 |
| `CumulativeProbabilityChart` | `TDA.GUI.Controls` | 概率纸 CDF：Normal / LogNormal / Weibull 刻度，可选尾部拉伸 |
| `WaferMapChart` | `TDA.GUI.Controls` | 晶圆图：Bin 彩色编码、图例面板、悬停显示 die 信息（XY/Idx/HBIN/SBIN/Site）、复测显示模式 |
| `TDADataGrid` | `TDA.GUI.Controls` | 只读虚拟化表格（SkiaSharp 渲染，GPU 加速 + CPU 回退）：冻结窗格、旋转表头、排序、条件格式、Fail 单元格导航 |
| `EasyPlotForm` / `FlexiblePlotForm` | `TDA.GUI` | 开箱即用的绘图窗体：固定 5 页签布局，或 1~4 图动态布局 |

配套组件：`BinCalculator`（Sturges / Scott / Freedman-Diaconis / 自定义分箱）、`ProbabilityScale`（Benard 中位秩 ECDF）、`StatisticsHelper`（均值/标准差、分位数、Cp/Cpk、箱线统计）、`WaferCoordinateHelper`、`ChartLinkingManager`（多图表选区联动）。

## 平台要求

- 目标框架：`net10.0-windows10.0.19041`（仅 Windows），启用 `UseWindowsForms`——引用方项目必须同样设置。
- 依赖 [TDA](https://www.nuget.org/packages/TDA) 核心包、ScottPlot.WinForms 与 SkiaSharp。

## 快速上手

### 绘制直方图

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
histogram.SetData(data);      // 分箱 + 正态拟合 + 规格线 + 统计量标注
histogram.Show();
```

### 一行打开绘图窗体

```csharp
using TDA.GUI;

var form = new EasyPlotForm();        // 趋势 / 直方图 / CDF / 箱线图 / 晶圆图 五个页签
form.PlotHistogram(data);
form.PlotTrend(data);
Application.Run(form);

// 或按需动态组合布局：
var flex = new FlexiblePlotForm(ChartLayout.Trend | ChartLayout.Histogram);
flex.LoadData(data);
flex.Show();
```

### 晶圆图（CP 数据）

```csharp
using TDA.GUI.Controls;

var waferMap = new WaferMapChart { RtDataMode = MapRtDataMode.OverWrite }; // 复测：以最终结果为准
waferMap.SetData(waferMapData);     // WaferMapData：die 数据 + Bin 信息，Pass Bin 亮绿色
waferMap.Show();
```

### 多图表选区联动

```csharp
using TDA.GUI.Linking;

var linker = new ChartLinkingManager();
linker.Register(trendChart);
linker.Register(histogramChart);    // 在一个图中框选范围，另一个图同步高亮
```

## 文档

- 可运行示例（[tda-learn](https://github.com/GeminiDraig/tda-learn) 仓库）：`Examples/Ex601.GUICharts`、`Examples/Ex602.GUIEasyPlotForm`、`Examples/Ex603.FlexiblePlotForm`。
- 设计文档（TDADataGrid 虚拟化、图表联动等）保留在内部源码仓，不公开发布。
- API 文档内嵌于 XML 注释，IDE 悬停即可查看。

## 链接

- GitHub：https://github.com/GeminiDraig/tda-learn
- GitCode：https://gitcode.com/GeminiDraig/tda-learn
- NuGet：https://www.nuget.org/packages/TDA.GUI

## 许可证

© TDA Project Contributors
