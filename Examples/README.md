# Examples 范例程序说明

本目录包含 TDA Framework 的系列范例程序（Examples），按分类编号组织，每个范例对应一个独立可运行的 .NET 项目。

## 范例程序列表

| 范例程序                                                        | 描述                                                                                                                   |
| ---------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| [Ex101.UseDataPrism](#ex101usedataprism)                         | 演示 `DataPrism` 的多种构造方式、数据处理流程及各阶段数据访问方法（直接打印到控制台）                                  |
| [Ex102.ExportParamsStatistics](#ex102exportparamsstatistics)     | 批量统计多个 STDF 文件中测试参数的摘要（Mean / Stdev / 百分位数等），按多种匹配模式导出为 CSV                          |
| [Ex103.ExportRawData](#ex103exportrawdata)                       | 用 `DataPrism` 解析 STDF 并导出 raw data 到 CSV，支持按 TestID 集合 / 选择器 / Part 过滤                              |
| [Ex105.ExportSummary](#ex105exportsummary)                       | 导出 HBin / SBin 统计摘要（Yield、Bin 分布）到 CSV，支持自定义格式与多批次合并导出                                     |
| [Ex106.ExportPartSummary](#ex106exportpartsummary)               | 导出每颗 Part/Device 的信息列表（Pass/Fail、HardBin/SoftBin、坐标、数据来源等）到 CSV                                  |
| [Ex201.UseMsgInConsole](#ex201usemsginconsole)                   | 演示在控制台程序中使用 `Msg` 消息系统：自动/显式初始化、六级日志、结构化 Tag 与对象日志等                              |
| [Ex202.UseMsgInWinform](#ex202usemsginwinform)                   | 演示在 WinForm 程序中通过界面按钮触发 `Msg` 各级别日志输出、日志过滤与配置管理                                         |
| [Ex203.UseMsgInConsoleAndWinform](#ex203usemsginconsoleandwinform) | 演示在「控制台 + WinForm」混合宿主中使用 `Msg` 消息系统                                                              |
| [Ex204.CfgFileForDataPrismSetup](#ex204cfgfilefordataprismsetup) | 演示构建含多 Sublot / 多轮复测的 `DataPrismSetup`，并序列化 / 反序列化为 JSON 配置文件（绝对 / 相对路径两种风格）     |
| [Ex205.CfgFileForTestIDSet](#ex205cfgfilefortestidset)           | 演示 `TestIDSet` 配置文件的读写流程：从 STDF 导出全部 TestID 到 CSV，再读回并打印                                     |
| [Ex206.CfgFileForTestIDProfileSet](#ex206cfgfilefortestidprofileset) | 演示 `TestIDProfileSet` 配置文件的创建、读写与从 `TestIDSet` 转换                                                    |
| [Ex301.UseRecordReader](#ex301userecordreader)                   | 演示 `RecordReader` 读取 STDF 文件：基本工作流、显式关闭、通用 / 强类型逐条读取，及解压模式（Buffered / Streaming）对比 |
| [Ex302.UseRecordWriter](#ex302userecordwriter)                   | 演示用 `RecordReader` 逐条读取源文件、`RecordWriter` 写入新文件，展示 STDF 完整读写流程及写入统计摘要                 |
| [Ex601.GUICharts](#ex601guicharts)                               | 演示 TDA.GUI 全部 5 种图表控件（趋势图 / 直方图 / 累积概率图 / 箱线图 / 晶圆图）在同一窗口 Tab 页中的展示             |
| [Ex602.GUIEasyPlotForm](#ex602guieasyplotform)                   | 演示 `EasyPlotForm` 快速绘图窗口与 `DataGridView` 联动：多选参数自动绘图                                              |
| [Ex603.FlexiblePlotForm](#ex603flexibleplotform)                 | 演示 `FlexiblePlotForm` 按需多图布局：根据选中图表数量自动适配（1 图全屏 / 2 图左右 / 3~4 图 2×2 网格）              |
| [Ex701.LotYieldReport](#ex701lotyieldreport)                     | 演示 TDA.Report 良率报告模板：从 STDF / NEDA CSV / 演示数据生成单文件自包含的 HTML 良率报告                           |

---

## 项目命名格式

以 `Ex601.GUICharts` 为例：

* `Ex` 表示这是一个 Example（范例）项目
* `Ex` 后的数字，百位表示 Example 项目的分类编号，取值为十六进制 1 ~ F，不同值含义为：

  | 百位 | 分类                       | 包含范例                |
  | ---- | -------------------------- | ----------------------- |
  | 1    | DataPrism 数据访问与导出   | Ex101 ~ Ex106           |
  | 2    | Msg 日志工具与 TDA 配置文件 | Ex201 ~ Ex206           |
  | 3    | STDF 记录级读写（RecordReader / RecordWriter） | Ex301 ~ Ex302 |
  | 6    | TDA.GUI 可视化控件         | Ex601 ~ Ex603           |
  | 7    | TDA.Report 报告            | Ex701                   |

* `Ex` 后的十位与个位为该分类下的范例序号；同一范例内的多个演示场景以扩展编号区分（如 `Ex10301`、`Ex10311`，末两位表示子演示序号，数字越大演示越进阶）
* 点号（`.`）后为该范例的用途描述名，如 `UseRecordReader`、`GUIEasyPlotForm`

---

## Ex1xx：DataPrism 数据访问与导出

### Ex101.UseDataPrism

演示 `DataPrism` 的多种构造方式、数据处理流程及各阶段数据访问方法。所有演示均直接打印数据到控制台，不导出文件。默认运行 `Ex10101`，其余演示通过取消 `Program.cs` 中注释启用。

| 演示文件 | 说明 |
| --- | --- |
| `Ex10101_UseDataPrism` | 最简流程：弹窗选 STDF → `Load()` → `DoStatisticAnalysis()` → 打印 MIR/SDR 元数据（LotID、SublotID、站点、Part/测试项数量等） |
| `Ex10102_DifferentInputFormats` | 三种不同的 `DataPrism` 构造方式 |
| `Ex10103_AccessMetaInfoAndRawData` | `Load()` 后访问元信息与原始测试值 |
| `Ex10104_AccessStatistics` | 分析后访问统计数据与 Bin 汇总 |
| `Ex10105_ApplyTestIDProfile` | `ApplyTestIDProfile` 过滤与重命名 TestID |

### Ex102.ExportParamsStatistics

批量打开多个 STDF 文件，统计指定测试参数的摘要（TestedCnt、FailedCnt、Mean、Stdev、P1/P5/P50/P95/P99 等），导出为 CSV。支持宽格式 / 长格式两种布局，以及按 TestText / TestNumber 两种方式匹配参数。默认运行 `Ex10203`。

| 演示文件 | 说明 |
| --- | --- |
| `Ex10201_ExportParamsStatistics` | 宽格式导出统计结果 |
| `Ex10202_ExportParamsStatisticsLongFormat` | 长格式导出（直接匹配） |
| `Ex10203_ExportStatisticsByTestText` | 基于 TestText 匹配参数后导出 |
| `Ex10204_ExportStatisticsByTestNumber` | 基于 TestNumber 匹配参数后导出 |

### Ex103.ExportRawData

选取 STDF 文件后用 `DataPrism` 解析，将 raw data（原始测试值）导出为 CSV。默认运行 `Ex10301`。

| 演示文件 | 说明 |
| --- | --- |
| `Ex10301_ExportDataWithSpecifiedTestIDs` | 指定 TestID 集合导出对应测试项数据 |
| `Ex10302_ExportDataWithTestIDSelector` | 通过 TestID 选择器（交互式）导出 |
| `Ex10311_ExportDataWithPartFilter` | 先按 Part 过滤，再导出数据 |

### Ex105.ExportSummary

将 `DataPrism` 分析结果中的 HBin / SBin 统计摘要（Yield、各 Bin 分布数量）导出为 CSV。默认运行 `Ex10503`。

| 演示文件 | 说明 |
| --- | --- |
| `Ex10501_ExportSummary` | 调用 `DataPrism.ExportSummary` 导出基本摘要 |
| `Ex10502_ExportSummaryCustomizeFormat` | 自定义输出格式后导出 |
| `Ex10503_ExportSummaryMultiLot` | 多批次（IEnumerable）数据合并导出 |

### Ex106.ExportPartSummary

将每个 Part/Device 的判定信息列表导出到 CSV：Pass/Fail 结果、HardBin/SoftBin、芯片坐标、数据来源（批次 / 轮次）等字段。

---

## Ex2xx：Msg 日志工具与 TDA 配置文件

### Ex201.UseMsgInConsole

在纯控制台程序中演示 `Msg` 消息系统的完整用法，默认 5 个演示全部运行。

| 演示文件 | 说明 |
| --- | --- |
| `Ex20101_AutoInitAndSixLevels` | 自动初始化并输出六级日志（Debug ~ Fatal 等） |
| `Ex20102_ExplicitInitWithConfig` | 显式初始化并加载配置 |
| `Ex20103_StructuredTagLogging` | 结构化 Tag 日志 |
| `Ex20104_StructuredObjectAndException` | 结构化对象与异常日志 |
| `Ex20105_BlankLineAndLegacyApi` | 空行控制与旧 API 兼容用法 |

### Ex202.UseMsgInWinform

在 WinForm 程序中通过窗体工具栏按钮触发 `Msg` 各级别消息输出，演示 WinForm 环境下的日志行为，并提供 Tag 输入、异常模拟、日志级别过滤与配置管理等界面功能。

### Ex203.UseMsgInConsoleAndWinform

在「控制台 + WinForm」混合宿主程序中使用 `Msg`：控制台窗口输出日志的同时，WinForm 窗体可通过按钮触发消息，展示双输出目标的日志用法。

### Ex204.CfgFileForDataPrismSetup

演示如何通过代码构建 `DataPrismSetup` 配置（描述一个 Lot 的所有 Sublot、测试轮次、STDF 文件路径与复测 HardBin 范围），并序列化 / 反序列化为 JSON 配置文件。默认运行 `Ex20401` 与 `Ex20402`。目录下 `ExampleSetupFiles/` 提供示例配置文件。

| 演示文件 | 说明 |
| --- | --- |
| `Ex20401_DataPrismSetupWithAbsolutePaths` | 使用绝对路径风格构建并读写 JSON |
| `Ex20402_DataPrismSetupWithRelativePaths` | 使用相对路径风格构建并读写 JSON |

### Ex205.CfgFileForTestIDSet

演示 `TestIDSet` 配置文件的完整读写流程：打开 STDF → 将全部 TestID 导出到 CSV → 重新加载 CSV 并打印。

| 演示文件 | 说明 |
| --- | --- |
| `Ex20501_TestIDSetCfgFileWriteRead` | TestIDSet 配置文件写出与读回 |

### Ex206.CfgFileForTestIDProfileSet

演示 `TestIDProfileSet` 配置文件的创建、读写与转换。默认运行 `Ex20601`。

| 演示文件 | 说明 |
| --- | --- |
| `Ex20601_ProfileFromStdfAndSaveCsv` | 从 STDF 文件生成 Profile 并保存为 CSV |
| `Ex20602_CreateProfileManuallyAndSaveCsv` | 手动构建 Profile 并保存为 CSV |
| `Ex20603_LoadProfileFromCsv` | 从 CSV 加载 Profile |
| `Ex20604_ConvertFromTestIDSet` | 从 `TestIDSet` 转换为 `TestIDProfileSet` |

---

## Ex3xx：STDF 记录级读写

### Ex301.UseRecordReader

演示 `RecordReader` 读取 STDF 文件的核心用法：选择文件与格式校验 → 初始化 → 读取文件头（FAR）→ 按基类 / 具体类型逐条读取记录 → 显式关闭或自动释放资源，以及对压缩 STDF 文件（.gz/.zip/.tar.gz）的解压模式配置。默认运行 `Ex30101`。

| 演示文件 | 说明 |
| --- | --- |
| `Ex30101_BasicWorkflow` | 基本工作流：`IsValidStdFile` 校验 → `new RecordReader(file)` → 读 FAR → `ReadNext()` 迭代 → `using` 自动释放 |
| `Ex30102_HowToCloseRecordReader` | 显式调用 `Close()` 控制读取器生命周期（而非依赖 using 自动释放） |
| `Ex30103_ReadRecordsAsGeneralRecordType` | 以通用 `Record` 类型读取所有记录并打印 |
| `Ex30104_ReadRecordAsSpecificRecordType` | 按 TypeID 将记录转换为具体强类型后打印，获取各类型专属数据 |
| `Ex30105_ConfigurationDemo` | 对比 `StdfDecompressionMode.Buffered`（全量解压、可随机访问）与 `Streaming`（流式解压、内存恒定）两种构造模式；建议用压缩文件运行 |

### Ex302.UseRecordWriter

演示 `RecordReader` + `RecordWriter` 的完整读写流程：弹窗选取源 STDF → 校验 → 逐条 `ReadNext()` 读取 → 逐条 `WriteRecord()` 写入同目录下的 `.copy.stdf` 新文件，最后通过 `RecordWriter.RecordCount` / `TotalRecords` 打印各类型 Record 的写入统计摘要。

| 演示文件 | 说明 |
| --- | --- |
| `Ex30201_CopyStdfFile` | 将源 STDF 逐条 Record 拷贝为新文件并输出统计摘要 |

> 说明：原分类 Ex4xx（STDF 记录级解析）与 Ex5xx（RecordWriter）已并入本分类，分别由 Ex301（读取）与 Ex302（写入）承接，分类编号 4、5 空出。

---

## Ex6xx：TDA.GUI 可视化

### Ex601.GUICharts

演示 TDA.GUI 全部 5 种图表控件——趋势图（TrendChart）、直方图（HistogramChart）、累积概率图（CumulativeProbabilityChart）、箱线图（BoxPlotChart）、晶圆图（WaferMapChart），通过 `MainForm` 的 Tab 页分页展示；窗体提供 TestCase 选择与数据参数量（1/2/3）选择以观察不同渲染效果。

### Ex602.GUIEasyPlotForm

演示 `EasyPlotForm` 快速绘图窗口与主窗体的联动用法：`MainForm` 以 `DataGridView` 列出各参数及其统计数据，用户可多选行，选中的参数将自动绘制到内嵌的 `EasyPlotForm` 中，无需手动编写 WinForm 布局代码。

### Ex603.FlexiblePlotForm

演示 `FlexiblePlotForm` 的按需多图布局能力：`MainForm` 左侧列出参数并选择图表布局，右侧展示 `FlexiblePlotForm`；调用方可自由组合趋势图 / 直方图 / 累积概率图 / 箱线图，窗体根据选中的图表数量自动适配布局（1 图全屏、2 图左右分栏、3~4 图 2×2 网格）。

---

## Ex7xx：TDA.Report 报告

### Ex701.LotYieldReport

演示 `TDA.Report` 的 LotYieldReport 报告模板，从测试数据生成单文件自包含的 HTML 良率报告（图表经 `ChartService` 直出 PNG 并以 base64 data URI 内嵌）。

用法（命令行）：

```text
Ex701.LotYieldReport <input.stdf> [-o out.html]
Ex701.LotYieldReport input.neda_raw.csv [-o out.html]
Ex701.LotYieldReport --demo [-o out.html]      # 生成演示数据并出报告
```

| 演示文件 | 说明 |
| --- | --- |
| `Ex70101_BuildLotYieldReport` | 全流程：加载数据（STDF / NEDA CSV / demo）→ 适配为结构化摘要 → 渲染参数图表 → 模板装配 → 输出 HTML |
