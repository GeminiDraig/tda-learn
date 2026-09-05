using TDA.FileIO;
using TDA.StandardTestData;

namespace Ex301.UseRecordReader
{
    /// <summary>
    /// 使用 StdfReader 逐条读取 STDF 文件，将每条记录作为基类 Record 打印。
    /// 演示最基本的逐条读取用法，不区分具体记录类型。
    /// </summary>
    public static class Ex30103_ReadRecordsAsGeneralRecordType
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
                System.Console.WriteLine("Not a valid stdf file, program exit!");
                return;
            }

            // ── Step 2: 逐条读取并打印（基类 Record） ────────────────────────
            using (RecordReader recordReader = new RecordReader(filePath))
            {
                while (recordReader.ReadNext(out Record record))
                {
                    // 对于大型量产 STDF 文件，此循环可能耗时较长
                    record.Print();
                }
            }
        }
    }
}
