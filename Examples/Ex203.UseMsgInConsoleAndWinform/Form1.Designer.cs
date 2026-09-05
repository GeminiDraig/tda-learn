namespace Ex203.UseMsgInConsoleAndWinform
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

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            toolStrip1 = new ToolStrip();
            toolStripButtonDebug = new ToolStripButton();
            toolStripButtonInfo = new ToolStripButton();
            toolStripButtonWarning = new ToolStripButton();
            toolStripButtonError = new ToolStripButton();
            textBoxMsg = new TextBox();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButtonDebug, toolStripButtonInfo, toolStripButtonWarning, toolStripButtonError });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(531, 27);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonDebug
            // 
            toolStripButtonDebug.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonDebug.Image = (Image)resources.GetObject("toolStripButtonDebug.Image");
            toolStripButtonDebug.ImageTransparentColor = Color.Magenta;
            toolStripButtonDebug.Name = "toolStripButtonDebug";
            toolStripButtonDebug.Size = new Size(70, 24);
            toolStripButtonDebug.Text = " Debug ";
            toolStripButtonDebug.Click += toolStripButtonDebug_Click;
            // 
            // toolStripButtonInfo
            // 
            toolStripButtonInfo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonInfo.Image = (Image)resources.GetObject("toolStripButtonInfo.Image");
            toolStripButtonInfo.ImageTransparentColor = Color.Magenta;
            toolStripButtonInfo.Name = "toolStripButtonInfo";
            toolStripButtonInfo.Size = new Size(49, 24);
            toolStripButtonInfo.Text = " Info ";
            toolStripButtonInfo.Click += toolStripButtonInfo_Click;
            // 
            // toolStripButtonWarning
            // 
            toolStripButtonWarning.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonWarning.Image = (Image)resources.GetObject("toolStripButtonWarning.Image");
            toolStripButtonWarning.ImageTransparentColor = Color.Magenta;
            toolStripButtonWarning.Name = "toolStripButtonWarning";
            toolStripButtonWarning.Size = new Size(82, 24);
            toolStripButtonWarning.Text = " Warning ";
            toolStripButtonWarning.Click += toolStripButtonWarning_Click;
            // 
            // toolStripButtonError
            // 
            toolStripButtonError.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonError.Image = (Image)resources.GetObject("toolStripButtonError.Image");
            toolStripButtonError.ImageTransparentColor = Color.Magenta;
            toolStripButtonError.Name = "toolStripButtonError";
            toolStripButtonError.Size = new Size(57, 24);
            toolStripButtonError.Text = " Error ";
            toolStripButtonError.Click += toolStripButtonError_Click;
            // 
            // textBoxMsg
            // 
            textBoxMsg.Dock = DockStyle.Fill;
            textBoxMsg.Location = new Point(0, 27);
            textBoxMsg.Multiline = true;
            textBoxMsg.Name = "textBoxMsg";
            textBoxMsg.Size = new Size(531, 92);
            textBoxMsg.TabIndex = 2;
            textBoxMsg.Text = "Message to be output";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(531, 119);
            Controls.Add(textBoxMsg);
            Controls.Add(toolStrip1);
            Name = "Form1";
            Text = "Form1";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonDebug;
        private ToolStripButton toolStripButtonInfo;
        private ToolStripButton toolStripButtonWarning;
        private ToolStripButton toolStripButtonError;
        private TextBox textBoxMsg;
    }
}
