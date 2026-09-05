using TDA.FileIO;
using TDA.StandardTestData;
using TDA.Utilities;

namespace Ex301.UseRecordReader
{
    /// <summary>
    /// 演示如何使用显式 Close() 关闭 StdfReader，而非依赖 using 块自动释放。
    /// 适用于需要显式控制文件生命周期（如跨函数传递 reader）的场景。
    /// </summary>
    public static class Ex30102_HowToCloseRecordReader
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 选择 STDF 文件 ────────────────────────
            string? filePath = TDAFileDialog.SelectFile(FileType.stdf);
            if (filePath is null || !RecordReader.IsValidStdFile(filePath))
            {
                Msg.Debug("Not a valid stdf file, program exit!");
                return;
            }

            // ── Step 2: 显式创建、读取、关闭 RecordReader ────────────────────────
            RecordReader recordReader = new RecordReader(filePath);
            recordReader.ReadNext(out Record record);
            record.Print();
            recordReader.Close();
        }
    }
}
