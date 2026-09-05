using TDA.GUI.Controls;

namespace Ex601.GUICharts;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _tabControl = new TabControl();
        _tabPageTrend = new TabPage();
        _trendChart = new TrendChart();
        _tabPageHistogram = new TabPage();
        _histogramChart = new HistogramChart();
        _tabPageProbability = new TabPage();
        _probabilityChart = new CumulativeProbabilityChart();
        _tabPageBoxPlot = new TabPage();
        _boxPlotChart = new BoxPlotChart();
        _tabPageWaferMap = new TabPage();
        _waferMapChart = new WaferMapChart();
        toolStrip1 = new ToolStrip();
        toolStripComboBox1 = new ToolStripComboBox();
        _tabControl.SuspendLayout();
        _tabPageTrend.SuspendLayout();
        _tabPageHistogram.SuspendLayout();
        _tabPageProbability.SuspendLayout();
        _tabPageBoxPlot.SuspendLayout();
        _tabPageWaferMap.SuspendLayout();
        toolStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // _tabControl
        // 
        _tabControl.Controls.Add(_tabPageTrend);
        _tabControl.Controls.Add(_tabPageHistogram);
        _tabControl.Controls.Add(_tabPageProbability);
        _tabControl.Controls.Add(_tabPageBoxPlot);
        _tabControl.Controls.Add(_tabPageWaferMap);
        _tabControl.Dock = DockStyle.Fill;
        _tabControl.Location = new Point(0, 28);
        _tabControl.Name = "_tabControl";
        _tabControl.SelectedIndex = 0;
        _tabControl.Size = new Size(1182, 725);
        _tabControl.TabIndex = 0;
        // 
        // _tabPageTrend
        // 
        _tabPageTrend.Controls.Add(_trendChart);
        _tabPageTrend.Location = new Point(4, 29);
        _tabPageTrend.Name = "_tabPageTrend";
        _tabPageTrend.Size = new Size(1174, 692);
        _tabPageTrend.TabIndex = 0;
        _tabPageTrend.Text = "Trend Chart";
        // 
        // _trendChart
        // 
        _trendChart.ChartTitle = "Trend Chart";
        _trendChart.Location = new Point(0, 0);
        _trendChart.Name = "_trendChart";
        _trendChart.Size = new Size(1174, 720);
        _trendChart.TabIndex = 0;
        _trendChart.XAxisLabel = "Index";
        _trendChart.YAxisLabel = "Value";
        // 
        // _tabPageHistogram
        // 
        _tabPageHistogram.Controls.Add(_histogramChart);
        _tabPageHistogram.Location = new Point(4, 29);
        _tabPageHistogram.Name = "_tabPageHistogram";
        _tabPageHistogram.Size = new Size(1010, 549);
        _tabPageHistogram.TabIndex = 1;
        _tabPageHistogram.Text = "Histogram";
        // 
        // _histogramChart
        // 
        _histogramChart.BinMethod = TDA.GUI.Statistics.BinMethod.Scott;
        _histogramChart.ChartTitle = "Histogram";
        _histogramChart.Dock = DockStyle.Fill;
        _histogramChart.Location = new Point(0, 0);
        _histogramChart.Name = "_histogramChart";
        _histogramChart.Size = new Size(1010, 549);
        _histogramChart.TabIndex = 0;
        _histogramChart.XAxisLabel = "Value";
        _histogramChart.YAxisLabel = "Frequency";
        // 
        // _tabPageProbability
        // 
        _tabPageProbability.Controls.Add(_probabilityChart);
        _tabPageProbability.Location = new Point(4, 29);
        _tabPageProbability.Name = "_tabPageProbability";
        _tabPageProbability.Size = new Size(1010, 549);
        _tabPageProbability.TabIndex = 2;
        _tabPageProbability.Text = "Cumulative Probability";
        // 
        // _probabilityChart
        // 
        _probabilityChart.ChartTitle = "Cumulative Probability";
        _probabilityChart.Dock = DockStyle.Fill;
        _probabilityChart.Location = new Point(0, 0);
        _probabilityChart.Name = "_probabilityChart";
        _probabilityChart.ScaleType = TDA.GUI.Statistics.ProbabilityScaleType.Normal;
        _probabilityChart.Size = new Size(1010, 549);
        _probabilityChart.TabIndex = 0;
        _probabilityChart.XAxisLabel = "Value";
        _probabilityChart.YAxisLabel = "Cumulative Probability";
        // 
        // _tabPageBoxPlot
        // 
        _tabPageBoxPlot.Controls.Add(_boxPlotChart);
        _tabPageBoxPlot.Location = new Point(4, 29);
        _tabPageBoxPlot.Name = "_tabPageBoxPlot";
        _tabPageBoxPlot.Size = new Size(1010, 549);
        _tabPageBoxPlot.TabIndex = 3;
        _tabPageBoxPlot.Text = "Box Plot";
        // 
        // _boxPlotChart
        // 
        _boxPlotChart.ChartTitle = "Box Plot";
        _boxPlotChart.Dock = DockStyle.Fill;
        _boxPlotChart.Location = new Point(0, 0);
        _boxPlotChart.Name = "_boxPlotChart";
        _boxPlotChart.Size = new Size(1010, 549);
        _boxPlotChart.TabIndex = 0;
        _boxPlotChart.XAxisLabel = "Group";
        _boxPlotChart.YAxisLabel = "Value";
        // 
        // _tabPageWaferMap
        // 
        _tabPageWaferMap.Controls.Add(_waferMapChart);
        _tabPageWaferMap.Location = new Point(4, 29);
        _tabPageWaferMap.Name = "_tabPageWaferMap";
        _tabPageWaferMap.Size = new Size(1010, 549);
        _tabPageWaferMap.TabIndex = 4;
        _tabPageWaferMap.Text = "Wafer Map";
        // 
        // _waferMapChart
        // 
        _waferMapChart.ChartTitle = "Wafer Map";
        _waferMapChart.Dock = DockStyle.Fill;
        _waferMapChart.Location = new Point(0, 0);
        _waferMapChart.Name = "_waferMapChart";
        _waferMapChart.Size = new Size(1010, 549);
        _waferMapChart.TabIndex = 0;
        _waferMapChart.XAxisLabel = "Column";
        _waferMapChart.YAxisLabel = "Row";
        // 
        // toolStrip1
        // 
        toolStrip1.ImageScalingSize = new Size(20, 20);
        toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripComboBox1 });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(1182, 28);
        toolStrip1.TabIndex = 2;
        toolStrip1.Text = "toolStrip1";
        // 
        // toolStripComboBox1
        // 
        toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        toolStripComboBox1.Items.AddRange(new object[] { 1, 2, 3 });
        toolStripComboBox1.Name = "toolStripComboBox1";
        toolStripComboBox1.Size = new Size(121, 28);
        toolStripComboBox1.SelectedIndexChanged += OnSelectionChanged;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(9F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1182, 753);
        Controls.Add(_tabControl);
        Controls.Add(toolStrip1);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "TDA.GUI — Demo";
        _tabControl.ResumeLayout(false);
        _tabPageTrend.ResumeLayout(false);
        _tabPageHistogram.ResumeLayout(false);
        _tabPageProbability.ResumeLayout(false);
        _tabPageBoxPlot.ResumeLayout(false);
        _tabPageWaferMap.ResumeLayout(false);
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private TabControl _tabControl;

    private TabPage _tabPageTrend;
    private TabPage _tabPageHistogram;
    private TabPage _tabPageProbability;
    private TabPage _tabPageBoxPlot;
    private TabPage _tabPageWaferMap;

    private TrendChart _trendChart;
    private HistogramChart _histogramChart;
    private CumulativeProbabilityChart _probabilityChart;
    private BoxPlotChart _boxPlotChart;
    private WaferMapChart _waferMapChart;
    private ToolStrip toolStrip1;
    private ToolStripComboBox toolStripComboBox1;
}
