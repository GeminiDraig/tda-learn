namespace Ex603.FlexiblePlotForm;

/// <summary>
/// Ex603：演示 FlexiblePlotForm 的按需多图布局能力。
/// MainForm 左侧列出参数 + 图表布局选择，右侧展示 FlexiblePlotForm。
/// 调用方可自由组合趋势图 / 直方图 / 累积概率图 / 箱线图，
/// FlexiblePlotForm 根据选中的图表数量自动适配布局（1图全屏、2图左右、3~4图2×2网格）。
/// </summary>
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
