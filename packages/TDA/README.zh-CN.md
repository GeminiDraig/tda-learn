# TDA

> [English](https://github.com/GeminiDraig/tda-learn/blob/master/packages/TDA/README.md) | 简体中文

**TDA** 是一个基于 **.NET 10** 的半导体测试数据读写与分析库，面向 **STDF（Standard Test Data Format）** 格式的**半导体测试数据（CP/FT）**，提供读写、解析、统计分析与导入导出能力。

读写能力分两层：`RecordReader` / `RecordWriter` 面向**记录级**（单条 STDF 记录）读写；`DataPrism`（原 `StdfReader`）面向 **Lot 整批文件级**——读取、解析、多 Sublot 与多轮复测关联、统计分析与导出。其中 `DataPrism` 是整个库的核心：它解析 STDF 后，把文件中承载的全部信息组织成一个对象模型——批次 / 程序 / 机台等头部信息、每颗芯片的 Part 明细与 Hard/Soft Bin、测试项定义与上下限（Limit）、逐颗逐项的原始测试值，以及算出的统计量与良率。

调用方可以按任意视角访问这些数据（按批次、按测试项、按 Part、按 Site、按 Bin、按测试轮次，或通过 DeviceID 追踪同一颗芯片跨轮次的数据），且每个视角都同时提供取数与导出到文件的 API。

`DataPrism` 以 STDF 文件或 `DataPrismSetup` 对象为输入。`DataPrismSetup` 是「一个 Lot 的所有 STDF 文件的组织对象」：包含所有 Sublot 下所有 TestRound 的 STDF 文件路径，以及 TestRound 之间的复测关系（哪个 TestRound 是哪一个 TestRound 的哪些 HardBin 的复测）。


**TDA 套件在 NuGet 上发布为三个包：**

| 包                                                     | 用途                                                                    |
| ------------------------------------------------------ | ----------------------------------------------------------------------- |
| **TDA**                                          | 核心库——STDF 解析、分析、导入导出                                     |
| [TDA.GUI](https://www.nuget.org/packages/TDA.GUI)       | WinForms 可视化控件——趋势图、直方图、箱线图、概率图、晶圆图、数据表格 |
| [TDA.Report](https://www.nuget.org/packages/TDA.Report) | 报告生成——把分析结果输出为报告（HTML / Excel 现已可用，Pdf / Markdown 规划中）        |


**安装**

```bash
dotnet add package TDA
dotnet add package TDA.GUI
dotnet add package TDA.Report
```

## 平台要求

- 目标框架：`net10.0-windows10.0.19041`（仅 Windows）。
- 本包启用了 `<UseWindowsForms>true</UseWindowsForms>`，引用方项目必须是 Windows 上的 `net10.0-windows*` 且同样启用 `UseWindowsForms`；Linux / macOS TFM 无法引用。

## 功能特性

- **STDF 记录级引擎** — 支持 32 种记录类型（FAR、MIR、SDR、WIR、WRR、PIR、PRR、PTR、MPR、FTR、HBR、SBR、TSR……），`RecordReader` / `RecordWriter` 支持完整的读 → 写往返。
- **压缩 STDF** — 直接读取 `.stdf`、`.std`、`.gz`、`.zip`、`.tar.gz`/`.tgz`；两种解压模式：`Buffered`（快速）与 `Streaming`（数 GB 文件仅约 70 KB 内存）。
- **DataPrism 分析引擎** — 一个实例 = 一个 Lot：多 Sublot、初测 + 多轮复测关联（每轮可指定复测 HardBin）、DeviceID 跟踪、CP 晶圆 X/Y 坐标自动识别。
- **并行统计分析** — `DoStatisticAnalysis()` 基于 `Parallel.ForEach`，为每个测试项计算：Mean / Median / Min / Max / Sigma、Cp / Cpk / Cpl / Cpu、13 个百分位（P0.1 … P99.9）、分 Site 统计，以及 Site 间差指标（SGM/SGI）。
- **导入 / 导出** — 原始数据（NEDA / RPV 布局）、参数统计、Lot 汇总、TestID 集合导出为 CSV；支持从 NEDA CSV 反向导入构建 `DataPrism`。
- **JSON 可配置输入** — `DataPrismSetup`（Sublot → TestRound 层级）支持 JSON 序列化/反序列化与相对路径规范化；TestID 改名配置支持 CSV/XLSX。

## 快速上手

### 加载 STDF 数据并分析

```csharp
using TDA;

// 1. 加载一个或多个 STDF 文件（.stdf/.std/.gz/.zip/.tar.gz，或 setup .json）
using var dp = DataPrism.FromFile(@"C:\data\lot001_sl00_ft1.stdf.gz");
dp.Load();
dp.DoStatisticAnalysis();   // 均值/标准差、百分位、Cp/Cpk、分 Site、Site 间差

// 2. 查询批次级信息
Console.WriteLine($"LotID: {dp.DataSourcesInfoList[0].Mir.LOT_ID}");
Console.WriteLine($"Parts: {dp.PartsDescription.Length}, Test items: {dp.TestItems.Count}");

// 3. 导出参数统计到 CSV
DataPrism.ExportStatistics(dp, outfile: @"C:\output\param_stats.csv");

// 4. 导出批次总览（良率 / HardBin / SoftBin，均含各 Site 列）
DataPrism.ExportSummary(dp, outfile: @"C:\output\lot_summary.csv");   // 也可单独 ExportYield / ExportHBin / ExportSBin
```

### 多轮复测配置

```csharp
using TDA;

var sublot = new SublotSetup();
sublot.AddFirstTestRoundDataFiles("Init1.stdf", "Init2.stdf");
sublot.AddRetestRoundDataFiles(new[] { "Rt1.stdf" }, new ushort[] { 2, 3 }); // 仅复测 HardBin 2,3

var setup = new DataPrismSetup();
setup.Sublots.Add(sublot);
DataPrismSetup.SerializeToJsonFile(setup, "lot001.setup.json");   // 配置可复用

using var dp = new DataPrism(setup);
dp.Load();
dp.DoStatisticAnalysis();
```

### 按 TestID 子集导出原始数据（NEDA 布局）

```csharp
using TDA;
using TDA.TypeDef;

TestIDSet ids = dp.ExportTestIDSet(@"C:\output\testids.csv"); // 导出完整 TestID 清单作模板
DataPrism.ExportRawDatas(dp, ids, isFinalDataOnly: true,
    layout: RawDataLayout.NEDA, outfile: @"C:\output\raw_neda.csv");
```

## 核心类型

| 类型                                                      | 命名空间                 | 用途                                                          |
| --------------------------------------------------------- | ------------------------ | ------------------------------------------------------------- |
| `DataPrism`                                             | `TDA`                  | 核心引擎：加载、统计、查询、克隆、导入导出                    |
| `DataPrismSetup` / `SublotSetup` / `TestRoundSetup` | `TDA`                  | 输入配置层级（JSON 往返）                                     |
| `TestID`                                                | `TDA.TypeDef`          | `(TestNumber, TestText, TestSequence)` 三元组唯一标识测试项 |
| `TestItem` / `TestItemStatistics`                     | `TDA.TypeDef`          | 单项测试值与统计量（均值/标准差、百分位、Cp/Cpk、分 Site）    |
| `TestIDSet` / `TestIDRenameSet`                       | `TDA.TypeDef`          | TestID 集合与改名配置（CSV/XLSX）                             |
| `RecordReader` / `RecordWriter`                       | `TDA.StandardTestData` | 记录级 STDF 读写                                              |
| `Msg`                                                   | `TDA.Utilities`        | 6 级结构化日志                                                |

## 文档

- **示例与学习材料** —— [tda-learn](https://github.com/GeminiDraig/tda-learn) 仓库：`Examples/Ex101.UseDataPrism`、`Examples/Ex102.ExportParamsStatistics`、`Examples/Ex103.ExportRawData`、`Examples/Ex105.ExportSummary`、`Examples/Ex204.CfgFileForDataPrismSetup`、`Examples/Ex301.RecordReaderBasics`、`Examples/Ex501.UseRecordWriter`。
- **命令行用法** —— 同一仓库的 `CLI/` 目录是 `dp` 手册与逐命令参考，见 [CLI/README.md](https://github.com/GeminiDraig/tda-learn)。
- **设计文档**（架构、压缩 STDF 内部实现等）保留在内部源码仓，不公开发布。
- API 文档内嵌于 XML 注释，IDE 悬停即可查看。

## 链接

- GitHub：https://github.com/GeminiDraig/tda-learn
- GitCode：https://gitcode.com/GeminiDraig/tda-learn
- NuGet：https://www.nuget.org/packages/TDA

## 许可证

© TDA Project Contributors
