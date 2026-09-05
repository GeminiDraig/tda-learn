namespace Ex602.GUIEasyPlotForm;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

        // 控件创建
        this._dataGridView = new DataGridView();

        ((System.ComponentModel.ISupportInitialize)this._dataGridView).BeginInit();
        this.SuspendLayout();

        // 
        // MainForm
        // 
        resources.ApplyResources(this, "$this");
        this.AutoScaleMode = AutoScaleMode.Font;
        this.Name = "MainForm";
        this.Text = "Ex602 — TDA Parameter Browser";
        this.Size = new Size(1400, 900);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(1000, 600);

        // 
        // _dataGridView
        // 
        this._dataGridView.AllowUserToAddRows = false;
        this._dataGridView.AllowUserToDeleteRows = false;
        this._dataGridView.AllowUserToResizeRows = false;
        this._dataGridView.ColumnHeadersHeight = 29;
        this._dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        this._dataGridView.Dock = DockStyle.Fill;
        this._dataGridView.Location = new Point(0, 0);
        this._dataGridView.Name = "_dataGridView";
        this._dataGridView.ReadOnly = true;
        this._dataGridView.RowHeadersVisible = false;
        this._dataGridView.RowHeadersWidth = 51;
        this._dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this._dataGridView.MultiSelect = true;
        this._dataGridView.Size = new Size(1400, 900);
        this._dataGridView.TabIndex = 0;

        // 
        // Add to form
        // 
        this.Controls.Add(this._dataGridView);

        ((System.ComponentModel.ISupportInitialize)this._dataGridView).EndInit();
        this.ResumeLayout(false);
    }

    #endregion

    private DataGridView _dataGridView;
}
