# dp AI / 脚本集成指南

> 面向 AI Agent 与自动化脚本的 dp 使用手册：如何选择命令、结果多大、怎么判断成败、常见坑。
> 配套：入口见 [README.md](README.md)，逐命令细节见 [commands.md](commands.md)。

## 1. 什么时候用 dp，什么时候写代码？

| 用户诉求 | 建议 |
| --- | --- |
| 一次性分析 / 批量跑数据 / 快速出报告 | **用 dp**（零代码，一条命令） |
| 写进自有程序 / 需要代码级定制 | 用 TDA 库写 C#（对照 `Examples/`） |
| 想验证"某 API 能做到什么" | 先用 dp 出结果，再读对应 Example 源码了解机制 |

dp 与 TDA 库共享同一内核：dp 跑出来的 CSV 与用库 API 导出的数据一致。

## 2. 命令选择决策表（自然语言 → 命令）

| 用户说（或隐含意图） | 命令序列 |
| --- | --- |
| "分析这份 STDF / 看这个批次" | 先 `dp export info`（这是什么文件）→ 再按需 |
| "良率多少 / Pass Fail" | `dp export lsum` |
| "哪个参数不合格 / Cp Cpk" | `dp export tsum` |
| "每颗 Die 情况 / Bin 分布" | `dp export psum`、`dp export hbin`、`dp export sbin` |
| "导原始数据 / 让我看数值明细" | `dp export raw`（**注意量级**，先 `tidset` 缩小范围） |
| "复测 / 多轮数据怎么关联" | `dp create setup`（多文件一次性推断）→ 后续命令吃 setup |
| "把结果汇总一下" | `dp export sum` |
| "所有参数都导出来做分析" | `dp export tidset`（获得全量测试项清单） |

> 建议先跑一次 `dp export info` 确认拿到的是目标批次/产品，避免在错误的文件上跑完整分析。

## 3. 输出规模分层（先想清楚结果多大，再决定怎么消费）

dp **没有 `--stdout`**：数据结果一律**落盘**，终端只回状态/警告。因此"消费结果"= 知道输出文件在哪、有多大，然后读取。

量级参考（约 512 MB 的 STDF 实测，仅作规划概念）：

| 层级 | 输出量级 | 命令 | 建议 |
| --- | --- | --- | --- |
| 极轻量 | < 1 KB | `info` `lsum` `hbin` `sbin` `sum` `create setup` `show version` | 直接读整个 CSV，一次全拿 |
| 轻量 | 1 ~ 200 KB | `tidset`(~15 KB) `tsum`(~100 KB) | 可直接读，仍在常规上下文内 |
| 中量 | 0.2 ~ 5 MB | `psum`(~2 MB) | 建议读前 N 行 + 用脚本聚合（如按 Bin 计数），勿整读 |
| 重量 | > 5 MB | `raw`（可达 100 MB+） | **绝不整读**：只读结构（行数/列头/抽样前几行）或按需过滤后重新导出 |

对应实践：

- 多数对话类分析（"良率多少""哪些参数差"）用 `lsum` / `tsum` / `psum`+聚合，**不要一上来就 raw**；
- 只有用户明确要"原始数值 / 明细"时才 `export raw`，且应加 `-t <testids.csv>` 缩小范围、`-o` 指定落盘；
- `raw` 后想了解产出：读 CSV 首行列头与抽样行即可，判断行数规模（测试项数 × Part 数）。

## 4. 输出文件位置：永远显式指定 `-o`

默认规则（见 commands.md）把结果写到**输入文件同目录**，AI 未必知道该目录，也会污染数据目录。**建议每条命令都带 `-o`**：

```powershell
dp export lsum "D:\data\demo_lot.stdf" -o "D:\result\lsum.csv"
```

> 若在临时目录跑且不带 `-o`，默认输出会生成在源文件旁边（`<源名>.lsum.csv`），查找结果时注意。

## 5. 退出码（判断成败的权威信号）

| 退出码 | 含义 | 常见触发 |
| --- | --- | --- |
| 0 | 成功 | — |
| 1 | 参数 / 用法错误 | 未知选项、缺参数、混用互斥输入（如 STDF+json、压缩+非压缩） |
| 2 | 文件不存在 | 路径写错、glob 无匹配 |
| 3 | 无效 STDF | 文件损坏 / 不是 STDF / 无法解析 |
| 4 | 无效 Setup | setup.json 内容不合法 |
| 5 | 数据加载失败 | 加载/分析阶段出错 |
| 6 | Daemon 内存超限 | 缓存超上限（Standalone 模式下一般不会出现） |
| 7 | Daemon 不可用 | 自动降级失败时的显式失败路径 |
| 8 | Pipe 超时 | Daemon 通信超时（大文件首载偶发） |
| 9 | 队列已满 | Daemon 并发过载 |

PowerShell 中检查：命令后执行 `echo $LASTEXITCODE`（或 `if ($LASTEXITCODE -ne 0) { ... }`）。批处理：`echo %ERRORLEVEL%`。

