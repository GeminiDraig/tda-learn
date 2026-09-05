using System.Data;
using TDA.GUI;
using TDA.GUI.DataModel;
using TDA.GUI.Statistics;
using TDA.GUI.Utilities;

using FlexPlot = TDA.GUI.FlexiblePlotForm;

namespace Ex603.FlexiblePlotForm;

/// <summary>
/// Ex603 主窗体：左侧 DataGridView 列出所有参数及其统计数据，右侧通过 CheckBox 选择图表布局，
/// 选中的参数数据绘制到 FlexiblePlotForm 中。
/// 与 Ex602 的区别：使用 FlexiblePlotForm 替代 EasyPlotForm，支持按需选择图表类型。
/// </summary>
public partial class MainForm : Form
{
    #region 字段

    private FlexPlot? _flexPlotForm;
    private DataTable _synopsisTable = null!;
    private List<ParameterInfo> _allParams = new();

    #endregion

    #region 构造与初始化

    public MainForm()
    {
        InitializeComponent();
        PopulateSynopsisTable();
        BindEvents();
    }

    private void BindEvents()
    {
        _dataGridView.SelectionChanged += OnDataGridViewSelectionChanged;
        _layoutCombo.SelectedIndexChanged += OnLayoutChanged;
        this.Load += OnMainFormLoad;
    }

    private void OnMainFormLoad(object? sender, EventArgs e)
    {
        // 初始布局：全部 4 种图表
        _layoutCombo.SelectedIndex = 4;
    }

    #endregion

    #region DataGridView 数据填充

