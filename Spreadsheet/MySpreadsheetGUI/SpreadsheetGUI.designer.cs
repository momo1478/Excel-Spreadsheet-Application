namespace SSGui
{
    partial class SpreadsheetGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cellName = new System.Windows.Forms.Label();
            this.cellContents = new System.Windows.Forms.Label();
            this.cellValue = new System.Windows.Forms.Label();
            this.cellNameBox = new System.Windows.Forms.TextBox();
            this.cellValueBox = new System.Windows.Forms.TextBox();
            this.cellContentsBox = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel1 = new SSGui.SpreadsheetPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // cellName
            // 
            this.cellName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellName.AutoSize = true;
            this.cellName.Location = new System.Drawing.Point(12, 381);
            this.cellName.Name = "cellName";
            this.cellName.Size = new System.Drawing.Size(55, 13);
            this.cellName.TabIndex = 2;
            this.cellName.Text = "Cell Name";
            // 
            // cellContents
            // 
            this.cellContents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellContents.AutoSize = true;
            this.cellContents.Location = new System.Drawing.Point(12, 418);
            this.cellContents.Name = "cellContents";
            this.cellContents.Size = new System.Drawing.Size(69, 13);
            this.cellContents.TabIndex = 3;
            this.cellContents.Text = "Cell Contents";
            // 
            // cellValue
            // 
            this.cellValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellValue.AutoSize = true;
            this.cellValue.Location = new System.Drawing.Point(141, 381);
            this.cellValue.Name = "cellValue";
            this.cellValue.Size = new System.Drawing.Size(54, 13);
            this.cellValue.TabIndex = 4;
            this.cellValue.Text = "Cell Value";
            // 
            // cellNameBox
            // 
            this.cellNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellNameBox.Enabled = false;
            this.cellNameBox.Location = new System.Drawing.Point(15, 395);
            this.cellNameBox.Name = "cellNameBox";
            this.cellNameBox.ReadOnly = true;
            this.cellNameBox.Size = new System.Drawing.Size(100, 20);
            this.cellNameBox.TabIndex = 5;
            // 
            // cellValueBox
            // 
            this.cellValueBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellValueBox.Enabled = false;
            this.cellValueBox.Location = new System.Drawing.Point(144, 395);
            this.cellValueBox.Name = "cellValueBox";
            this.cellValueBox.ReadOnly = true;
            this.cellValueBox.Size = new System.Drawing.Size(100, 20);
            this.cellValueBox.TabIndex = 6;
            // 
            // cellContentsBox
            // 
            this.cellContentsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellContentsBox.Location = new System.Drawing.Point(15, 435);
            this.cellContentsBox.Name = "cellContentsBox";
            this.cellContentsBox.Size = new System.Drawing.Size(819, 20);
            this.cellContentsBox.TabIndex = 7;
            this.cellContentsBox.TextChanged += new System.EventHandler(this.cellContentsBox_TextChanged);
            this.cellContentsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cellContentsBox_KeyDown);
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 27);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(884, 351);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(884, 473);
            this.Controls.Add(this.cellContentsBox);
            this.Controls.Add(this.cellValueBox);
            this.Controls.Add(this.cellNameBox);
            this.Controls.Add(this.cellValue);
            this.Controls.Add(this.cellContents);
            this.Controls.Add(this.cellName);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetGUI";
            this.Text = "SpreadsheetGUI";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Label cellName;
        private System.Windows.Forms.Label cellContents;
        private System.Windows.Forms.Label cellValue;
        private System.Windows.Forms.TextBox cellNameBox;
        private System.Windows.Forms.TextBox cellValueBox;
        private System.Windows.Forms.TextBox cellContentsBox;
    }
}

