# TDA NuGet 包文档

本目录是 `TDA` / `TDA.GUI` / `TDA.Report` 三个 NuGet 包 **README 的公开镜像** —— 内容与各包在 [nuget.org](https://www.nuget.org/packages/TDA) 包主页上显示的 README 完全一致。

| 包 | English | 简体中文 | NuGet |
| --- | --- | --- | --- |
| **TDA**（核心库：STDF 解析 / 分析 / 导入导出） | [README.md](TDA/README.md) | [README.zh-CN.md](TDA/README.zh-CN.md) | [nuget.org/packages/TDA](https://www.nuget.org/packages/TDA) |
| **TDA.GUI**（WinForms 图表控件） | [README.md](TDA.GUI/README.md) | [README.zh-CN.md](TDA.GUI/README.zh-CN.md) | [nuget.org/packages/TDA.GUI](https://www.nuget.org/packages/TDA.GUI) |
| **TDA.Report**（HTML / Excel 报告生成） | [README.md](TDA.Report/README.md) | [README.zh-CN.md](TDA.Report/README.zh-CN.md) | [nuget.org/packages/TDA.Report](https://www.nuget.org/packages/TDA.Report) |

> 命令行工具 `dp` 不是 NuGet 包，它的用户文档在 [`CLI/`](../CLI/)（公开仓直接维护）。

## 维护约定

- **唯一事实源在 TDA 私有仓**：`Src/<项目>/README.md` 与 `README.zh-CN.md`。本目录的内容一律由那边单向镜像过来，**不要在这里直接编辑**。
- **同源双用**：同一份 `README.md` 两处在用 —— 打进 NuGet 包（`PackageReadmeFile`，见 `Directory.Build.props`）与本目录（供中英切换深链访问）。改它就是在改 nuget.org 的包主页。
- **同步脚本**：`tools/sync-package-readme.ps1`（只复制 + 写日志，不 commit / push）。改完 TDA 侧的 README 后跑一次，差异随下一次同步提交入库。
- **Pull 前先跑脚本**，避免本目录落后于包主页造成两处说法不一致。
