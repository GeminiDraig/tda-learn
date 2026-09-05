using Serilog.Events;
using TDA.Utilities;

namespace Ex202.UseMsgInWinform
{
    /// <summary>
    /// Msg 消息系统在 WinForm 中的完整演示窗体。
    /// 支持六级日志输出、结构化 Tag、异常捕获、级别过滤、自定义配置等功能。
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitMsg();
        }

        /// <summary>
        /// 初始化 Msg 日志系统（默认配置）。
        /// </summary>
        private void InitMsg()
        {
            Msg.Init(new MsgConfig
            {
                MinLevel      = LogEventLevel.Debug,
                EnableConsole = true,
            });
            AppendStatus("Msg 已初始化（MinLevel=Debug, Console=On）");
        }

        /// <summary>
        /// 将消息追加到状态输出框。
        /// </summary>
        private void AppendStatus(string msg)
        {
            if (textBoxOutput.InvokeRequired)
            {
                textBoxOutput.Invoke(() => AppendStatus(msg));
                return;
            }
            textBoxOutput.AppendText($"{DateTime.Now:HH:mm:ss} [SYS] {msg}{Environment.NewLine}");
        }

        #region --- 六级日志按钮事件 ---

        private void btnVerbose_Click(object sender, EventArgs e)
        {
            Msg.Verbose(GetMessage());
            AppendStatus("Verbose 日志已输出");
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            Msg.Debug(GetMessage());
            AppendStatus("Debug 日志已输出");
        }

        private void btnInformation_Click(object sender, EventArgs e)
        {
            Msg.Information(GetMessage());
            AppendStatus("Information 日志已输出");
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            Msg.Warning(GetMessage());
            AppendStatus("Warning 日志已输出");
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            Msg.Error(GetMessage());
            AppendStatus("Error 日志已输出");
        }

        private void btnFatal_Click(object sender, EventArgs e)
        {
            Msg.Fatal(GetMessage());
            AppendStatus("Fatal 日志已输出");
        }

        #endregion

        #region --- 异常模拟按钮事件 ---

        private void btnSimulateException_Click(object sender, EventArgs e)
        {
            var msg = GetMessage();
            try
            {
                throw new InvalidOperationException($"模拟异常：{msg}");
            }
            catch (Exception ex)
            {
                Msg.Error(ex, "捕获到异常：{Message}", msg);
                AppendStatus("异常已捕获并记录为 Error 日志");
            }
        }

        private void btnSimulateFatal_Click(object sender, EventArgs e)
        {
            var msg = GetMessage();
            try
            {
                throw new ApplicationException($"模拟致命错误：{msg}");
            }
            catch (Exception ex)
            {
                Msg.Fatal(ex, "致命错误：{Message}", msg);
                AppendStatus("致命错误已捕获并记录为 Fatal 日志");
            }
        }

        #endregion

        #region --- 辅助按钮事件 ---

        private void btnBlankLine_Click(object sender, EventArgs e)
        {
            int.TryParse(textBoxBlankCount.Text, out int count);
            if (count <= 0) count = 1;
            Msg.BlankLine(count);
            AppendStatus($"已输出 {count} 个空行");
        }

        private void btnReconfigure_Click(object sender, EventArgs e)
        {
            var level = comboBoxMinLevel.SelectedItem?.ToString() ?? "Information";
            var minLevel = level switch
            {
                "Verbose"    => LogEventLevel.Verbose,
                "Debug"      => LogEventLevel.Debug,
                "Information" => LogEventLevel.Information,
                "Warning"    => LogEventLevel.Warning,
                "Error"      => LogEventLevel.Error,
                "Fatal"      => LogEventLevel.Fatal,
                _            => LogEventLevel.Information,
            };

            Msg.Init(new MsgConfig
            {
                MinLevel      = minLevel,
                EnableConsole = true,
            });
            AppendStatus($"Msg 已重新配置（MinLevel={level}）");
        }

        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            textBoxOutput.Clear();
        }

        private void btnCloseMsg_Click(object sender, EventArgs e)
        {
            Msg.Close();
            AppendStatus("Msg 已关闭，日志已刷新到磁盘");
        }

        #endregion

        #region --- 窗体关闭事件 ---

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Msg.Information("应用程序正在退出...");
            Msg.Close();
            base.OnFormClosing(e);
        }

        #endregion

        #region --- 辅助方法 ---

        /// <summary>获取用户输入的消息内容。</summary>
        private string GetMessage()
        {
            return string.IsNullOrWhiteSpace(textBoxMsg.Text)
                ? "(空消息)"
                : textBoxMsg.Text;
        }

        #endregion
    }
}
