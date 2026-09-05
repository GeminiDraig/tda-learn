using TDA.FileIO;
using TDA.StandardTestData;
using TDA.Utilities;

namespace Ex302.UseRecordWriter
{
    /// <summary>
    /// 场景1：将源 STDF 文件逐条 Record 拷贝为新文件。
    /// 用于验证 RecordReader + RecordWriter 的完整读写流程，
    /// 同时演示如何利用 RecordWriter.RecordCount 获取各类 Record 数量统计。
    /// </summary>
    public static class Ex30201_CopyStdfFile
    {
        /// <summary>
        /// 执行入口，由 Program.Main() 调用。
        /// </summary>
        public static void Execute()
        {
            // ── Step 1: 弹窗选取源 STDF 文件 ────────────────────────────
            string? srcPath = TDAFileDialog.SelectFile(FileType.stdf);
            if (srcPath is null)
            {
                Msg.Information("未选择文件，程序退出。");
                return;
            }

            if (!RecordReader.IsValidStdFile(srcPath))
            {
                Msg.Error($"不是有效的 STDF 文件：{srcPath}");
                return;
            }

            // ── Step 2: 构建输出文件路径 ─────────────────────────────────
            string dir      = Path.GetDirectoryName(srcPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string baseName = Path.GetFileNameWithoutExtension(srcPath);
            string ext      = Path.GetExtension(srcPath);
            string dstPath  = Path.Combine(dir, $"{baseName}.copy{ext}");

            Msg.Information($"源文件：{srcPath}");
            Msg.Information($"输出文件：{dstPath}");

            // ── Step 3: 逐条 Record 读取并写入 ──────────────────────────
            int readCount = 0;
            using (RecordReader reader = new RecordReader(srcPath))
            using (RecordWriter writer = new RecordWriter(dstPath))
            {
                while (reader.ReadNext(out Record record))
                {
                    writer.WriteRecord(record);
                    readCount++;
                }

                // ── Step 4: 输出统计摘要 ────────────────────────────────
                Msg.Information($"读取 Record 总数：{readCount}");
                Msg.Information($"写入 Record 总数：{writer.TotalRecords}");
                Msg.Information("各类型 Record 写入数量：");
                foreach (var kvp in writer.RecordCount.OrderBy(x => x.Key))
                    Msg.Information($"  {kvp.Key,-8}: {kvp.Value}");
            }

            Msg.Information($"拷贝完成 → {dstPath}");
        }
    }
}
