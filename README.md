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
├── Examples/   # 可运行示例代码（与 TDA 内部仓库 1:1 镜像）
├── Docs/       # 教程文档（规划中）
└── Skills/     # AI Skills：面向常见 TDA 使用场景（规划中）
```

## Examples 快速开始

要求：Windows + .NET 10 SDK。

```bash
# 还原并构建全部示例
dotnet build tda-examples.slnx

# 运行某个示例（以 Ex301.RecordReaderBasics 为例）
dotnet run --project Examples/Ex301.RecordReaderBasics
```

示例项目通过 NuGet 引用上述包（不依赖 TDA 源码），**项目名即学习路径**：

| 分类 | 主题 | 示例 |
| --- | --- | --- |
| Ex1xx | 数据分析与导出（DataPrism / CSV） | Ex101 ~ Ex106 |
| Ex2xx | `Msg` 日志工具与配置文件（JSON/CSV） | Ex201 ~ Ex205 |
| Ex4xx | STDF 逐记录解析与字节校验 | Ex401 |
| Ex5xx | `RecordWriter` 写入 | Ex501 |
| Ex6xx | TDA.GUI 图表控件 | Ex601 ~ Ex603 |
| Ex7xx | 良率 / 报表 | Ex701 |

> 注：Ex206（TestIDProfileSet 配置）及 RecordReader 系列（Ex3xx）示例依赖尚未发布到 NuGet 的最新 API（如 `TDA.Utilities.Str`），待 TDA 发布对应新版后随同步补入本仓库。

每个示例的详细说明见 [Examples/README.md](Examples/README.md)。

## 版本对齐

Examples 同步自 TDA 私有仓库，并引用当时已发布的 NuGet 包版本；若示例引用的版本早于最新版，升级请同步替换各 `.csproj` 中的 `Version`（或用 `tools/convert-to-nuget-refs.ps1 -Version <新版>` 重跑转换）。
