namespace Ex202.UseMsgInWinform
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            groupBoxLogLevels = new GroupBox();
            btnFatal = new Button();
            btnError = new Button();
            btnWarning = new Button();
            btnInformation = new Button();
            btnDebug = new Button();
            btnVerbose = new Button();
            groupBoxMessage = new GroupBox();
            labelTag = new Label();
            textBoxTag = new TextBox();
            labelMsg = new Label();
            textBoxMsg = new TextBox();
            groupBoxException = new GroupBox();
            btnSimulateFatal = new Button();
            btnSimulateException = new Button();
            groupBoxAuxiliary = new GroupBox();
            btnCloseMsg = new Button();
            btnClearOutput = new Button();
            btnReconfigure = new Button();
            comboBoxMinLevel = new ComboBox();
            labelMinLevel = new Label();
            btnBlankLine = new Button();
            textBoxBlankCount = new TextBox();
            labelBlankCount = new Label();
            groupBoxOutput = new GroupBox();
            textBoxOutput = new TextBox();
            groupBoxLogLevels.SuspendLayout();
            groupBoxMessage.SuspendLayout();
            groupBoxException.SuspendLayout();
            groupBoxAuxiliary.SuspendLayout();
            groupBoxOutput.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxLogLevels
            // 
            groupBoxLogLevels.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxLogLevels.Controls.Add(btnFatal);
            groupBoxLogLevels.Controls.Add(btnError);
            groupBoxLogLevels.Controls.Add(btnWarning);
            groupBoxLogLevels.Controls.Add(btnInformation);
            groupBoxLogLevels.Controls.Add(btnDebug);
            groupBoxLogLevels.Controls.Add(btnVerbose);
            groupBoxLogLevels.Location = new Point(12, 12);
            groupBoxLogLevels.Name = "groupBoxLogLevels";
            groupBoxLogLevels.Size = new Size(780, 60);
            groupBoxLogLevels.TabIndex = 0;
            groupBoxLogLevels.TabStop = false;
            groupBoxLogLevels.Text = "六级日志（点击按钮输出对应级别的日志）";
            // 
            // btnFatal
            // 
            btnFatal.BackColor = Color.DarkRed;
            btnFatal.ForeColor = Color.White;
            btnFatal.Location = new Point(655, 22);
            btnFatal.Name = "btnFatal";
            btnFatal.Size = new Size(115, 28);
            btnFatal.TabIndex = 5;
            btnFatal.Text = "Fatal";
            btnFatal.UseVisualStyleBackColor = false;
            btnFatal.Click += btnFatal_Click;
            // 
            // btnError
            // 
            btnError.BackColor = Color.IndianRed;
            btnError.ForeColor = Color.White;
            btnError.Location = new Point(530, 22);
            btnError.Name = "btnError";
            btnError.Size = new Size(115, 28);
            btnError.TabIndex = 4;
            btnError.Text = "Error";
            btnError.UseVisualStyleBackColor = false;
            btnError.Click += btnError_Click;
            // 
            // btnWarning
            // 
            btnWarning.BackColor = Color.Goldenrod;
            btnWarning.ForeColor = Color.White;
            btnWarning.Location = new Point(405, 22);
            btnWarning.Name = "btnWarning";
            btnWarning.Size = new Size(115, 28);
            btnWarning.TabIndex = 3;
            btnWarning.Text = "Warning";
            btnWarning.UseVisualStyleBackColor = false;
            btnWarning.Click += btnWarning_Click;
            // 
            // btnInformation
            // 
            btnInformation.BackColor = Color.SteelBlue;
            btnInformation.ForeColor = Color.White;
            btnInformation.Location = new Point(280, 22);
            btnInformation.Name = "btnInformation";
            btnInformation.Size = new Size(115, 28);
            btnInformation.TabIndex = 2;
            btnInformation.Text = "Information";
            btnInformation.UseVisualStyleBackColor = false;
            btnInformation.Click += btnInformation_Click;
            // 
            // btnDebug
            // 
            btnDebug.BackColor = Color.Gray;
            btnDebug.ForeColor = Color.White;
            btnDebug.Location = new Point(155, 22);
            btnDebug.Name = "btnDebug";
            btnDebug.Size = new Size(115, 28);
            btnDebug.TabIndex = 1;
            btnDebug.Text = "Debug";
            btnDebug.UseVisualStyleBackColor = false;
            btnDebug.Click += btnDebug_Click;
            // 
            // btnVerbose
            // 
            btnVerbose.BackColor = Color.DarkGray;
            btnVerbose.ForeColor = Color.White;
            btnVerbose.Location = new Point(30, 22);
            btnVerbose.Name = "btnVerbose";
            btnVerbose.Size = new Size(115, 28);
            btnVerbose.TabIndex = 0;
            btnVerbose.Text = "Verbose";
            btnVerbose.UseVisualStyleBackColor = false;
            btnVerbose.Click += btnVerbose_Click;
            // 
            // groupBoxMessage
            // 
            groupBoxMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMessage.Controls.Add(labelTag);
            groupBoxMessage.Controls.Add(textBoxTag);
            groupBoxMessage.Controls.Add(labelMsg);
            groupBoxMessage.Controls.Add(textBoxMsg);
            groupBoxMessage.Location = new Point(12, 78);
            groupBoxMessage.Name = "groupBoxMessage";
            groupBoxMessage.Size = new Size(780, 80);
            groupBoxMessage.TabIndex = 1;
            groupBoxMessage.TabStop = false;
            groupBoxMessage.Text = "消息与 Tag";
            // 
            // labelTag
            // 
            labelTag.AutoSize = true;
            labelTag.Location = new Point(30, 50);
            labelTag.Name = "labelTag";
            labelTag.Size = new Size(41, 20);
            labelTag.TabIndex = 3;
            labelTag.Text = "Tag：";
            // 
            // textBoxTag
            // 
            textBoxTag.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTag.Location = new Point(77, 47);
            textBoxTag.Name = "textBoxTag";
            textBoxTag.PlaceholderText = "可选，如 Auth、TDA、Order（留空则不带 Tag）";
            textBoxTag.Size = new Size(693, 27);
            textBoxTag.TabIndex = 2;
            // 
            // labelMsg
            // 
            labelMsg.AutoSize = true;
            labelMsg.Location = new Point(6, 23);
            labelMsg.Name = "labelMsg";
            labelMsg.Size = new Size(65, 20);
            labelMsg.TabIndex = 1;
            labelMsg.Text = "消息内容：";
            // 
            // textBoxMsg
            // 
            textBoxMsg.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxMsg.Location = new Point(77, 20);
            textBoxMsg.Name = "textBoxMsg";
            textBoxMsg.Size = new Size(693, 27);
            textBoxMsg.TabIndex = 0;
            textBoxMsg.Text = "这是一条测试消息";
            // 
            // groupBoxException
            // 
            groupBoxException.Controls.Add(btnSimulateFatal);
            groupBoxException.Controls.Add(btnSimulateException);
            groupBoxException.Location = new Point(12, 164);
            groupBoxException.Name = "groupBoxException";
            groupBoxException.Size = new Size(380, 60);
            groupBoxException.TabIndex = 2;
            groupBoxException.TabStop = false;
            groupBoxException.Text = "异常模拟（点击按钮模拟异常并自动捕获记录）";
            // 
            // btnSimulateFatal
            // 
            btnSimulateFatal.BackColor = Color.DarkRed;
            btnSimulateFatal.ForeColor = Color.White;
            btnSimulateFatal.Location = new Point(195, 22);
            btnSimulateFatal.Name = "btnSimulateFatal";
            btnSimulateFatal.Size = new Size(170, 28);
            btnSimulateFatal.TabIndex = 1;
            btnSimulateFatal.Text = "模拟 Fatal 异常";
            btnSimulateFatal.UseVisualStyleBackColor = false;
            btnSimulateFatal.Click += btnSimulateFatal_Click;
            // 
            // btnSimulateException
            // 
            btnSimulateException.BackColor = Color.IndianRed;
            btnSimulateException.ForeColor = Color.White;
            btnSimulateException.Location = new Point(15, 22);
            btnSimulateException.Name = "btnSimulateException";
            btnSimulateException.Size = new Size(170, 28);
            btnSimulateException.TabIndex = 0;
            btnSimulateException.Text = "模拟 Error 异常";
            btnSimulateException.UseVisualStyleBackColor = false;
            btnSimulateException.Click += btnSimulateException_Click;
            // 
            // groupBoxAuxiliary
            // 
            groupBoxAuxiliary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxAuxiliary.Controls.Add(btnCloseMsg);
            groupBoxAuxiliary.Controls.Add(btnClearOutput);
            groupBoxAuxiliary.Controls.Add(btnReconfigure);
            groupBoxAuxiliary.Controls.Add(comboBoxMinLevel);
            groupBoxAuxiliary.Controls.Add(labelMinLevel);
            groupBoxAuxiliary.Controls.Add(btnBlankLine);
            groupBoxAuxiliary.Controls.Add(textBoxBlankCount);
            groupBoxAuxiliary.Controls.Add(labelBlankCount);
            groupBoxAuxiliary.Location = new Point(398, 164);
            groupBoxAuxiliary.Name = "groupBoxAuxiliary";
            groupBoxAuxiliary.Size = new Size(394, 60);
            groupBoxAuxiliary.TabIndex = 3;
            groupBoxAuxiliary.TabStop = false;
            groupBoxAuxiliary.Text = "辅助功能";
            // 
            // btnCloseMsg
            // 
            btnCloseMsg.Location = new Point(300, 22);
            btnCloseMsg.Name = "btnCloseMsg";
            btnCloseMsg.Size = new Size(85, 28);
            btnCloseMsg.TabIndex = 7;
            btnCloseMsg.Text = "关闭 Msg";
            btnCloseMsg.UseVisualStyleBackColor = true;
            btnCloseMsg.Click += btnCloseMsg_Click;
            // 
            // btnClearOutput
            // 
            btnClearOutput.Location = new Point(220, 22);
            btnClearOutput.Name = "btnClearOutput";
            btnClearOutput.Size = new Size(75, 28);
            btnClearOutput.TabIndex = 6;
            btnClearOutput.Text = "清屏";
            btnClearOutput.UseVisualStyleBackColor = true;
            btnClearOutput.Click += btnClearOutput_Click;
            // 
            // btnReconfigure
            // 
            btnReconfigure.Location = new Point(155, 22);
            btnReconfigure.Name = "btnReconfigure";
            btnReconfigure.Size = new Size(55, 28);
            btnReconfigure.TabIndex = 5;
            btnReconfigure.Text = "应用";
            btnReconfigure.UseVisualStyleBackColor = true;
            btnReconfigure.Click += btnReconfigure_Click;
            // 
            // comboBoxMinLevel
            // 
            comboBoxMinLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMinLevel.FormattingEnabled = true;
            comboBoxMinLevel.Items.AddRange(new object[] { "Verbose", "Debug", "Information", "Warning", "Error", "Fatal" });
            comboBoxMinLevel.Location = new Point(95, 23);
            comboBoxMinLevel.Name = "comboBoxMinLevel";
            comboBoxMinLevel.Size = new Size(55, 28);
            comboBoxMinLevel.TabIndex = 4;
            // 
            // labelMinLevel
            // 
            labelMinLevel.AutoSize = true;
            labelMinLevel.Location = new Point(6, 26);
            labelMinLevel.Name = "labelMinLevel";
            labelMinLevel.Size = new Size(83, 20);
            labelMinLevel.TabIndex = 3;
            labelMinLevel.Text = "最低级别：";
            // 
            // btnBlankLine
            // 
            btnBlankLine.Location = new Point(325, 22);
            btnBlankLine.Name = "btnBlankLine";
            btnBlankLine.Size = new Size(60, 28);
            btnBlankLine.TabIndex = 2;
            btnBlankLine.Text = "空行";
            btnBlankLine.UseVisualStyleBackColor = true;
            btnBlankLine.Visible = false;
            btnBlankLine.Click += btnBlankLine_Click;
            // 
            // textBoxBlankCount
            // 
            textBoxBlankCount.Location = new Point(76, 22);
            textBoxBlankCount.Name = "textBoxBlankCount";
            textBoxBlankCount.Size = new Size(30, 27);
            textBoxBlankCount.TabIndex = 1;
            textBoxBlankCount.Text = "1";
            textBoxBlankCount.TextAlign = HorizontalAlignment.Center;
            textBoxBlankCount.Visible = false;
            // 
            // labelBlankCount
            // 
            labelBlankCount.AutoSize = true;
            labelBlankCount.Location = new Point(6, 25);
            labelBlankCount.Name = "labelBlankCount";
            labelBlankCount.Size = new Size(69, 20);
            labelBlankCount.TabIndex = 0;
            labelBlankCount.Text = "空行数量：";
            labelBlankCount.Visible = false;
            // 
            // groupBoxOutput
            // 
            groupBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxOutput.Controls.Add(textBoxOutput);
            groupBoxOutput.Location = new Point(12, 230);
            groupBoxOutput.Name = "groupBoxOutput";
            groupBoxOutput.Size = new Size(780, 219);
            groupBoxOutput.TabIndex = 4;
            groupBoxOutput.TabStop = false;
            groupBoxOutput.Text = "运行状态（日志实际输出到控制台和文件，此处显示操作反馈）";
            // 
            // textBoxOutput
            // 
            textBoxOutput.BackColor = Color.FromArgb(30, 30, 30);
            textBoxOutput.Dock = DockStyle.Fill;
            textBoxOutput.ForeColor = Color.Lime;
            textBoxOutput.Location = new Point(3, 23);
            textBoxOutput.Multiline = true;
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.ReadOnly = true;
            textBoxOutput.ScrollBars = ScrollBars.Vertical;
            textBoxOutput.Size = new Size(774, 193);
            textBoxOutput.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 461);
            Controls.Add(groupBoxOutput);
            Controls.Add(groupBoxAuxiliary);
            Controls.Add(groupBoxException);
            Controls.Add(groupBoxMessage);
            Controls.Add(groupBoxLogLevels);
            MinimumSize = new Size(820, 500);
            Name = "Form1";
            Text = "Ex202 — Msg 在 WinForm 中的使用演示";
            groupBoxLogLevels.ResumeLayout(false);
            groupBoxMessage.ResumeLayout(false);
            groupBoxMessage.PerformLayout();
            groupBoxException.ResumeLayout(false);
            groupBoxAuxiliary.ResumeLayout(false);
            groupBoxAuxiliary.PerformLayout();
            groupBoxOutput.ResumeLayout(false);
            groupBoxOutput.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxLogLevels;
        private Button btnVerbose;
        private Button btnDebug;
        private Button btnInformation;
        private Button btnWarning;
        private Button btnError;
        private Button btnFatal;
        private GroupBox groupBoxMessage;
        private Label labelMsg;
        private TextBox textBoxMsg;
        private Label labelTag;
        private TextBox textBoxTag;
        private GroupBox groupBoxException;
        private Button btnSimulateException;
        private Button btnSimulateFatal;
        private GroupBox groupBoxAuxiliary;
        private Button btnBlankLine;
        private TextBox textBoxBlankCount;
        private Label labelBlankCount;
        private ComboBox comboBoxMinLevel;
        private Label labelMinLevel;
        private Button btnReconfigure;
        private Button btnClearOutput;
        private Button btnCloseMsg;
        private GroupBox groupBoxOutput;
        private TextBox textBoxOutput;
    }
}
