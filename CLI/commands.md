# dp 命令参考（逐命令）

> 适用 dp 2.x（TDA 2.1.x）。**只收录已实现命令**；带「未实现」标注的命令可解析但会返回失败。
> 所有示例路径与输出均为示意（`demo_lot.stdf` 等占位值），命令行为与真实一致。

## 命令速查总表

| 命令 | 输入 | 主要选项 | 默认输出文件 | 输出量级参考* | 状态 |
| --- | --- | --- | --- | --- | --- |
| `dp export raw` | .stdf/.std/.gz / .json | `-t/--testid`、`--final/--all`、`--format neda\|rpv` | `<名>.raw.csv` | 数十 MB ~ 100 MB+ | ✅ |
| `dp export tsum` | 同上 | `-t/--testid` | `<名>.tsum.csv` | ~100 KB | ✅ |
| `dp export psum`（别名 `dsum`） | 同上 | — | `<名>.psum.csv` | ~2 MB | ✅ |
| `dp export lsum`（别名 `lot-summary`） | 同上 | — | `<名>.lsum.csv` | < 1 KB | ✅ |
| `dp export hbin`（别名 `hard-bin`） | 同上 | — | `<名>.hbin.csv` | < 1 KB | ✅ |
| `dp export sbin`（别名 `soft-bin`） | 同上 | — | `<名>.sbin.csv` | < 1 KB | ✅ |
| `dp export sum`（别名 `all-summary`） | 同上 | — | `<名>.sum.csv` | ~1 KB | ✅ |
| `dp export tidset` | 同上 | — | `<名>.tidset.csv` | ~15 KB | ✅ |
| `dp export info` | 同上 | — | `<名>.info.csv` | < 1 KB | ✅ |
| `dp create setup` | .stdf 文件 | `-o/--output` | `<名>.setup.json` | < 1 KB | ✅ |
| `dp plot tmap`（别名 `test-map`） | .stdf/.std/.gz / .json | — | `<名>.tmap.png` | — | ⚠️ 未实现 |
| `dp plot wmap`（别名 `wafer-map`） | 同上 | `--xy <p1> <p2>` | `<名>.wmap.png` | — | ⚠️ 未实现 |
| `dp show version` | 无 | — | 终端输出 | — | ✅ |
| `dp daemon status/stop/restart/flush` | 无 | — | 终端输出 | — | ✅ |

\* 量级参考基于约 512 MB 的 STDF 实测样本，仅为命令选择与资源规划提供量级概念（详见 [ai-guide.md](ai-guide.md) 输出分层）。

## 通用约定

### 输入：files 裸参数

所有数据命令接受一个或多个**裸路径参数**（无 `--` 前缀），支持：

- STDF 文件：`.stdf`、`.std`、`.stdf.gz`、`.std.gz`（压缩解压自动处理）；
- Setup 配置文件：`dp create setup` 生成的 `.json`；
- 通配符：`*`（同层）、`**`（递归子目录）、`?`（单字符）。glob 由 dp 自身展开，**路径含 `*` 时外层仍建议加引号**（交给 dp 而非 shell）；
- 多个文件可一次给出（同一 Sublot 多文件、或经 setup 合并的批次）。

**互斥规则**（违反会报错并退出码非 0）：

- 不能混用 STDF 与 `.json`；
- 不能混用压缩与非压缩 STDF（如 `.stdf` 与 `.stdf.gz`）；
- 不能混入不支持的类型（如 `.summ`、`.summary`）——需使用 STDF/Setup 输入时混入其他后缀即报错；
- glob 无匹配文件时报"文件不存在"类错误。

### 通用选项（export 组与 plot 组通用）

| 选项 | 别名 | 说明 |
| --- | --- | --- |
| `--output <path>` | `-o` | 输出文件路径；缺省时自动生成（见下） |
| `--filetype <type>` | `-f` | 输出文件扩展名/格式，默认 `csv` |
| `--verbose` | `-v` | 显示详细处理日志 |

