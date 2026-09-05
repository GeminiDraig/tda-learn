## 范例程序列表


| 范例程序                                                             | 描述                                                                                                                              |
| ---------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| [Ex101.UseDataPrism](#ex101usedataprism)                             | 演示通过`DataPrism` 加载单个 STDF 文件，读取 Lot 基本信息（LotID、SublotID、站点列表等）的最基础流程                              |
| Ex101.UseStdf                                                        | 使用底层 STDF API 直接访问文件（**源码待补充**）                                                                                  |
| [Ex102.ExportParamsStatistics](#ex102exportparamsstatistics)         | 批量选取多个 STDF 文件，按 TestIDSet 过滤测试项，将各项统计指标（均值、标准差、良率、百分位数等）导出为 CSV                       |
| Ex102.ObtainTestItemsSummary                                         | 获取 STDF 文件中所有测试项的汇总信息（**源码待补充**）                                                                            |
| [Ex103.ExportRawData](#ex103exportrawdata)                           | 用DataPrism解析数据并导出raw data到 CSV 文件                                                                                     |
| [Ex105.ExportSummary](#ex105exportsummary)                           | 读取单个 STDF 文件，将 Yield 良率、HardBin / SoftBin 各站点统计数量汇整为 CSV 摘要报告                                            |
| [Ex201.UseMsgInConsole](#ex201usemsginconsole)                       | 演示在控制台程序中使用`Msg` 工具类（Debug / Info / Warning / Error）向文件写入分级日志                                            |
| [Ex202.UseMsgInWinform](#ex202usemsgsinwinform)                      | 演示在 WinForm 程序中通过按钮事件触发`Msg` 各级别日志输出                                                                         |
| [Ex203.UseMsgInConsoleAndWinform](#ex203usemsginconsoleandwinform)   | 演示在 WinForm + 控制台混合程序中同时使用`Msg` 进行日志输出                                                                       |
| [Ex204.CfgFileForDataPrismSetup](#ex204cfgfilefordataprismsetup)     | 演示如何用代码构建含多 Sublot / 多轮复测的`DataPrismSetup`，并序列化 / 反序列化为 JSON 配置文件（绝对路径与相对路径两种风格）     |
| Ex204.StdfSourceConfigByJson                                         | 通过 JSON 文件配置 STDF 数据源（**源码待补充**）                                                                                  |
| [Ex205.CfgFileForTestIDSet](#ex205cfgfilefortestidset)               | 演示从 STDF 文件中提取所有 TestID 并导出为 CSV，再从 CSV 读回，展示`TestIDSet` 配置文件读写完整流程                               |
| [Ex206.CfgFileForTestIDMappingSet](#ex206cfgfilefortestidmappingset) | 演示`TestIDMappingSet` 配置文件的读写流程（**主逻辑待补充**）                                                                     |
| [Ex301.RecordReaderBasics](#ex301recordreaderbasics)                 | 演示`RecordReader` 基本读取工作流（初始化→读取文件头→迭代记录→自动释放）及 Encoding / ValidationLevel 配置属性                 |
| [Ex401.UseStdfReader](#ex401usestdfreader)                           | 演示使用`RecordReader` 逐条解析 STDF 文件中所有记录类型（FAR/MIR/PTR/PRR 等），并对每条记录执行 `ToBytesCheck()` 逐字节完整性验证 |
| [Ex501.UseRecordWriter](#ex501userecordwriter)                       | 演示使用`RecordReader` 逐条读取源 STDF 文件，再通过 `RecordWriter` 写入新文件，展示 STDF 文件完整读写流程及写入统计摘要           |
| [Ex601.TDA.GUI](#ex601tdagui)                                        | 演示 TDA.GUI 全部 5 种图表控件（趋势图、直方图、累积概率图、箱线图、晶圆图）在同一 WinForm 窗口中的 Tab 页展示                    |
| [Ex602.TDA.GUI.EasyPlotForm](#ex602tdaguieasyplotform)               | 演示`EasyPlotForm` 快速画图窗口的用法：通过单例模式获取实例，传入数据后一键生成图表，无需手动编写 WinForm 布局代码                |

---

### 项目名格式

以Ex312.RecordReader为例：

* Ex表示这是一个Example项目
* Ex后的数字312，百位3表示Example项目分类编号，取值为十六进制1~F，不同值含义为：
  *  | Category     | Number | Description |
    | -------------- | -------- | ------------- |
    | StdfReader   | 1      |             |
    | StdfWriter   | 2      |             |
    | RecordReader | 3      |             |
    | RecordWriter | 4      |             |
    | ConfigFile   | 5      |             |
    | TDA.GUI      | 6      |             |
* Ex后的数字312，个十位为该大类下Example项目编号

### Ex1xx

##### Ex101.UseDataPrism

展示通过 DataPrism 访问 STDF 文件的基础流程：弹窗选取文件 → `Load()` 解析 → `DoStatisticAnalysis()` 统计 → 读取 MIR/SDR 中的 Lot 信息。

##### Ex101.UseStdf

使用底层 STDF API 直接访问文件（源码待补充）。

##### Ex102.ExportParamsStatistics

批量选取多个 STDF 文件，可选加载 TestIDSet 配置过滤测试项，将每个测试项的统计指标（均值、Sigma、FailRate、P1/P5/P10 等）导出为 CSV 汇总文件。

##### Ex102.ObtainTestItemsSummary

获取 STDF 文件中所有测试项的汇总信息（源码待补充）。

##### Ex103.ExportRawData

选取单个 STDF 文件，可选 TestIDSet 过滤，调用 `DataPrism.ExportRawDatas()` 将 Final Data 原始数值导出为 CSV。

##### Ex104.ExportFinalData

批量选取多个 STDF 文件，循环 `Load` / `DoStatisticAnalysis` 后，调用 `DataPrism.ExportRawDatas()` 将所有文件的 Final Data 合并导出为单一 CSV。

##### Ex105.ExportSummary

读取单个 STDF 文件，将 Yield 数量统计、HardBin 分布、SoftBin 分布（按站点细分）逐一写入 `.sum.csv` 摘要文件。

### Ex2xx

##### Ex201.UseMsgInConsole

演示在控制台程序中使用 `Msg` 工具类：配置 `SetupTextWriter` 输出到文件，调用 Debug / Info / Warning / Error 各级别方法。

##### Ex202.UseMsgInWinform

演示在 WinForm 程序中，通过工具栏按钮点击事件调用 `Msg` 各级别方法，验证日志在 WinForm 环境下的输出行为。

##### Ex203.UseMsgInConsoleAndWinform

演示 `Msg` 同时挂载控制台（Console）与 WinForm 两个输出目标，展示混合宿主环境下的日志输出用法。

##### Ex204.CfgFileForDataPrismSetup

演示如何通过代码构建包含多 Sublot、多轮复测的 `DataPrismSetup`，并序列化 / 反序列化为 JSON 配置文件，支持绝对路径与相对路径两种风格。

##### Ex204.StdfSourceConfigByJson

通过 JSON 文件配置 STDF 数据源（源码待补充）。

##### Ex205.CfgFileForTestIDSet

演示 `TestIDSet` 配置文件的完整读写流程：解析 STDF → `TestIDSet.ExportToFile()` 导出 CSV → `ShowDiaglog4TestIDSet()` 读回并打印。

##### Ex206.CfgFileForTestIDMappingSet

演示 `TestIDMappingSet` 配置文件的读写流程（主逻辑待补充）。

### Ex3xx

##### Ex301.RecordReaderBasics

演示 `RecordReader` 的基本读取工作流（初始化 → `ReadHeader()` → `Read()` 迭代 → `using` 自动释放），以及 Encoding 与 ValidationLevel 配置属性的对比效果。

### Ex4xx

##### Ex401.UseStdfReader

演示使用 `RecordReader` 逐条解析 STDF 文件中所有记录类型（FAR / MIR / PTR / PRR 等），并对每条记录调用 `ToBytesCheck()` 进行逐字节完整性验证。

### Ex5xx

##### Ex501.UseRecordWriter

演示 `RecordWriter` 的基本写入流程：

1. 弹窗选取源 STDF 文件
2. 用 `RecordReader.ReadNext()` 逐条读取所有 Record
3. 用 `RecordWriter.WriteRecord()` 将每条 Record 写入同目录下的 `.copy.stdf` 输出文件
4. 读写结束后打印各类型 Record 的数量统计摘要（通过 `RecordWriter.RecordCount` / `TotalRecords`）

### Ex6xx

##### Ex601.TDA.GUI

演示 TDA.GUI 全部 5 种图表控件（TrendChart / HistogramChart / CumulativeProbabilityChart / BoxPlotChart / WaferMapChart）通过 TabControl 在同一 WinForm 窗口中分页展示，使用 `SampleDataGenerator` 生成的模拟数据验证每种图表的渲染效果。

##### Ex602.TDA.GUI.EasyPlotForm

演示 `EasyPlotForm` 快速画图窗口的两种使用方式：

1. **Ex60201_QuickPlotTrend**：最简 3 行代码用法 — `GetInstance()` → `PlotTrend()` → `ShowDialog()`
2. **Ex60202_PlotAllChartTypesInSequence**：依次绘制全部 5 种图表，每步通过 MessageBox 提示用户查看

核心 API：

- `EasyPlotForm.GetInstance()` 获取单例实例
- `PlotTrend()` / `PlotHistogram()` / `PlotCumulativeProbability()` / `PlotBoxPlot()` / `PlotWaferMap()` 分别绘制对应图表
- 窗口关闭时自动 `Hide()` 而非销毁，可重复使用
