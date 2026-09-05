using TDA;
using TDA.FileIO;
using TDA.Utilities;

namespace Ex106.ExportPartSummary
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Ex10601_ExportPartSummary();
            Console.WriteLine("Job Done!");
        }

        #region Ex10601 - 导出 Part/Device 信息列表到 CSV
        /// <summary>
        /// 导出 Part/Device 信息列表到 CSV 文件。
        /// 每颗芯片包含 Pass/Fail、HardBin/SoftBin、坐标、数据来源等字段。
        /// </summary>
        static void Ex10601_ExportPartSummary()
        {
            ExportPartSummary.Test();
        }
        #endregion
    }
}