### 输出文件默认命名

未指定 `-o` 时，输出写到**第一个输入文件的同目录**：

```
<第一个输入文件去掉 STDF 后缀的文件名>.<子命令>.<扩展名>
例：D:\data\demo_lot.stdf  →  D:\data\demo_lot.lsum.csv（lsum 命令）
    D:\data\demo_lot.std.gz →  D:\data\demo_lot.raw.csv
    输入为 setup.json 时，以 setup.json 所在目录与文件名为基准
```

`dp create setup` 默认扩展名为 `.json`。

### 结果文件与终端

数据命令的结果**写入文件**，终端只打印进度/状态/警告；判断成败看**退出码**（`dp --version` 后可用 `echo $LASTEXITCODE`）。CSV 为 UTF-8 文本，首行为列头。

---

## dp export raw — 导出原始测试数据（重量输出）

把每个 Part 的原始测试值导出为 CSV，支持按 TestID 过滤与 Final/All 轮次控制。

```powershell
dp export raw "D:\data\demo_lot.stdf"
dp export raw "D:\data\demo_lot.stdf" -t "D:\config\testids.csv" -o "D:\result\raw_filtered.csv"
dp export raw "D:\data\demo_lot.stdf" --all                # 含初测+复测所有轮次
dp export raw "D:\config\setup.json" --format rpv          # RowPerValue 长格式
```

| 选项 | 说明 |
| --- | --- |
| `-t, --testid <csv>` | TestID 集合文件（由 `dp export tidset` 生成），只导出清单中的测试项；缺省导出全部 |
| `--final` | **默认**。只导出 Final 数据（有复测的批次只保留复测轮次，剔除对应初测） |
| `--all` | 导出全部轮次（初测+复测所有数据） |
| `--format neda\|rpv` | `neda`（默认，每行一个 Part 的宽格式）/ `rpv`（RowPerValue，每行一个测试值的长格式） |

> **注意：** 这是唯一默认就可能是**数十 MB~100 MB+** 的命令。请始终用 `-o` 指定落盘位置，不要把大结果当普通文件处理；按需使用 `-t` 过滤以显著减小体积。

---

## dp export tsum — 测试项参数统计（Cp / Cpk / 分位数）

对全部测试项输出描述性统计，一行一个测试项。适合找"哪些参数异常 / 分布如何"。

```powershell
dp export tsum "D:\data\demo_lot.stdf"
dp export tsum "D:\config\setup.json" -o "D:\result\tsum.csv"
dp export tsum "D:\data\**\*.std" -t "D:\config\testids.csv"
```

**输出列（CSV 首行）：**

```
FileName, LotID, SublotID, WaferID, TestNumber, TestText, FQN, Lsl, Hsl, Unit,
TestedCnt, FailedCnt, FailedRate, Min, Mean, Max, Stdev,
P1, P5, P10, P25, P50, P75, P90, P95, P99,
Cp, Cpk, Cpl, Cpu
```

示意行（数值为演示值）：

```
demo_lot.stdf,DEMO-001,1,W1,1001,VDD_Isb_Stby,VDD_Isb_Stby,0.1,5.0,V,999,12,1.20,0.102,0.215,1.401,0.183,0.110,0.121,0.141,0.168,0.210,0.260,0.312,0.342,0.4,0.7,0.5,0.9
```

---

## dp export psum / dsum — Part（Die）明细

每颗被测 Part/Device 一行：Bin、Pass/Fail、坐标、轮次、所属数据源等。

```powershell
dp export psum "D:\data\demo_lot.stdf"
dp export dsum "D:\data\demo_lot.stdf"          # dsum 是 psum 的别名
dp export psum "D:\config\setup.json" -o "D:\result\parts.csv"
```

**输出列：**

