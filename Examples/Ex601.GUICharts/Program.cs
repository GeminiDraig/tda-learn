using Ex601.GUICharts;

namespace Ex601.GUICharts;

/// <summary>
/// Ex601：演示 TDA.GUI 全部 5 种图表控件（趋势图、直方图、累积概率图、箱线图、晶圆图）
/// MainForm 提供 TestCase 选择和数据参数量选择（1/2/3）。
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
