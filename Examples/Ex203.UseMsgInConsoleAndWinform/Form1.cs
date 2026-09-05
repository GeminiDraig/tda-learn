using TDA.Utilities;

namespace Ex203.UseMsgInConsoleAndWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButtonDebug_Click(object sender, EventArgs e)
        {
            Msg.Debug(textBoxMsg.Text);
        }

        private void toolStripButtonInfo_Click(object sender, EventArgs e)
        {
            Msg.Information(textBoxMsg.Text);
        }

        private void toolStripButtonWarning_Click(object sender, EventArgs e)
        {
            Msg.Warning(textBoxMsg.Text);
        }

        private void toolStripButtonError_Click(object sender, EventArgs e)
        {
            Msg.Error(textBoxMsg.Text);
        }
    }
}
