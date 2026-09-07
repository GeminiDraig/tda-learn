# tda-examples

**TDA Framework 官方示例与学习仓库** — 半导体 STDF 数据分析入门到实战。

[TDA Framework](https://www.nuget.org/packages/TDA) 是一个面向半导体量产测试数据分析的 .NET 框架（STDF V4 解析、多 Sublot / 复测数据关联、统计分析与 Cp/Cpk、NEDA CSV / Excel 导入导出、WinForms 可视化等），核心库以 NuGet 包形式公开发布：

| 包 | 说明 | NuGet |
| --- | --- | --- |
| `TDA` | 核心库：STDF 解析 / 数据模型 / 统计分析 / 导入导出 | [nuget.org/packages/TDA](https://www.nuget.org/packages/TDA) |
| `TDA.GUI` | WinForms 可视化控件（趋势 / 直方图 / 累积概率 / 箱线图 / 晶圆图） | [nuget.org/packages/TDA.GUI](https://www.nuget.org/packages/TDA.GUI) |
| `TDA.Report` | HTML / Excel 报告生成 | [nuget.org/packages/TDA.Report](https://www.nuget.org/packages/TDA.Report) |

## 仓库结构

```
tda-examples/
├── Examples/   # 可运行示例代码（与 TDA 内部仓库 1:1 镜像，NuGet 引用方式）
├── CLI/        # dp 命令行工具手册：零代码调用 DataPrism（公开仓直接维护，见 CLI/README.md）
├── Docs/       # 教程文档（规划中）
├── Skills/     # AI Skills：面向常见 TDA 使用场景（规划中）
├── tools/      # 维护脚本（ProjectReference → NuGet 引用转换等）
├── Directory.Build.props   # 统一 TargetFramework
└── tda-examples.slnx       # 全部示例的解决方案
```

## CLI 工具（dp）：不写代码，直接分析 STDF

[dp](CLI/README.md) 是 TDA 的命令行接口，面向 AI / 工程师**零代码**使用 DataPrism：`dp export lsum test.stdf` 一行即可输出良率。文档位于 [CLI/](CLI/)，含 [逐命令参考](CLI/commands.md) 与 [AI 集成指南](CLI/ai-guide.md)（输出规模分层 / 退出码 / 最佳实践）。

> dp 目前为内部版本；公开安装包（MSI）规划中，将发布在本仓库 Release 页面，发布后本节会更新安装说明。

## Examples 快速开始

要求：Windows + .NET 10 SDK。

```bash
# 还原并构建全部示例
dotnet build tda-examples.slnx

# 运行某个示例（以 Ex101.UseDataPrism 为例）
dotnet run --project Examples/Ex101.UseDataPrism
```

示例项目通过 NuGet 引用上述包（不依赖 TDA 源码），**项目名即学习路径**。

## 示例项目清单

编号规则：`Ex` + 分类（百位） + 序号（十/个位）；分类见 [Examples/README.md](Examples/README.md) 中的命名约定说明。

| 项目 | 主题 | 关联 API / 概念 |
| --- | --- | --- |
| [Ex101.UseDataPrism](Examples/Ex101.UseDataPrism) | DataPrism 加载单个 STDF 文件，读取 Lot 基本信息（LotID / SublotID / 站点等） | `DataPrism` |
| [Ex102.ExportParamsStatistics](Examples/Ex102.ExportParamsStatistics) | 批量选取多个 STDF 文件，按 TestIDSet 过滤测试项，导出统计指标 CSV | `DataPrism`、TestIDSet、CSV |
| [Ex103.ExportRawData](Examples/Ex103.ExportRawData) | DataPrism 解析并导出 Raw Data 到 CSV | `DataPrism.ExportRawDatas()` |
| [Ex105.ExportSummary](Examples/Ex105.ExportSummary) | 单文件 Yield / HardBin / SoftBin（按站点）汇总为 CSV 摘要 | `DataPrism`、统计汇总 |
| [Ex106.ExportPartSummary](Examples/Ex106.ExportPartSummary) | 导出 Part / Device 信息列表到 CSV | PartDescription、CSV |
| [Ex201.UseMsgInConsole](Examples/Ex201.UseMsgInConsole) | 控制台程序中 `Msg` 分级日志（Debug / Info / Warning / Error）输出到文件 | `Msg` |
| [Ex202.UseMsgInWinform](Examples/Ex202.UseMsgInWinform) | WinForms 程序中触发 `Msg` 分级日志 | `Msg` |
| [Ex203.UseMsgInConsoleAndWinform](Examples/Ex203.UseMsgInConsoleAndWinform) | `Msg` 同时挂载控制台 + WinForms 双输出目标 | `Msg` |
| [Ex204.CfgFileForDataPrismSetup](Examples/Ex204.CfgFileForDataPrismSetup) | 构建含多 Sublot / 多轮复测的 DataPrismSetup 并序列化为 JSON 配置 | `DataPrismSetup`、JSON |
| [Ex205.CfgFileForTestIDSet](Examples/Ex205.CfgFileForTestIDSet) | TestIDSet 配置读写：从 STDF 提取导出 CSV、再读回 | `TestIDSet`、CSV |
| [Ex206.CfgFileForTestIDProfileSet](Examples/Ex206.CfgFileForTestIDProfileSet) | TestIDProfileSet 四种场景：从 STDF 自动生成 / 手动创建 / CSV 加载 / 由 TestIDSet 转换 | `TestIDProfileSet`、CSV |
| [Ex301.RecordReaderBasics](Examples/Ex301.RecordReaderBasics) | RecordReader 基础工作流（校验 → 读 FAR → 迭代 ReadNext → 自动释放）与压缩文件解压模式对比 | `RecordReader`、`StdfDecompressionMode` |
| [Ex401.UseStdfReader](Examples/Ex401.UseStdfReader) | RecordReader 逐条解析全部记录类型并对每条执行逐字节完整性校验 | `RecordReader`、`ToBytesCheck()` |
| [Ex501.UseRecordWriter](Examples/Ex501.UseRecordWriter) | RecordReader → RecordWriter 整文件逐条拷贝，输出写入统计 | `RecordReader`、`RecordWriter` |
| [Ex601.GUICharts](Examples/Ex601.GUICharts) | TDA.GUI 五种图表控件（趋势 / 直方图 / 累积概率 / 箱线图 / 晶圆图）Tab 页展示 | `TDA.GUI` 控件 |
| [Ex602.GUIEasyPlotForm](Examples/Ex602.GUIEasyPlotForm) | EasyPlotForm 快速画图：单例 + 3 行代码出图 | `EasyPlotForm` |
| [Ex603.FlexiblePlotForm](Examples/Ex603.FlexiblePlotForm) | FlexiblePlotForm 按需多图布局（1 图全屏 / 2 图左右 / 3~4 图 2×2） | `FlexiblePlotForm` |
| [Ex701.LotYieldReport](Examples/Ex701.LotYieldReport) | 从 STDF / NEDA CSV 经 LotYieldReport 模板渲染良率 HTML 报告 | `TDA.Report`、`LotYieldReport` |

> 各示例的详细说明见 [Examples/README.md](Examples/README.md)。其中标注「源码待补充」的行属于 TDA 内部规划中的示例，尚未随同步进入本仓库。

## 版本对齐

本仓库全部示例当前引用 **TDA / TDA.GUI / TDA.Report 2.1.2**（见各 `.csproj`）。`CLI/` 文档对应 dp 2.x（TDA 2.1.x），升级 TDA 时注意同步核对。升级 TDA 版本时：

1. 确认目标版本已发布到 nuget.org；
2. 同步替换全部 `.csproj` 中的 `Version`（或重跑 `tools/convert-to-nuget-refs.ps1`）；
3. 同步更新本文档中标注的版本号。

## 仓库纪律

- `Examples/` 是 TDA 私有仓库的单向快照镜像：目录结构与文件与 `TDA/Examples` 保持 1:1，**不在本仓库内重命名、重组或直接修改示例代码**（改动应先在 TDA 侧完成，再随同步进入）；
- 示例一律通过 NuGet `PackageReference` 引用，**禁止 `ProjectReference`**（避免引入 TDA 源码）；
- 真实 `.stdf` / `.std` 测试数据可能含产品信息，**禁止入库**（示例应使用自生成演示数据或命令行参数输入）；
- `CLI/` 为公开仓库直接维护（非镜像）：内容必须与 dp 实际行为一致，不放二进制 / 安装包 / 真实数据，维护约定见 [CLI/README.md](CLI/README.md)。
