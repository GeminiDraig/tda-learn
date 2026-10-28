# TDA

> English | [简体中文](https://github.com/GeminiDraig/tda-learn/blob/master/packages/TDA/README.zh-CN.md)

**TDA** is a **.NET 10** library for reading, writing and analyzing **semiconductor test data (CP/FT)** stored in the **STDF (Standard Test Data Format)** format.

IO comes in two layers: `RecordReader` / `RecordWriter` work at **record level** (individual STDF records); `DataPrism` (formerly `StdfReader`) works at **whole-Lot file level** — loading, parsing, correlating multiple sublots and retest rounds, statistical analysis and export. `DataPrism` is the core of the library: after parsing STDF it organizes everything the files carry into one object model — lot / program / tester header information, per-part details with Hard/Soft Bins, test-item definitions with limits, raw per-part per-item test values, plus the computed statistics and yield.

Callers can access that data from any angle (by lot, by test item, by part, by site, by bin, by test round, or by DeviceID to follow one chip across rounds), and every angle offers both query and export-to-file APIs.

`DataPrism` takes either STDF files or a `DataPrismSetup` object as input. A `DataPrismSetup` is the organization of all STDF files of one Lot: the file paths of every TestRound of every Sublot, plus the retest relationships between rounds (which TestRound is the retest of which HardBins of which TestRound).

The TDA suite is published on NuGet as three packages:

| Package | Purpose |
|---|---|
| **TDA** | Core library — STDF parsing, analysis, import/export |
| [TDA.GUI](https://www.nuget.org/packages/TDA.GUI) | WinForms visualization controls — trend chart, histogram, box plot, probability plot, wafer map, data grid |
| [TDA.Report](https://www.nuget.org/packages/TDA.Report) | Report generation — outputs analysis results as reports (HTML / Excel available now, Pdf / Markdown planned) |

**Install**

```bash
dotnet add package TDA
dotnet add package TDA.GUI
dotnet add package TDA.Report
```

## Platform Requirements

- Target framework: `net10.0-windows10.0.19041` (Windows only).
- The package enables `<UseWindowsForms>true</UseWindowsForms>`; consuming projects must target `net10.0-windows*` on Windows and also enable `UseWindowsForms`. Linux / macOS TFMs are not supported.

## Features

- **STDF record-level engine** — 32 record types supported (FAR, MIR, SDR, WIR, WRR, PIR, PRR, PTR, MPR, FTR, HBR, SBR, TSR, …), with `RecordReader` / `RecordWriter` for full read → write round trips.
- **Compressed STDF** — reads `.stdf`, `.std`, `.gz`, `.zip`, `.tar.gz`/`.tgz`; two decompression modes: `Buffered` (fast) and `Streaming` (~70 KB memory even for multi-GB files).
- **DataPrism analysis engine** — one instance = one Lot: multi-sublot, first-test + multi-round retest correlation with per-round HardBin targeting, DeviceID tracking, CP wafer X/Y auto-detection.
- **Parallel statistics** — `DoStatisticAnalysis()` runs via `Parallel.ForEach` and computes, per test item: Mean / Median / Min / Max / Sigma, Cp / Cpk / Cpl / Cpu, 13 percentiles (P0.1 … P99.9), per-site statistics, and site-gap indices (SGM/SGI).
- **Import / Export** — export raw data (NEDA / RPV layout), parameter statistics, lot summary and TestID sets to CSV; import NEDA CSV back into a `DataPrism`.
- **JSON-configurable input** — `DataPrismSetup` (Sublot → TestRound hierarchy) serializes to/deserializes from JSON with relative-path normalization; TestID rename profiles from CSV/XLSX.

## Quick Start

### Load STDF data and analyze it

```csharp
using TDA;

// 1. Load one or more STDF files (.stdf/.std/.gz/.zip/.tar.gz, or a setup .json)
using var dp = DataPrism.FromFile(@"C:\data\lot001_sl00_ft1.stdf.gz");
dp.Load();
dp.DoStatisticAnalysis();   // mean/sigma, percentiles, Cp/Cpk, per-site, site-gap

// 2. Query lot-level info
Console.WriteLine($"LotID: {dp.DataSourcesInfoList[0].Mir.LOT_ID}");
Console.WriteLine($"Parts: {dp.PartsDescription.Length}, Test items: {dp.TestItems.Count}");

// 3. Export parameter statistics to CSV
DataPrism.ExportStatistics(dp, outfile: @"C:\output\param_stats.csv");

// 4. Export lot overview (yield, HardBin, SoftBin — all with per-site columns)
DataPrism.ExportSummary(dp, outfile: @"C:\output\lot_summary.csv");   // or ExportYield / ExportHBin / ExportSBin
```

### Multi-round retest configuration

```csharp
using TDA;

var sublot = new SublotSetup();
sublot.AddFirstTestRoundDataFiles("Init1.stdf", "Init2.stdf");
sublot.AddRetestRoundDataFiles(new[] { "Rt1.stdf" }, new ushort[] { 2, 3 }); // retest HardBin 2,3 only

var setup = new DataPrismSetup();
setup.Sublots.Add(sublot);
DataPrismSetup.SerializeToJsonFile(setup, "lot001.setup.json");   // reusable config

using var dp = new DataPrism(setup);
dp.Load();
dp.DoStatisticAnalysis();
```

### Export raw data (NEDA layout) for a TestID subset

```csharp
using TDA;
using TDA.TypeDef;

TestIDSet ids = dp.ExportTestIDSet(@"C:\output\testids.csv"); // full TestID list as template
DataPrism.ExportRawDatas(dp, ids, isFinalDataOnly: true,
    layout: RawDataLayout.NEDA, outfile: @"C:\output\raw_neda.csv");
```

## Key Types

| Type | Namespace | Purpose |
|---|---|---|
| `DataPrism` | `TDA` | Core engine: load, statistics, query, clone, import/export |
| `DataPrismSetup` / `SublotSetup` / `TestRoundSetup` | `TDA` | Input configuration hierarchy (JSON round-trip) |
| `TestID` | `TDA.TypeDef` | `(TestNumber, TestText, TestSequence)` triple identifying a test item |
| `TestItem` / `TestItemStatistics` | `TDA.TypeDef` | Per-test values + statistics (mean/sigma, percentiles, Cp/Cpk, per-site) |
| `TestIDSet` / `TestIDRenameSet` | `TDA.TypeDef` | TestID collections and rename profiles (CSV/XLSX) |
| `RecordReader` / `RecordWriter` | `TDA.StandardTestData` | Record-level STDF IO |
| `Msg` | `TDA.Utilities` | 6-level structured logging |

## Documentation

- **Examples & learning material** — the [tda-learn](https://github.com/GeminiDraig/tda-learn) repository: `Examples/Ex101.UseDataPrism`, `Examples/Ex102.ExportParamsStatistics`, `Examples/Ex103.ExportRawData`, `Examples/Ex105.ExportSummary`, `Examples/Ex204.CfgFileForDataPrismSetup`, `Examples/Ex301.RecordReaderBasics`, `Examples/Ex501.UseRecordWriter`.
- **Command-line usage** — the `CLI/` folder of the same repository holds the `dp` manual and per-command reference.
- **Design notes** (architecture, compressed-STDF internals, …) are maintained in the private source repository and are not published.
- API documentation is embedded as XML doc comments — hover in your IDE.

## Links

- GitHub: https://github.com/GeminiDraig/tda-learn
- GitCode: https://gitcode.com/GeminiDraig/tda-learn
- NuGet: https://www.nuget.org/packages/TDA

## License

© TDA Project Contributors