> 注意：Daemon 模式返回的是执行命令的退出码（0-5 区间为主）；通信类失败（7/8）多在 Standalone 降级路径上被消化——多数情况下用户看到的是"降级后照常出结果"。

## 6. 高频失败模式与应对

| 现象 | 原因 | 应对 |
| --- | --- | --- |
| 退出码 2"文件不存在" | 路径错误或 glob 无匹配 | 核对路径；确认通配符语法；文件确在再跑 |
| "Cannot mix STDF and .json" | 一次命令传了两种输入 | 分开跑 |
| "Cannot mix compressed and uncompressed" | `.stdf` 与 `.stdf.gz` 混传 | 分开跑 |
| 报不支持 `.summ` / `.summary` | 传了非 STDF 输入 | 只传 STDF/Setup；这些是副产品格式 |
| 首条命令很慢 | Daemon 启动 + 首次加载 | 正常；同文件第二条命令应命中缓存 |
| 某次命令异常地慢 / `daemon status` 显示 `Daemon is not running.` | Daemon 进程被意外结束（外部 kill / 异常退出），缓存加速失效 | `dp daemon restart` 重新拉起，恢复常驻缓存 |
| 终端出现 falling back to standalone | Daemon 未就绪自动降级 | 结果仍正确；重跑一次即可享受缓存 |
| 结果文件没找到 | 忘带 `-o`，写到源文件旁 | 按默认命名规则查找（`<源名>.<子命令>.csv`） |
| `dp` 不是内部或外部命令 | 未安装 / PATH 未配置 | 见 README 获取章节 |

## 7. 推荐的 AI 工作流模板

对"分析一批 STDF"类任务，推荐固定动作序列：

```powershell
# Step 1 确认文件
dp export info "D:\data\**\*.stdf" -o "out\info.csv"

# Step 2 总览（良率 / 摘要）
dp export sum "D:\data\**\*.stdf" -o "out\summary.csv"

# Step 3 深挖参数（若需要）
dp export tsum "D:\data\**\*.stdf" -o "out\tsum.csv"

# Step 4 拿测试项清单用于后续过滤
dp export tidset "D:\data\demo_lot.stdf" -o "out\testids.csv"

# Step 5 需要明细时，用清单过滤、缩小范围
dp export raw "D:\data\demo_lot.stdf" -t "out\testids.csv" --all -o "out\raw.csv"
```

要点：

- 每个 `-o` 都指向**自己可控的输出目录**，文件名含命令名避免互相覆盖；
- 读完大文件务必记录：文件路径、行数（`Get-Content ... | Measure-Object -Line` 或逐块读）、列头，避免把 100 MB 塞进上下文；
- 路径一律加引号（空格、`*` 通配符都建议引号包裹——glob 是 dp 自己展开的，不是 shell）。

## 8. Daemon / 缓存相关注意事项

- 数据命令自动走 Daemon，**重复分析同一批文件应复用缓存**（可 `dp daemon status` 查看命中率）；
- 数据源文件更新后想强制重载：`dp daemon flush`（清缓存不杀进程）；
- 排查"是否 Daemon 导致的问题"：命令前设置 `TDA_DAEMON_DISABLED=1` 走 Standalone 对照（结果应一致）；
- 升级 / 替换 dp 前先 `dp daemon stop`（进程锁文件时覆盖会失败）；
- Daemon 空闲约 30 分钟自动退出，属正常，不需要手动 keep-alive；
- Daemon 也可能被外部结束（任务管理器 / `taskkill` / 杀软等）：表现为命令变慢且 `dp daemon status` 显示 `Daemon is not running.`。执行 `dp daemon restart` 即可拉起（该命令对未运行进程同样有效）。

## 9. 深入时：对照 Examples

dp 与示例库共用内核。想理解命令背后的数据语义 / 验证边界行为时，按表对照读源码示例：

| dp 命令 | 对应示例（相对链接） |
| --- | --- |
| `export info` / 基础加载 | [Ex101.UseDataPrism](../Examples/Ex101.UseDataPrism) |
| `export tsum` / 统计 | [Ex102.ExportParamsStatistics](../Examples/Ex102.ExportParamsStatistics) |
| `export raw` | [Ex103.ExportRawData](../Examples/Ex103.ExportRawData) |
| `export lsum / hbin / sbin` | [Ex105.ExportSummary](../Examples/Ex105.ExportSummary) |
| `export psum` | [Ex106.ExportPartSummary](../Examples/Ex106.ExportPartSummary) |
| `create setup` 关系推断 | [Ex204.CfgFileForDataPrismSetup](../Examples/Ex204.CfgFileForDataPrismSetup) |
| TestID / Profile 过滤语义 | [Ex205.CfgFileForTestIDSet](../Examples/Ex205.CfgFileForTestIDSet)、[Ex206](../Examples/Ex206.CfgFileForTestIDProfileSet) |

> 文档约束：本仓库不包含真实 STDF 数据；示例数据用 `Ex701.LotYieldReport --demo` 这类自生成模式或用户自有文件。
