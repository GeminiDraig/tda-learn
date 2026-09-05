namespace Ex602.GUIEasyPlotForm;

/// <summary>
/// Ex602：演示 MainForm + EasyPlotForm 联动使用方式。
/// MainForm 以 DataGridView 列出所有参数及其统计数据，
/// 用户可多选行，选中的参数自动绘制到内嵌的 EasyPlotForm 中。
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
