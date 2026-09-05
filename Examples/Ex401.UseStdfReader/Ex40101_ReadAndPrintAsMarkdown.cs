using TDA.FileIO;
using TDA.StandardTestData;
using TDA.Utilities;

namespace Ex401.UseStdfReader
{
    /// <summary>
    /// 使用 StdfReader 逐条读取 STDF 文件，按记录类型解析，调用 ToBytesCheck 验证序列化/反序列化一致性。
    /// </summary>
    public static class Ex40101_ReadAndPrintAsMarkdown
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

            // ── Step 2: 逐条读取记录，按类型分支校验字节流 ────────────────────────
            Msg.Debug(filePath);
            using (RecordReader recordReader = new RecordReader(filePath))
            {
                int i = 0;
                while (recordReader.ReadNext(out Record record))
                {
                    switch (record.TypeID)
                    {
                        case RecordType.RID_FAR: FAR far = (FAR)record; far.ToBytesCheck(); break;
                        case RecordType.RID_ATR: ATR atr = (ATR)record; atr.ToBytesCheck(); break;
                        case RecordType.RID_VUR: VUR vur = (VUR)record; vur.ToBytesCheck(); break;
                        case RecordType.RID_MIR: MIR mir = (MIR)record; mir.ToBytesCheck(); break;
                        case RecordType.RID_RDR: RDR rdr = (RDR)record; rdr.ToBytesCheck(); break;
                        case RecordType.RID_SDR: SDR sdr = (SDR)record; sdr.ToBytesCheck(); break;
                        case RecordType.RID_GDR: GDR gdr = (GDR)record; gdr.ToBytesCheck(); break;
                        case RecordType.RID_PMR: PMR pmr = (PMR)record; pmr.ToBytesCheck(); break;
                        case RecordType.RID_PGR: PGR pgr = (PGR)record; pgr.ToBytesCheck(); break;
                        case RecordType.RID_PLR: PLR plr = (PLR)record; plr.ToBytesCheck(); break;
                        case RecordType.RID_WCR: WCR wcr = (WCR)record; wcr.ToBytesCheck(); break;
                        case RecordType.RID_WIR: WIR wir = (WIR)record; wir.ToBytesCheck(); break;
                        case RecordType.RID_PIR: PIR pir = (PIR)record; pir.ToBytesCheck(); break;
                        case RecordType.RID_BPS: BPS bps = (BPS)record; bps.ToBytesCheck(); break;
                        case RecordType.RID_PTR: PTR ptr = (PTR)record; ptr.ToBytesCheck(); break;
                        case RecordType.RID_MPR: MPR mpr = (MPR)record; mpr.ToBytesCheck(); break;
                        case RecordType.RID_FTR: FTR ftr = (FTR)record; ftr.ToBytesCheck(); break;
                        case RecordType.RID_EPS: EPS eps = (EPS)record; eps.ToBytesCheck(); break;
                        case RecordType.RID_PRR: PRR prr = (PRR)record; prr.ToBytesCheck(); break;
                        case RecordType.RID_WRR: WRR wrr = (WRR)record; wrr.ToBytesCheck(); break;
                        case RecordType.RID_HBR: HBR hbr = (HBR)record; hbr.ToBytesCheck(); break;
                        case RecordType.RID_SBR: SBR sbr = (SBR)record; sbr.ToBytesCheck(); break;
                        case RecordType.RID_TSR: TSR tsr = (TSR)record; tsr.ToBytesCheck(); break;
                        case RecordType.RID_PCR: PCR pcr = (PCR)record; pcr.ToBytesCheck(); break;
                        case RecordType.RID_MRR: MRR mrr = (MRR)record; mrr.ToBytesCheck(); break;
                        case RecordType.RID_DTR: DTR dtr = (DTR)record; dtr.ToBytesCheck(); break;
                        default: break;
                    }
                    i++;
                }
            }
        }
    }
}
