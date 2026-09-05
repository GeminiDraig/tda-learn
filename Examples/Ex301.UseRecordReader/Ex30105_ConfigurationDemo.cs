using TDA.FileIO;
using TDA.StandardTestData;

namespace Ex301.UseRecordReader
{
    /// <summary>
    /// 演示 RecordReader 构造配置 StdfDecompressionMode（Buffered / Streaming）对压缩 STDF 文件读取行为的影响。
    /// Buffered 全量解压到内存、可随机访问；Streaming 流式边解压边解析、内存占用恒定。
    /// 对普通 .stdf 文件两种模式无差异，建议选择 .gz / .zip / .tar.gz 压缩文件观察差异。
    /// </summary>
    public static class Ex30105_ConfigurationDemo
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 选择 STDF 文件（推荐压缩格式）──────────────
            string? filePath = TDAFileDialog.SelectFile(FileType.stdf);
            if (filePath is null)
            {
                Console.WriteLine("未选择文件，程序退出。");
                return;
            }

            // ── Step 2: 定义对比配置 ────────────────────────
            var configs = new[]
            {
                new { Name = "Buffered (默认)", Mode = StdfDecompressionMode.Buffered },
                new { Name = "Streaming", Mode = StdfDecompressionMode.Streaming }
            };

            // ── Step 3: 遍历配置并分别初始化 Reader 读取 ─────────
            foreach (var config in configs)
            {
                Console.WriteLine($"配置: {config.Name}");

                // CheckAndOpenStdX：打开文件并校验首记录为 FAR（自动释放）
                using var reader = RecordReader.CheckAndOpenStdX(filePath, out _, config.Mode);

                int count = 0;
                while (reader.ReadNext(out _)) count++;

                Console.WriteLine($"   读取记录数: {count}");
                Console.WriteLine($"   可随机访问 (CanSeek): {reader.CanSeek}");
                string lengthText = reader.Length >= 0 ? $"{reader.Length} 字节" : "不可用 (流式解压)";
                Console.WriteLine($"   Length: {lengthText}\n");
            }
        }
    }
}
