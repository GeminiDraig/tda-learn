# dp — TDA 命令行工具（TDA.CLI）

> **文档版本：** v1.0 · 2026-09 · 适用 dp 2.x（TDA Framework 2.1.x）
> **维护位置：** 本目录（tda-examples `CLI/`）直接维护，随 dp 版本更新。若文中用法与 `dp --help` 不一致，**以实际命令行为准**，并请顺手修正本文档。

`dp` 是 TDA Framework 的命令行接口：把 DataPrism 的核心能力（STDF 解析、多 Sublot / 复测关联、统计分析、CSV 导出、Setup 配置生成）封装为**一行命令**，无需编写任何 C# 代码即可完成常见的数据分析任务。

设计初衷是**给 AI Agent 与脚本使用**：AI 只需拼出正确的命令行字符串、读回输出的 CSV / JSON，即可完成"分析这份 STDF"这类请求。

## 与 Examples 的分工

| 通道 | 适合谁 | 入口 |
| --- | --- | --- |
| `Examples/` | 想**写 C# 代码**调用 TDA 的学习者 / 集成到自有程序 | 本仓库 [Examples/README.md](../Examples/README.md) |
| `CLI/`（本目录） | 不想写代码，想**快速分析 STDF 文件**的工程师 / AI Agent / 脚本 | 本目录 `README.md` |

两者能力重叠（同一 DataPrism 内核）。想深入理解命令背后的行为时，可对照阅读 `Examples/` 中对应的示例项目。

## 特性

- **零代码**：`dp export lsum test.stdf` 一行输出良率摘要；
- **输出标准化**：数据结果默认写为 CSV 文件，适合 AI 与脚本解析；
- **通配符批量**：支持 `*`、`**`（递归）、`?`，一次处理整个目录的批次文件；
- **复测 / 多 Sublot 关联**：`dp create setup` 自动推断初测-复测关系生成配置文件，后续命令可直接复用；
- **后台缓存加速**：数据命令默认经由常驻 Daemon 进程执行，同一 STDF 只加载一次，重复分析命中缓存（详见 [ai-guide.md](ai-guide.md)）；
- **Windows / .NET**：基于 .NET 10 构建，仅支持 Windows 10/11。

## 获取 dp

dp 目前处于内部开发阶段，**尚未公开发布**；规划中的发布形态为 **MSI 安装包，将托管在本仓库的 Release 页面**（即将提供）。

- 已通过内部渠道获得绿色包（免安装目录）的用户：目标机需 Windows 10/11 + **.NET 10 Desktop Runtime（x64）**，把目录加入 `PATH` 后 `dp` 即可用；
- MSI 正式发布后，本节将更新为标准安装步骤（Release → 下载 → 安装 → 验证）。

> 需要提前体验完整命令？可联系维护者获取当前内部版本。

## 快速开始（30 秒）

以下假设你已有 STDF 文件（路径含空格时务必加引号）：

```powershell
# 1. 验证安装：显示 dp 与 TDA 框架版本
dp show version

# 2. 查看 Lot 良率摘要（自动写出 <目录>\<文件名>.lsum.csv）
dp export lsum "D:\data\demo_lot.stdf"

# 3. 导出全部测试项的统计（均值/极差/Cp/Cpk/分位数）
dp export tsum "D:\data\demo_lot.stdf" -o "D:\result\tsum.csv"

# 4. 批量：一个目录下多个批次文件（支持 ** 递归）全部出摘要
dp export lsum "D:\data\**\*.std"
```

> 第一条数据命令会自动启动后台 Daemon 并加载文件，**首次执行明显偏慢属正常**；同一文件再次执行会命中缓存。
>
> **如果某次命令异常地慢**（或终端出现 `falling back to standalone`）：多半是后台 Daemon 进程被意外结束了，缓存加速暂时失效。先 `dp daemon status` 查看，若输出 `Daemon is not running.`，执行 `dp daemon restart` 即可把它重新拉起（详见 [commands.md](commands.md)「dp daemon」一节）。

## 命令速查

完整逐命令参考见 [commands.md](commands.md)。快速定位表：

