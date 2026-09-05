namespace Ex701.LotYieldReport
{
    /// <summary>
    /// Ex701：演示 LotYieldReport 模板 —— 从 STDF / NEDA CSV 生成良率报告 HTML。
    /// <para>
    /// 用法：
    /// <code>Ex701.LotYieldReport &lt;input.stdf&gt; [-o out.html]</code>
    /// <code>Ex701.LotYieldReport input.neda_raw.csv [-o out.html]</code>
    /// <code>Ex701.LotYieldReport --demo [-o out.html]</code>（生成演示数据并出报告）
    /// </para>
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string? input = null, output = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is "-o" or "--output")
                {
                    if (i + 1 < args.Length)
                        output = args[++i];
                }
                else
                {
                    input = args[i];
                }
            }

            Ex70101_BuildLotYieldReport.Execute(input, output);

            Console.WriteLine("Job done!");
            Console.ReadKey();
        }
    }
}