    /// <summary>
    /// 将 SampleData 中所有参数的统计信息填充到 DataGridView，
    /// 列结构与 Ex602 保持一致。
    /// </summary>
    private void PopulateSynopsisTable()
    {
        _synopsisTable = new DataTable();

        // ── 定义列 ──
        _synopsisTable.Columns.Add("TestNum", typeof(string));
        _synopsisTable.Columns.Add("TestText", typeof(string));
        _synopsisTable.Columns.Add("TestName", typeof(string));
        _synopsisTable.Columns.Add("Tested", typeof(string));
        _synopsisTable.Columns.Add("Failed", typeof(string));
        _synopsisTable.Columns.Add("FailRate", typeof(string));
        _synopsisTable.Columns.Add("Unit", typeof(string));
        _synopsisTable.Columns.Add("Hsl", typeof(string));
        _synopsisTable.Columns.Add("Lsl", typeof(string));
        _synopsisTable.Columns.Add("Stdev", typeof(string));
        _synopsisTable.Columns.Add("Min", typeof(string));
        _synopsisTable.Columns.Add("Mean", typeof(string));
        _synopsisTable.Columns.Add("Median", typeof(string));
        _synopsisTable.Columns.Add("Max", typeof(string));
        _synopsisTable.Columns.Add("Cp", typeof(string));
        _synopsisTable.Columns.Add("Cpk", typeof(string));
        _synopsisTable.Columns.Add("Cpl", typeof(string));
        _synopsisTable.Columns.Add("Cpu", typeof(string));
        _synopsisTable.Columns.Add("P1", typeof(string));
        _synopsisTable.Columns.Add("P5", typeof(string));
        _synopsisTable.Columns.Add("P10", typeof(string));
        _synopsisTable.Columns.Add("P25", typeof(string));
        _synopsisTable.Columns.Add("P75", typeof(string));
        _synopsisTable.Columns.Add("P90", typeof(string));
        _synopsisTable.Columns.Add("P95", typeof(string));
        _synopsisTable.Columns.Add("P99", typeof(string));

        // ── 收集所有参数名 ──
        var paramNames = new[]
        {
            SampleData.PN__OSPS_VFIM_VDD1,
            SampleData.PN__OSPS_VFIM_VDD2,
            SampleData.PN__OSPS_VFIM_VDD3_0V6,
            SampleData.PN__OSPS_VFIM_VDD3_0V9,
            SampleData.PN__VPU_LDODIG,
            SampleData.PN__VPU_LDOANA,
            SampleData.PN__VPU_LDOADC,
            SampleData.PN__VPU_LDODAC,
            SampleData.PN__X_handler,
            SampleData.PN__Y_handler,
            SampleData.PN__LDODAC_VPretrim,
            SampleData.PN__LDODAC_VTrim,
            SampleData.PN__LDODAC_TrimCode,
            SampleData.PN__LDODAC_VPosttrim,
            SampleData.PN__LDOADC_VPretrim,
            SampleData.PN__LDOADC_VTrim,
            SampleData.PN__LDOADC_TrimCode,
            SampleData.PN__LDOADC_VPosttrim,
            SampleData.PN__VBG_VPretrim,
            SampleData.PN__VBG_VTrim,
            SampleData.PN__VBG_TrimCode,
            SampleData.PN__VBG_VPosttrim,
            SampleData.PN__Mbist,
            SampleData.PN__StuckAt,
            SampleData.PN__Transition,
            SampleData.PN__Iddq1,
            SampleData.PN__Iddq2,
            SampleData.PN__Iddq3
        };

        // ── 逐参数获取数据并计算统计量 ──
        int testNum = 1;
        foreach (string pn in paramNames)
        {
            var dataCollection = SampleData.GenerateHistogramData(pn);
            if (dataCollection.Series.Count == 0) continue;

            var series = dataCollection.Series[0];
            double[] values = series.Values;
            if (values.Length == 0) continue;

            var sorted = values.OrderBy(v => v).ToArray();
            double mean = StatisticsHelper.Mean(values);
            double stdDev = StatisticsHelper.StdDev(values);
            double median = StatisticsHelper.Quantile(sorted, 0.5);

            // 规格限
            double? usl = dataCollection.USL;
            double? lsl = dataCollection.LSL;

            // CP/CPK
            string cpStr = "-", cpkStr = "-", cplStr = "-", cpuStr = "-";
            if (usl.HasValue && lsl.HasValue && !double.IsNaN(stdDev) && stdDev > 0)
            {
                double cp = StatisticsHelper.CP(usl.Value, lsl.Value, stdDev);
                double cpk = StatisticsHelper.CPK(usl.Value, lsl.Value, mean, stdDev);
                double cpu = (usl.Value - mean) / (3.0 * stdDev);
                double cpl = (mean - lsl.Value) / (3.0 * stdDev);
                cpStr = cp.ToString("F3");
                cpkStr = cpk.ToString("F3");
                cpuStr = cpu.ToString("F3");
                cplStr = cpl.ToString("F3");
            }

            // 分位数
            string QuantileStr(double p) => StatisticsHelper.Quantile(sorted, p).ToString("G4");

            DataRow row = _synopsisTable.NewRow();
            row["TestNum"] = testNum.ToString();
            row["TestText"] = pn;
            row["TestName"] = pn;
            row["Tested"] = values.Length.ToString();
            row["Failed"] = "-";
            row["FailRate"] = "-";
            row["Unit"] = dataCollection.Unit ?? "";
            row["Hsl"] = usl?.ToString("G4") ?? "";
            row["Lsl"] = lsl?.ToString("G4") ?? "";
            row["Stdev"] = stdDev.ToString("G4");
            row["Min"] = sorted[0].ToString("G4");
            row["Mean"] = mean.ToString("G4");
            row["Median"] = median.ToString("G4");
            row["Max"] = sorted[^1].ToString("G4");
            row["Cp"] = cpStr;
            row["Cpk"] = cpkStr;
            row["Cpl"] = cplStr;
            row["Cpu"] = cpuStr;
            row["P1"] = QuantileStr(0.01);
            row["P5"] = QuantileStr(0.05);
            row["P10"] = QuantileStr(0.10);
            row["P25"] = QuantileStr(0.25);
            row["P75"] = QuantileStr(0.75);
            row["P90"] = QuantileStr(0.90);
            row["P95"] = QuantileStr(0.95);
            row["P99"] = QuantileStr(0.99);

            _synopsisTable.Rows.Add(row);

            _allParams.Add(new ParameterInfo
            {
                TestText = pn,
                TestNum = testNum,
                DataCollection = dataCollection
            });

            testNum++;
        }

        _dataGridView.DataSource = _synopsisTable;

        // 列头加粗
        _dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(_dataGridView.Font, FontStyle.Bold);

        // 自适应列宽
        foreach (DataGridViewColumn col in _dataGridView.Columns)
        {
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }

    #endregion

    #region 事件处理 — DataGridView 选中行变化

    private void OnDataGridViewSelectionChanged(object? sender, EventArgs e)
    {
        if (_flexPlotForm == null || _flexPlotForm.IsDisposed) return;
        UpdateChartsForSelection();
        _flexPlotForm.Activate();
    }

    private void OnLayoutChanged(object? sender, EventArgs e)
    {
        // 根据 ComboBox 选择重建 FlexiblePlotForm
        var layout = GetSelectedLayout();
        RecreateFlexiblePlotForm(layout);
        UpdateChartsForSelection();
    }

    private ChartLayout GetSelectedLayout()
    {
        return _layoutCombo.SelectedIndex switch
        {
            0 => ChartLayout.Trend,
            1 => ChartLayout.Histogram,
            2 => ChartLayout.CumulativeProbability,
            3 => ChartLayout.BoxPlot,
            4 => ChartLayout.All,
            5 => ChartLayout.Trend | ChartLayout.Histogram,
            6 => ChartLayout.Trend | ChartLayout.Histogram | ChartLayout.BoxPlot,
            7 => ChartLayout.Histogram | ChartLayout.BoxPlot,
            8 => ChartLayout.Histogram | ChartLayout.CumulativeProbability,
            _ => ChartLayout.All,
        };
    }

    private void RecreateFlexiblePlotForm(ChartLayout layout)
    {
        _flexPlotForm?.Close();
        _flexPlotForm?.Dispose();

        _flexPlotForm = new FlexPlot(layout)
        {
            StartPosition = FormStartPosition.CenterScreen,
            Size = new Size(1200, 800)
        };

        // 重建后同步对数压缩状态
        SyncLogCompression();

        _flexPlotForm.Show();
    }

    private void SyncLogCompression()
    {
        if (_flexPlotForm == null || _flexPlotForm.IsDisposed) return;

        var mode = _logCompressionCheckBox.Checked
            ? LogCompressionMode.Tails
            : LogCompressionMode.None;

        _flexPlotForm.SetCumulativeProbabilityTailStretch(mode);
    }

    private void OnLogCompressionChanged(object? sender, EventArgs e)
    {
        SyncLogCompression();
    }

    private void UpdateChartsForSelection()
    {
        if (_flexPlotForm == null || _flexPlotForm.IsDisposed) return;

        var selectedRows = _dataGridView.SelectedRows;
        if (selectedRows.Count == 0)
        {
            if (_dataGridView.Rows.Count > 0)
                _dataGridView.Rows[0].Selected = true;
            return;
        }

        // 收集选中参数的数据
        var selectedCollections = new List<TDADataCollection>();
        var selectedParamNames = new List<string>();

        foreach (DataGridViewRow row in selectedRows)
        {
            if (row.Index >= _allParams.Count) continue;
            var param = _allParams[row.Index];
            selectedCollections.Add(param.DataCollection);
            selectedParamNames.Add(param.TestText);
        }

        if (selectedCollections.Count == 0) return;

        // 构建多系列数据集合，所有图表共用
        var multiSeriesData = new TDADataCollection
        {
            XAxisLabel = "Sample Index",
            YAxisLabel = "Value",
            Title = string.Join(" | ", selectedParamNames),
            Unit = selectedCollections[0].Unit,
            Series = selectedCollections
                .SelectMany(c => c.Series)
                .ToList(),
            USL = selectedCollections[0].USL,
            LSL = selectedCollections[0].LSL
        };

        // FlexiblePlotForm 内部会根据 ChartLayout 自动分发给对应的图表
        _flexPlotForm.LoadData(multiSeriesData);
    }

    #endregion

    #region 内部类型

    private class ParameterInfo
    {
        public string TestText { get; set; } = string.Empty;
        public int TestNum { get; set; }
        public TDADataCollection DataCollection { get; set; } = null!;
    }

    #endregion
}
