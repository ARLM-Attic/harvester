namespace Harvester.Windows.Forms
{
  partial class Main
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
      this._splitContainer = new System.Windows.Forms.SplitContainer();
      this._messageHistory = new System.Windows.Forms.ListView();
      this._idColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._timestampColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._levelColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._processIdColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._processNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._threadColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._sourceColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._userColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._messageColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this._mainMenu = new System.Windows.Forms.MenuStrip();
      this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._autoScrollToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._colorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._messageDetails = new System.Windows.Forms.TabControl();
      this._messageDetailsTab = new System.Windows.Forms.TabPage();
      this._messageText = new System.Windows.Forms.RichTextBox();
      this._attributeDetailsTab = new System.Windows.Forms.TabPage();
      this._rawDetailsTab = new System.Windows.Forms.TabPage();
      this._exceptionDetailsTab = new System.Windows.Forms.TabPage();
      this._exceptionText = new System.Windows.Forms.RichTextBox();
      this._attributesText = new System.Windows.Forms.RichTextBox();
      this._rawText = new System.Windows.Forms.RichTextBox();
      this._splitContainer.Panel1.SuspendLayout();
      this._splitContainer.Panel2.SuspendLayout();
      this._splitContainer.SuspendLayout();
      this._mainMenu.SuspendLayout();
      this._messageDetails.SuspendLayout();
      this._messageDetailsTab.SuspendLayout();
      this._attributeDetailsTab.SuspendLayout();
      this._rawDetailsTab.SuspendLayout();
      this._exceptionDetailsTab.SuspendLayout();
      this.SuspendLayout();
      // 
      // _splitContainer
      // 
      this._splitContainer.Cursor = System.Windows.Forms.Cursors.Default;
      this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this._splitContainer.Location = new System.Drawing.Point(0, 0);
      this._splitContainer.Name = "_splitContainer";
      this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // _splitContainer.Panel1
      // 
      this._splitContainer.Panel1.Controls.Add(this._messageHistory);
      this._splitContainer.Panel1.Controls.Add(this._mainMenu);
      // 
      // _splitContainer.Panel2
      // 
      this._splitContainer.Panel2.Controls.Add(this._messageDetails);
      this._splitContainer.Size = new System.Drawing.Size(1008, 730);
      this._splitContainer.SplitterDistance = 489;
      this._splitContainer.TabIndex = 0;
      // 
      // _messageHistory
      // 
      this._messageHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._messageHistory.BackColor = System.Drawing.Color.Black;
      this._messageHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._idColumn,
            this._timestampColumn,
            this._levelColumn,
            this._processIdColumn,
            this._processNameColumn,
            this._threadColumn,
            this._sourceColumn,
            this._userColumn,
            this._messageColumn});
      this._messageHistory.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._messageHistory.ForeColor = System.Drawing.Color.Silver;
      this._messageHistory.FullRowSelect = true;
      this._messageHistory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this._messageHistory.Location = new System.Drawing.Point(12, 27);
      this._messageHistory.MultiSelect = false;
      this._messageHistory.Name = "_messageHistory";
      this._messageHistory.ShowGroups = false;
      this._messageHistory.ShowItemToolTips = true;
      this._messageHistory.Size = new System.Drawing.Size(984, 459);
      this._messageHistory.TabIndex = 1;
      this._messageHistory.UseCompatibleStateImageBehavior = false;
      this._messageHistory.View = System.Windows.Forms.View.Details;
      // 
      // _idColumn
      // 
      this._idColumn.Text = "Id";
      // 
      // _timestampColumn
      // 
      this._timestampColumn.Text = "Timestamp";
      this._timestampColumn.Width = 160;
      // 
      // _levelColumn
      // 
      this._levelColumn.Text = "Level";
      // 
      // _processIdColumn
      // 
      this._processIdColumn.Text = "PID";
      // 
      // _processNameColumn
      // 
      this._processNameColumn.Text = "Process Name";
      this._processNameColumn.Width = 120;
      // 
      // _threadColumn
      // 
      this._threadColumn.Text = "Thread";
      // 
      // _sourceColumn
      // 
      this._sourceColumn.Text = "Source";
      this._sourceColumn.Width = 120;
      // 
      // _userColumn
      // 
      this._userColumn.Text = "User";
      this._userColumn.Width = 120;
      // 
      // _messageColumn
      // 
      this._messageColumn.Text = "Message";
      // 
      // _mainMenu
      // 
      this._mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem,
            this._optionsToolStripMenuItem});
      this._mainMenu.Location = new System.Drawing.Point(0, 0);
      this._mainMenu.Name = "_mainMenu";
      this._mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this._mainMenu.Size = new System.Drawing.Size(1008, 24);
      this._mainMenu.TabIndex = 2;
      this._mainMenu.Text = "menuStrip1";
      // 
      // _fileToolStripMenuItem
      // 
      this._fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exitToolStripMenuItem});
      this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
      this._fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this._fileToolStripMenuItem.Text = "&File";
      // 
      // _exitToolStripMenuItem
      // 
      this._exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
      this._exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this._exitToolStripMenuItem.Text = "E&xit";
      // 
      // _optionsToolStripMenuItem
      // 
      this._optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._autoScrollToolStripMenuItem,
            this._colorsToolStripMenuItem});
      this._optionsToolStripMenuItem.Name = "_optionsToolStripMenuItem";
      this._optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
      this._optionsToolStripMenuItem.Text = "&Options";
      // 
      // _autoScrollToolStripMenuItem
      // 
      this._autoScrollToolStripMenuItem.Name = "_autoScrollToolStripMenuItem";
      this._autoScrollToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this._autoScrollToolStripMenuItem.Text = "&Auto Scroll";
      // 
      // _colorsToolStripMenuItem
      // 
      this._colorsToolStripMenuItem.Name = "_colorsToolStripMenuItem";
      this._colorsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this._colorsToolStripMenuItem.Text = "&Colors...";
      // 
      // _messageDetails
      // 
      this._messageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._messageDetails.Controls.Add(this._messageDetailsTab);
      this._messageDetails.Controls.Add(this._exceptionDetailsTab);
      this._messageDetails.Controls.Add(this._attributeDetailsTab);
      this._messageDetails.Controls.Add(this._rawDetailsTab);
      this._messageDetails.Location = new System.Drawing.Point(12, 3);
      this._messageDetails.Name = "_messageDetails";
      this._messageDetails.SelectedIndex = 0;
      this._messageDetails.Size = new System.Drawing.Size(984, 222);
      this._messageDetails.TabIndex = 2;
      // 
      // _messageDetailsTab
      // 
      this._messageDetailsTab.Controls.Add(this._messageText);
      this._messageDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._messageDetailsTab.Name = "_messageDetailsTab";
      this._messageDetailsTab.Padding = new System.Windows.Forms.Padding(3);
      this._messageDetailsTab.Size = new System.Drawing.Size(976, 196);
      this._messageDetailsTab.TabIndex = 0;
      this._messageDetailsTab.Text = "Message";
      this._messageDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _messageText
      // 
      this._messageText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._messageText.Location = new System.Drawing.Point(6, 6);
      this._messageText.Name = "_messageText";
      this._messageText.ReadOnly = true;
      this._messageText.Size = new System.Drawing.Size(964, 184);
      this._messageText.TabIndex = 0;
      this._messageText.Text = "";
      // 
      // _attributeDetailsTab
      // 
      this._attributeDetailsTab.Controls.Add(this._attributesText);
      this._attributeDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._attributeDetailsTab.Name = "_attributeDetailsTab";
      this._attributeDetailsTab.Padding = new System.Windows.Forms.Padding(3);
      this._attributeDetailsTab.Size = new System.Drawing.Size(976, 196);
      this._attributeDetailsTab.TabIndex = 1;
      this._attributeDetailsTab.Text = "Attributes";
      this._attributeDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _rawDetailsTab
      // 
      this._rawDetailsTab.Controls.Add(this._rawText);
      this._rawDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._rawDetailsTab.Name = "_rawDetailsTab";
      this._rawDetailsTab.Size = new System.Drawing.Size(976, 196);
      this._rawDetailsTab.TabIndex = 2;
      this._rawDetailsTab.Text = "Raw";
      this._rawDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _exceptionDetailsTab
      // 
      this._exceptionDetailsTab.Controls.Add(this._exceptionText);
      this._exceptionDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._exceptionDetailsTab.Name = "_exceptionDetailsTab";
      this._exceptionDetailsTab.Size = new System.Drawing.Size(976, 196);
      this._exceptionDetailsTab.TabIndex = 3;
      this._exceptionDetailsTab.Text = "Exception";
      this._exceptionDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _exceptionText
      // 
      this._exceptionText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._exceptionText.Location = new System.Drawing.Point(6, 6);
      this._exceptionText.Name = "_exceptionText";
      this._exceptionText.ReadOnly = true;
      this._exceptionText.Size = new System.Drawing.Size(964, 184);
      this._exceptionText.TabIndex = 1;
      this._exceptionText.Text = "";
      // 
      // _attributesText
      // 
      this._attributesText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._attributesText.Location = new System.Drawing.Point(6, 6);
      this._attributesText.Name = "_attributesText";
      this._attributesText.ReadOnly = true;
      this._attributesText.Size = new System.Drawing.Size(964, 184);
      this._attributesText.TabIndex = 1;
      this._attributesText.Text = "";
      // 
      // _rawText
      // 
      this._rawText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._rawText.Location = new System.Drawing.Point(6, 6);
      this._rawText.Name = "_rawText";
      this._rawText.ReadOnly = true;
      this._rawText.Size = new System.Drawing.Size(964, 184);
      this._rawText.TabIndex = 1;
      this._rawText.Text = "";
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 730);
      this.Controls.Add(this._splitContainer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this._mainMenu;
      this.Name = "Main";
      this.Text = "Main";
      this._splitContainer.Panel1.ResumeLayout(false);
      this._splitContainer.Panel1.PerformLayout();
      this._splitContainer.Panel2.ResumeLayout(false);
      this._splitContainer.ResumeLayout(false);
      this._mainMenu.ResumeLayout(false);
      this._mainMenu.PerformLayout();
      this._messageDetails.ResumeLayout(false);
      this._messageDetailsTab.ResumeLayout(false);
      this._attributeDetailsTab.ResumeLayout(false);
      this._rawDetailsTab.ResumeLayout(false);
      this._exceptionDetailsTab.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer _splitContainer;
    private System.Windows.Forms.ListView _messageHistory;
    private System.Windows.Forms.ColumnHeader _idColumn;
    private System.Windows.Forms.ColumnHeader _timestampColumn;
    private System.Windows.Forms.ColumnHeader _levelColumn;
    private System.Windows.Forms.ColumnHeader _processIdColumn;
    private System.Windows.Forms.ColumnHeader _processNameColumn;
    private System.Windows.Forms.ColumnHeader _threadColumn;
    private System.Windows.Forms.ColumnHeader _sourceColumn;
    private System.Windows.Forms.ColumnHeader _userColumn;
    private System.Windows.Forms.ColumnHeader _messageColumn;
    private System.Windows.Forms.TabControl _messageDetails;
    private System.Windows.Forms.TabPage _messageDetailsTab;
    private System.Windows.Forms.RichTextBox _messageText;
    private System.Windows.Forms.TabPage _attributeDetailsTab;
    private System.Windows.Forms.TabPage _rawDetailsTab;
    private System.Windows.Forms.MenuStrip _mainMenu;
    private System.Windows.Forms.ToolStripMenuItem _fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _autoScrollToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem _colorsToolStripMenuItem;
    private System.Windows.Forms.TabPage _exceptionDetailsTab;
    private System.Windows.Forms.RichTextBox _exceptionText;
    private System.Windows.Forms.RichTextBox _attributesText;
    private System.Windows.Forms.RichTextBox _rawText;
  }
}