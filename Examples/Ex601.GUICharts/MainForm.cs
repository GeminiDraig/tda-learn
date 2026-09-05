using TDA.GUI.Utilities;

namespace Ex601.GUICharts;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        // Set default selection (must be after InitializeComponent to avoid premature events)
        toolStripComboBox1.SelectedIndex = 1;

        // Load initial data
        RefreshAllCharts();
    }

    private void OnSelectionChanged(object? sender, EventArgs e)
    {
        RefreshAllCharts();
    }

    private void RefreshAllCharts()
    {
        int seriesCount = (int)toolStripComboBox1.SelectedItem!;

        _trendChart.SetData(SampleDataGenerator.GenerateTrendData(seriesCount));
        _histogramChart.SetData(SampleDataGenerator.GenerateHistogramData(seriesCount));
        _probabilityChart.SetData(SampleDataGenerator.GenerateProbabilityData(seriesCount));
        _boxPlotChart.SetData(SampleDataGenerator.GenerateBoxPlotData(seriesCount));
        _waferMapChart.SetData(SampleDataGenerator.GenerateWaferMapData());
    }
}
