using TDA.FileIO;
using TDA.StandardTestData;
using TDA.Utilities;

namespace Ex301.UseRecordReader
{
    /// <summary>
    /// 使用 StdfReader 逐条读取 STDF 文件，通过 TypeID 将基类 Record 转换为具体子类型后打印。
    /// 演示如何区分不同记录类型以获取各类型的专属数据。
    /// </summary>
    public static class Ex30104_ReadRecordAsSpecificRecordType
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

            // ── Step 2: 逐条读取，按 TypeID 转换为具体类型并打印 ────────────────────────
            using (RecordReader recordReader = new RecordReader(filePath))
            {
                int i = 0;
                while (recordReader.ReadNext(out Record record))
                {
                    switch (record.TypeID)
                    {
                        case RecordType.RID_FAR: FAR far = (FAR)record; far.Print(); break;
                        case RecordType.RID_ATR: ATR atr = (ATR)record; atr.Print(); break;
                        case RecordType.RID_VUR: VUR vur = (VUR)record; vur.Print(); break;
                        case RecordType.RID_MIR: MIR mir = (MIR)record; mir.Print(); break;
                        case RecordType.RID_RDR: RDR rdr = (RDR)record; rdr.Print(); break;
                        case RecordType.RID_SDR: SDR sdr = (SDR)record; sdr.Print(); break;
                        case RecordType.RID_GDR: GDR gdr = (GDR)record; gdr.Print(); break;
                        case RecordType.RID_PMR: PMR pmr = (PMR)record; pmr.Print(); break;
                        case RecordType.RID_PGR: PGR pgr = (PGR)record; pgr.Print(); break;
                        case RecordType.RID_PLR: PLR plr = (PLR)record; plr.Print(); break;
                        case RecordType.RID_WCR: WCR wcr = (WCR)record; wcr.Print(); break;
                        case RecordType.RID_WIR: WIR wir = (WIR)record; wir.Print(); break;
                        case RecordType.RID_PIR: PIR pir = (PIR)record; pir.Print(); break;
                        case RecordType.RID_BPS: BPS bps = (BPS)record; bps.Print(); break;
                        case RecordType.RID_PTR: PTR ptr = (PTR)record; ptr.Print(); break;
                        case RecordType.RID_MPR: MPR mpr = (MPR)record; mpr.Print(); break;
                        case RecordType.RID_FTR: FTR ftr = (FTR)record; ftr.Print(); break;
                        case RecordType.RID_EPS: EPS eps = (EPS)record; eps.Print(); break;
                        case RecordType.RID_PRR: PRR prr = (PRR)record; prr.Print(); break;
                        case RecordType.RID_WRR: WRR wrr = (WRR)record; wrr.Print(); break;
                        case RecordType.RID_HBR: HBR hbr = (HBR)record; hbr.Print(); break;
                        case RecordType.RID_SBR: SBR sbr = (SBR)record; sbr.Print(); break;
                        case RecordType.RID_TSR: TSR tsr = (TSR)record; tsr.Print(); break;
                        case RecordType.RID_PCR: PCR pcr = (PCR)record; pcr.Print(); break;
                        case RecordType.RID_MRR: MRR mrr = (MRR)record; mrr.Print(); break;
                        case RecordType.RID_DTR: DTR dtr = (DTR)record; dtr.Print(); break;
                        default: record.Print(); break;
                    }
                    i++;
                }
            }
        }
    }
}