| 想做什么 | 命令 |
| --- | --- |
| 良率 / Pass / Fail / 按 Site 汇总 | `dp export lsum` |
| 测试项统计（Cp/Cpk/分位数等，30 列） | `dp export tsum` |
| 逐颗 Part 明细（Bin/坐标/Pass/轮次） | `dp export psum` |
| HardBin / SoftBin 分布 | `dp export hbin` / `dp export sbin` |
| lsum+hbin+sbin 合并报表 | `dp export sum` |
| 测试项清单（可复用为过滤文件） | `dp export tidset` |
| 原始测试值逐条导出（大文件，注意体积） | `dp export raw` |
| Lot 基本信息（LotID/产品/程序/站点等） | `dp export info` |
| 由多个 STDF 推断复测关系，生成 Setup 配置 | `dp create setup` |
| 版本信息 | `dp show version` |
| Daemon 生命周期 / 缓存管理 | `dp daemon status / stop / restart / flush` |

输入约定：裸路径参数支持 `.stdf` / `.std` / `.stdf.gz` / `.std.gz`，或 `create setup` 生成的 `.json` 配置文件。不支持 `.summ` 等其他格式。

## 典型场景 → 命令

（详细决策表与输出规模建议见 [ai-guide.md](ai-guide.md)）

| 用户诉求 | 推荐命令 | 备注 |
| --- | --- | --- |
| "这份 STDF 良率多少？" | `dp export lsum` | 输出极小（< 1 KB 级） |
| "哪些测试项不合格率高？" | `dp export tsum` 后按 `FailedRate` 排序 | ~100 KB 级 |
| "每颗 Die 的 Bin 分布如何？" | `dp export psum` / `hbin` | |
| "这个 Lot 有复测，把 Final 数据导出来" | `dp export raw`（默认仅 Final 轮次） | **重量输出，务必指定 `-o`** |
| "按测试项清单只导某些参数" | 先 `dp export tidset` 得到清单 CSV，再 `dp export raw -t <清单>` | |
| "批量跑 100 个文件" | 路径用 `"D:\data\**\*.stdf"` | glob 由 dp 自身展开 |

## 行为速览

- **数据命令 = 写文件**：结果不打印到终端（终端只显示状态与警告），输出文件默认规则为「第一个输入文件的同目录 + `<文件名>.<子命令>.csv`」，可用 `-o/--output` 显式指定；
- **Daemon 自动管理**：数据命令默认先尝试走 Daemon（常驻进程，缓存已加载的 DataPrism），不可用时自动降级为 Standalone 直跑，结果一致；Daemon 进程可能被外部结束（被杀 / 异常退出），此时数据命令会暂时失去缓存加速——用 `dp daemon status` 查看、`dp daemon restart` 一键拉起即可恢复；
- **退出码**：`0` 成功，非 0 为失败（`1` 参数错误 / `2` 文件不存在 / `3` 无效 STDF …，完整表见 [ai-guide.md](ai-guide.md)）；
- **文件类型互斥**：同一命令中不能混用 STDF 与 `.json`、不能混用压缩与非压缩文件、不能混入其他类型（如 `.summ`）。

## 文档导航

| 文件 | 内容 |
| --- | --- |
| `README.md`（本文件） | 定位、获取、快速开始、场景入口 |
| [commands.md](commands.md) | 全部命令的逐条参考：语法 / 选项 / 输入输出 / 输出列与样例 |
| [ai-guide.md](ai-guide.md) | 面向 AI / 脚本的集成指南：命令选择决策、输出规模分层、退出码、失败模式 |

## 维护者约定（重要）

1. **本文档直接在本仓库维护**，不属于 `Examples/` 镜像范围，不会被同步脚本覆盖；
2. **内容必须与 dp 实际行为一致**：新增 / 变更命令时，同步更新 `commands.md` 速查表与对应小节、`README.md` 速查、`ai-guide.md` 决策表；
3. 命令参考只描述**已实现**的行为（`dp --help` 为准），未实现功能（如 `plot tmap/wmap`）需显式标注；
4. 文档中的示例数据一律使用**占位值**（如 `demo_lot.stdf`、`DEMO-001`），禁止出现真实产品 Lot / 文件信息；
5. 引用 `Examples/` 中示例时使用相对链接，不要复制代码块正文；
6. 本目录不放任何二进制 / 安装包 / 真实数据文件（仓库纪律，见顶层 [AGENTS.md](../AGENTS.md)）。
