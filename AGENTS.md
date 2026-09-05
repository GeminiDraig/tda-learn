# AGENTS.md — tda-examples 仓库导航（给 AI 助手的导览）

本文件帮助 AI 助手快速理解仓库结构与约定：**想教用户什么，代码在哪，以及哪些事不能做。**

## 仓库是什么

TDA Framework（半导体 STDF 数据分析 .NET 框架，nuget 包 `TDA` / `TDA.GUI` / `TDA.Report`）的**官方示例与学习仓库**。核心库不开源，本仓库通过示例让"人类 + AI"学会使用 TDA。

当前状态：`Examples/` 已镜像 18 个示例并通过 NuGet 编译；`Docs/`、`Skills/` 为规划板块，尚未落地。

## 目录布局

```
tda-examples/
├── Examples/                    # 18 个可运行示例（1:1 镜像自 TDA 私有仓库 Examples/）
│   ├── README.md                # 各示例详细说明（镜像内容，内部仓库维护）
│   ├── Ex1xx/                   # 数据分析与导出（DataPrism / CSV）
│   ├── Ex2xx/                   # Msg 日志 + 配置文件（TestIDSet / TestIDProfileSet / DataPrismSetup）
│   ├── Ex3xx/                   # RecordReader 基础
│   ├── Ex4xx/ Ex5xx/            # RecordReader 逐条解析 / RecordWriter 写入
│   ├── Ex6xx/                   # TDA.GUI 图表（GUICharts / EasyPlotForm / FlexiblePlotForm）
│   └── Ex7xx/                   # TDA.Report 良率报告
├── Docs/                        # （规划中）教程
├── Skills/                      # （规划中）AI Skills
├── tools/convert-to-nuget-refs.ps1   # ProjectReference → PackageReference 转换脚本
├── Directory.Build.props        # 统一 TargetFramework：net10.0-windows10.0.19041
└── tda-examples.slnx            # 全部示例解决方案（.NET 10 slnx 格式）
```

## Examples 学习地图（按目标定位示例）

用户问"怎么做 X"时，先在此表定位，再去对应目录读代码：

| 用户目标 | 起点示例 |
| --- | --- |
| 解析 STDF、统计、导出（数据分析入口） | `Ex101.UseDataPrism` → `Ex102` / `Ex103` / `Ex105` / `Ex106` |
| 用 `Msg` 写分级日志（Console / WinForms / 混合） | `Ex201` / `Ex202` / `Ex203` |
| 配置文件：DataPrismSetup（JSON） | `Ex204.CfgFileForDataPrismSetup` |
| 配置文件：TestIDSet / TestIDProfileSet（CSV） | `Ex205` / `Ex206`（自动生成、手动创建、加载、从 TestIDSet 转换） |
| 底层逐条读记录（RecordReader） | `Ex301.RecordReaderBasics`（入门）、`Ex401.UseStdfReader`（全类型 + 字节校验） |
| 写 STDF / 拷贝文件（RecordWriter） | `Ex501.UseRecordWriter` |
| WinForms 图表控件 | `Ex601.GUICharts`（五种控件）、`Ex602`（EasyPlotForm 速画）、`Ex603`（FlexiblePlotForm 多图布局） |
| 良率 HTML 报告 | `Ex701.LotYieldReport`（含 `--demo` 自生成数据模式） |
| STDF 记录 / 字节结构 / Codec | `Src` 不开源；参考 `Ex401` 的 `ToBytesCheck()` 用法 |

查找 API 用法：在 `Examples/` 下按命名空间 `TDA.*` 反查 `using`，即可找到某 API 的所有示例引用。

## 常用命令

```bash
dotnet build tda-examples.slnx            # 构建全部（还原走 nuget.org）
dotnet run --project Examples/Ex101.UseDataPrism   # 运行单个示例
dotnet run --project Examples/Ex701.LotYieldReport -- --demo   # 无需真实数据的演示模式
```

## 硬性约定（务必遵守）

1. **`Examples/` 是镜像**：目录名与文件与 TDA 私有仓库 `Examples/` 1:1，禁止在仓库内重命名 / 重组 / 直接修改示例源码。示例代码改进须指向 TDA 侧（用户操作），同步后再镜像进来。
2. **禁止 `ProjectReference`**：示例 csproj 只能 `PackageReference` 引用 `TDA` / `TDA.GUI` / `TDA.Report`（当前 2.1.2）。若源码里有 `Src\...` 路径引用说明镜像异常。
3. **禁止入库**：`Src/`、`*.csproj`（来自 TDA 核心的）、`*.nupkg`、真实 `.stdf` / `.std` 数据文件、任何 TDA 私有信息。
4. **版本一致性**：升级 TDA 包版本时须同步改全部 csproj + README，且版本必须已发布到 nuget.org（否则用户 clone 后无法还原）。
5. **Docs/Skills 代码引用约定**（落地后生效）：引用 `Examples/` 相对路径而非复制代码块，避免教程代码随版本过期。
6. **git 提交**：未经维护者明确指示，不要执行 `git commit` / `git push`（含本仓库与任何相关仓库）。

## 给维护者的同步提示

- 同步方向唯一：TDA（私有）→ tda-examples（公开）。每次同步对应一次 TDA NuGet 发布。
- 同步后若发现 csproj 变为 ProjectReference 形式，重跑 `tools/convert-to-nuget-refs.ps1` 转换。
- 本仓库 git 历史完全独立于 TDA，二者零血缘。