```
PartIndex, DataSourceID, PartID, PartText, HardBin, SoftBin, PartFlag,
IsPass, IsFinalData, TestRoundNumber, SiteNumber, X, Y, TestTime, FirstFailTestNum
```

示意行：

```
1,0,P1,DP0896-0001,1,1,0,True,True,1,1,12.5,24.0,2026-09-01 08:00:01,0
```

---

## dp export lsum — Lot 摘要（良率速报）

```powershell
dp export lsum "D:\data\demo_lot.stdf"
dp export lsum "D:\config\setup.json" -o "D:\result\lsum.csv"
```

**输出内容：** Lot 总体统计 + 按 Site 分组统计。

| 字段 | 含义 |
| --- | --- |
| TotalQty / PassQty / FailQty / InvalidQty | 有效总数 / Pass / Fail / 无效数（均为 Final 数据口径） |
| YieldRate | 良率 = PassQty / TotalQty × 100 |

示意：

```
LotID,SublotID,TotalQty,PassQty,FailQty,InvalidQty,YieldRate
DEMO-001,1,67200,65132,2068,0,96.92
```

> 终端无摘要打印；全部内容在 CSV 中。想看"命令执行即拿到结论"，可配合脚本读取本 CSV 的 YieldRate 字段。

---

## dp export hbin / sbin — Bin 分布

```powershell
dp export hbin "D:\data\demo_lot.stdf"
dp export sbin "D:\config\setup.json" -o "D:\result\sbin.csv"
```

**输出列：** `BinNo, BinName, ResultType, Count, Percentage`

示意：

```
1,PASS,Pass,65000,96.73
2,OPEN,Fail,1200,1.79
```

---

## dp export sum — 综合摘要（lsum + hbin + sbin 合并）

```powershell
dp export sum "D:\data\demo_lot.stdf"
dp export sum "D:\data\demo_lot.stdf" -o "D:\result\summary.csv"
```

单个 CSV 文件、多段格式（段标题 + 空行分隔），便于一份文件带走全部摘要。

---

## dp export tidset — 测试项清单（导出后可复用为过滤器）

```powershell
dp export tidset "D:\data\demo_lot.stdf"
dp export tidset "D:\config\setup.json" -o "D:\config\testids.csv"
```

**输出列：** `TestNumber, TestText, TestSequence`

导出的 CSV 可直接作为 `dp export raw / tsum` 的 `-t/--testid` 输入（见对应命令）。

---

## dp export info — Lot / 文件基本信息

```powershell
dp export info "D:\data\demo_lot.stdf"
dp export info "D:\config\setup.json" -o "D:\result\info.csv"
```

**输出字段（来自文件头部的 MIR/SDR/WCR/WIR 记录）：**

| 字段 | 含义 |
| --- | --- |
| LotID / SublotID | 批次 / 子批次编号 |
| ProductID | 产品型号（PART_TYP） |
| ProgramName / NodeName / TestType | 测试程序 / 机台节点 / 测试温度 |
| IsCP / WaferID | 是否为 CP（晶圆）数据 / 晶圆号 |
| FileName / FileCount | 源文件路径 / 数据源数量 |
| TotalParts / TestItemCount | Part 总数 / 测试项总数 |

用于快速确认"我拿到的文件是什么批次、什么产品"，适合一切分析前的第一步。

---

## dp create setup — 生成 Setup 配置（复测 / 多 Sublot 关联）

给定同一批次的多个 STDF（初测 + 若干轮复测），自动推断归属关系并输出 `.json` 配置，供后续所有命令复用——此后分析整个批次只需传这一个文件。

```powershell
# 单文件批次
dp create setup "D:\data\demo_lot.stdf" -o "D:\config\setup.json"

# 同一 Sublot 的初测 + 两轮复测（推断同 Lot：按 LOT_ID/PART_TYP/PROG_NAM 匹配）
dp create setup "D:\data\lot_P.stdf" "D:\data\lot_R1.stdf" "D:\data\lot_R2.stdf" -o "D:\config\setup.json"

# 生成后即可用 setup 跑任何 export
dp export sum "D:\config\setup.json"
```

