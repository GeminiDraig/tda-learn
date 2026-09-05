using TDA.FileIO;
using TDA.StandardTestData;

namespace Ex301.UseRecordReader
{
    /// <summary>
    /// 展示 RecordReader 的基本调用流程：选择文件 → 校验 → 初始化 → 读取 FAR 文件头 → 迭代 ReadNext() → 自动释放。
    /// </summary>
    public static class Ex30101_BasicWorkflow
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 选择 STDF 文件并校验 ────────────────────────
            string? filePath = TDAFileDialog.SelectFile(FileType.stdf);
            if (filePath is null || !RecordReader.IsValidStdFile(filePath))
            {
                Console.WriteLine("不是有效的 STDF 文件，程序退出。");
                return;
            }

            // ── Step 2: 初始化 RecordReader（using 保证自动释放）────
            Console.WriteLine($"1. 初始化读取器 (文件: {Path.GetFileName(filePath)})");
            using var reader = new RecordReader(filePath);

            // ── Step 3: 读取文件头（每个 STDF 文件的第一个记录必为 FAR）────
            Console.WriteLine("2. 读取文件头 (FAR):");
            if (reader.ReadNext(out Record header) && header is FAR far)
            {
                Console.WriteLine($"   CPU_TYPE = {far.CPU_TYPE}, STDF_VER = {far.STDF_VER}");
            }

            // ── Step 4: 迭代读取其余所有记录 ────────────────────────
            Console.WriteLine("3. 迭代读取记录:");
            int count = 0;
            while (reader.ReadNext(out Record record))
            {
                Console.WriteLine($"   [{++count,3}] {record.GetRecordTypeName()} (REC_LEN={record.REC_LEN})");
            }

            // ── Step 5: using 语句自动释放资源 ────────────────────────
            Console.WriteLine($"4. 自动释放资源 (using 语句)，共读取 {count} 条记录\n");
        }
    }
}