判定逻辑（概要）：多个文件通过 MIR 记录中的 Lot/产品/程序匹配是否同一 Lot；同一 Lot 内通过 PartID 与 XY 坐标比对推断初测与复测轮次关系。

---

## dp plot tmap / wmap — 可视化（⚠️ 未实现）

命令已注册（`tmap` 别名 `test-map`；`wmap` 别名 `wafer-map`，带 `--xy <p1> <p2>` 占位），但功能尚未实现，执行将返回失败。规划中的用途：由测试数据直接生成 Test Map / Wafer Map 图片。请勿依赖。

---

## dp show version — 版本信息

```powershell
dp show version
dp --version
```

输出 dp 自身与所引用 TDA 框架的版本（示意）：

```
dp (TDA.CLI) v2.1.0
TDA Framework v2.1.0
```

> 排查"文档与命令不一致"时先执行它，确认版本。

---

## dp daemon — 后台缓存进程管理

数据命令默认经由常驻 Daemon 执行（同一批文件只加载/解析一次，后续命令命中缓存，避免大文件重复加载）。Daemon **自动启动**：第一条数据命令发现未运行时自动拉起；**不可用时自动降级**为 Standalone 直跑，结果一致，只是没有缓存加速。

实际可用的子命令仅四个：

### dp daemon status — 查看运行状态与缓存

```powershell
dp daemon status
```

示意输出：

```
TDA.CLI Daemon
Status:      Running (PID 12345)

Cache:
  Capacity:   5
  Entries:    3
  Hit Rate:   87.5% (21/24)

  D:\data\demo_lot.stdf   2100 MB | hits: 8  | last: 2m ago
```

### dp daemon stop — 优雅停止（清缓存）

发送停止请求，Daemon 处理完当前请求后清理缓存并退出。升级 dp / 释放内存时使用。

### dp daemon restart — 重启 / 拉起 Daemon

等价于 stop + start：先停旧进程（缓存随进程清空），再启动新实例。**对未运行的 Daemon 同样有效**，是 Daemon 被意外结束后的标准恢复手段：

```powershell
dp daemon status        # 若输出 "Daemon is not running."，说明后台进程已不在
dp daemon restart       # 拉起（输出 "Daemon restarted." 即成功）
dp daemon status        # 确认恢复常驻
```

> 说明：Daemon 可能因外部原因被结束（任务管理器 / `taskkill` / 杀软 / 系统环境异常等）。未恢复前数据命令会自动降级为 Standalone 执行——结果正确，但没有缓存加速，重复分析同一批大文件会明显变慢。

### dp daemon flush — 只清缓存，进程保持

释放已缓存的数据实例与内存，Daemon 继续驻留；用于"数据文件已更新，需要强制重新加载"。

### 何时不需要手动管理

| 场景 | 说明 |
| --- | --- |
| 我想禁用 Daemon（排查问题） | 数据命令前先设置环境变量 `TDA_DAEMON_DISABLED=1`（会话内 `setx` 可持久化到用户级），命令将全部走 Standalone |
| Daemon 空闲 | 长时间无请求会自动退出；下一条数据命令再自动拉起 |
| 首次命令很慢 | 属正常（文件加载 + 建缓存）；同文件二次执行应命中缓存 |

---

## 附录：新增命令的文档模板（给维护者）

在 `Commands` 中增加新命令时，请同步在本文件补一条记录，复制以下模板：

```text
### dp <组> <子命令> — <一句话用途>

    dp <组> <子命令> "D:\data\demo_lot.stdf"

| 选项 | 说明 |
| --- | --- |
| ... | ... |

输出/说明：...
```

同时更新：README.md 速查表与场景表、ai-guide.md 决策表、本文件顶部速查总表；若命令改变输出规模量级，更新 [ai-guide.md](ai-guide.md) 分层表。
