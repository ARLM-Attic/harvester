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
      this._mainToolStrip = new System.Windows.Forms.ToolStrip();
      this._exitButton = new System.Windows.Forms.ToolStripButton();
      this._separator1 = new System.Windows.Forms.ToolStripSeparator();
      this._clearHistoryButton = new System.Windows.Forms.ToolStripButton();
      this._colorPickerButton = new System.Windows.Forms.ToolStripButton();
      this._scrollResumeButton = new System.Windows.Forms.ToolStripButton();
      this._separator2 = new System.Windows.Forms.ToolStripSeparator();
      this._filterLevelButton = new System.Windows.Forms.ToolStripButton();
      this._filterProcessButton = new System.Windows.Forms.ToolStripButton();
      this._filterApplicationButton = new System.Windows.Forms.ToolStripButton();
      this._filterSourceButton = new System.Windows.Forms.ToolStripButton();
      this._filterUserButton = new System.Windows.Forms.ToolStripButton();
      this._filterMessageButton = new System.Windows.Forms.ToolStripButton();
      this._separator3 = new System.Windows.Forms.ToolStripSeparator();
      this._searchButton = new System.Windows.Forms.ToolStripButton();
      this._searchText = new System.Windows.Forms.ToolStripTextBox();
      this._searchPrevButton = new System.Windows.Forms.ToolStripButton();
      this._searchNextButton = new System.Windows.Forms.ToolStripButton();
      this._selectedMessageDetails = new System.Windows.Forms.TabControl();
      this._messageDetailsTab = new System.Windows.Forms.TabPage();
      this._selectedMessageDetailsLayout = new System.Windows.Forms.TableLayoutPanel();
      this._selectedMessageLevel = new System.Windows.Forms.TextBox();
      this._selectedMessageUsernameHeader = new System.Windows.Forms.Label();
      this._selectedMessageThreadHeader = new System.Windows.Forms.Label();
      this._selectedMessageSourceHeader = new System.Windows.Forms.Label();
      this._selectedMessageLevelHeader = new System.Windows.Forms.Label();
      this._selectedMessageIdHeader = new System.Windows.Forms.Label();
      this._selectedMessageId = new System.Windows.Forms.TextBox();
      this._selectedMessageTimestamp = new System.Windows.Forms.TextBox();
      this._selectedMessageTimestampHeader = new System.Windows.Forms.Label();
      this._selectedMessageText = new System.Windows.Forms.RichTextBox();
      this._selectedMessageTextHeader = new System.Windows.Forms.Label();
      this._selectedMessageProcessHeader = new System.Windows.Forms.Label();
      this._selectedMessageSource = new System.Windows.Forms.TextBox();
      this._selectedMessageProcess = new System.Windows.Forms.TextBox();
      this._selectedMessageThread = new System.Windows.Forms.TextBox();
      this._selectedMessageUsername = new System.Windows.Forms.TextBox();
      this._attributeDetailsTab = new System.Windows.Forms.TabPage();
      this._attributesText = new System.Windows.Forms.RichTextBox();
      this._rawDetailsTab = new System.Windows.Forms.TabPage();
      this._rawText = new System.Windows.Forms.RichTextBox();
      this._splitContainer.Panel1.SuspendLayout();
      this._splitContainer.Panel2.SuspendLayout();
      this._splitContainer.SuspendLayout();
      this._mainToolStrip.SuspendLayout();
      this._selectedMessageDetails.SuspendLayout();
      this._messageDetailsTab.SuspendLayout();
      this._selectedMessageDetailsLayout.SuspendLayout();
      this._attributeDetailsTab.SuspendLayout();
      this._rawDetailsTab.SuspendLayout();
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
      this._splitContainer.Panel1.Controls.Add(this._mainToolStrip);
      this._splitContainer.Panel1MinSize = 200;
      // 
      // _splitContainer.Panel2
      // 
      this._splitContainer.Panel2.Controls.Add(this._selectedMessageDetails);
      this._splitContainer.Size = new System.Drawing.Size(1008, 730);
      this._splitContainer.SplitterDistance = 500;
      this._splitContainer.TabIndex = 0;
      // 
      // _messageHistory
      // 
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
      this._messageHistory.Dock = System.Windows.Forms.DockStyle.Fill;
      this._messageHistory.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._messageHistory.ForeColor = System.Drawing.Color.Silver;
      this._messageHistory.FullRowSelect = true;
      this._messageHistory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this._messageHistory.Location = new System.Drawing.Point(0, 25);
      this._messageHistory.MultiSelect = false;
      this._messageHistory.Name = "_messageHistory";
      this._messageHistory.ShowGroups = false;
      this._messageHistory.ShowItemToolTips = true;
      this._messageHistory.Size = new System.Drawing.Size(1008, 475);
      this._messageHistory.TabIndex = 3;
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
      // _mainToolStrip
      // 
      this._mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this._mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exitButton,
            this._separator1,
            this._clearHistoryButton,
            this._colorPickerButton,
            this._scrollResumeButton,
            this._separator2,
            this._filterLevelButton,
            this._filterProcessButton,
            this._filterApplicationButton,
            this._filterSourceButton,
            this._filterUserButton,
            this._filterMessageButton,
            this._separator3,
            this._searchButton,
            this._searchText,
            this._searchPrevButton,
            this._searchNextButton});
      this._mainToolStrip.Location = new System.Drawing.Point(0, 0);
      this._mainToolStrip.Name = "_mainToolStrip";
      this._mainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this._mainToolStrip.Size = new System.Drawing.Size(1008, 25);
      this._mainToolStrip.TabIndex = 2;
      // 
      // _exitButton
      // 
      this._exitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._exitButton.Image = ((System.Drawing.Image)(resources.GetObject("_exitButton.Image")));
      this._exitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._exitButton.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
      this._exitButton.Name = "_exitButton";
      this._exitButton.Size = new System.Drawing.Size(23, 22);
      this._exitButton.Text = "Close Application ";
      // 
      // _separator1
      // 
      this._separator1.Name = "_separator1";
      this._separator1.Size = new System.Drawing.Size(6, 25);
      // 
      // _clearHistoryButton
      // 
      this._clearHistoryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._clearHistoryButton.Image = ((System.Drawing.Image)(resources.GetObject("_clearHistoryButton.Image")));
      this._clearHistoryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._clearHistoryButton.Name = "_clearHistoryButton";
      this._clearHistoryButton.Size = new System.Drawing.Size(23, 22);
      this._clearHistoryButton.Text = "Clear History";
      // 
      // _colorPickerButton
      // 
      this._colorPickerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._colorPickerButton.Image = ((System.Drawing.Image)(resources.GetObject("_colorPickerButton.Image")));
      this._colorPickerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._colorPickerButton.Name = "_colorPickerButton";
      this._colorPickerButton.Size = new System.Drawing.Size(23, 22);
      this._colorPickerButton.Text = "Color Picker";
      // 
      // _scrollResumeButton
      // 
      this._scrollResumeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._scrollResumeButton.Image = ((System.Drawing.Image)(resources.GetObject("_scrollResumeButton.Image")));
      this._scrollResumeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._scrollResumeButton.Name = "_scrollResumeButton";
      this._scrollResumeButton.Size = new System.Drawing.Size(23, 22);
      this._scrollResumeButton.Text = "Resume Auto-Scroll";
      // 
      // _separator2
      // 
      this._separator2.Name = "_separator2";
      this._separator2.Size = new System.Drawing.Size(6, 25);
      // 
      // _filterLevelButton
      // 
      this._filterLevelButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._filterLevelButton.Enabled = false;
      this._filterLevelButton.Image = ((System.Drawing.Image)(resources.GetObject("_filterLevelButton.Image")));
      this._filterLevelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._filterLevelButton.Name = "_filterLevelButton";
      this._filterLevelButton.Size = new System.Drawing.Size(23, 22);
      this._filterLevelButton.Text = "Filter by Level";
      // 
      // _filterProcessButton
      // 
      this._filterProcessButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._filterProcessButton.Enabled = false;
      this._filterProcessButton.Image = ((System.Drawing.Image)(resources.GetObject("_filterProcessButton.Image")));
      this._filterProcessButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._filterProcessButton.Name = "_filterProcessButton";
      this._filterProcessButton.Size = new System.Drawing.Size(23, 22);
      this._filterProcessButton.Text = "Filter by Processes";
      // 
      // _filterApplicationButton
      // 
      this._filterApplicationButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._filterApplicationButton.Enabled = false;
      this._filterApplicationButton.Image = ((System.Drawing.Image)(resources.GetObject("_filterApplicationButton.Image")));
      this._filterApplicationButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._filterApplicationButton.Name = "_filterApplicationButton";
      this._filterApplicationButton.Size = new System.Drawing.Size(23, 22);
      this._filterApplicationButton.Text = "Filter by Applications";
      // 
      // _filterSourceButton
      // 
      this._filterSourceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._filterSourceButton.Enabled = false;
      this._filterSourceButton.Image = ((System.Drawing.Image)(resources.GetObject("_filterSourceButton.Image")));
      this._filterSourceButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._filterSourceButton.Name = "_filterSourceButton";
      this._filterSourceButton.Size = new System.Drawing.Size(23, 22);
      this._filterSourceButton.Text = "Filter by Sources";
      // 
      // _filterUserButton
      // 
      this._filterUserButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._filterUserButton.Enabled = false;
      this._filterUserButton.Image = ((System.Drawing.Image)(resources.GetObject("_filterUserButton.Image")));
      this._filterUserButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._filterUserButton.Name = "_filterUserButton";
      this._filterUserButton.Size = new System.Drawing.Size(23, 22);
      this._filterUserButton.Text = "Filter by Users";
      // 
      // _filterMessageButton
      // 
      this._filterMessageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._filterMessageButton.Enabled = false;
      this._filterMessageButton.Image = ((System.Drawing.Image)(resources.GetObject("_filterMessageButton.Image")));
      this._filterMessageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._filterMessageButton.Name = "_filterMessageButton";
      this._filterMessageButton.Size = new System.Drawing.Size(23, 22);
      this._filterMessageButton.Text = "Filter by Messages";
      // 
      // _separator3
      // 
      this._separator3.Name = "_separator3";
      this._separator3.Size = new System.Drawing.Size(6, 25);
      // 
      // _searchButton
      // 
      this._searchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._searchButton.Enabled = false;
      this._searchButton.Image = ((System.Drawing.Image)(resources.GetObject("_searchButton.Image")));
      this._searchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._searchButton.Name = "_searchButton";
      this._searchButton.Size = new System.Drawing.Size(23, 22);
      this._searchButton.Text = "Search";
      // 
      // _searchText
      // 
      this._searchText.Enabled = false;
      this._searchText.Name = "_searchText";
      this._searchText.Size = new System.Drawing.Size(250, 25);
      // 
      // _searchPrevButton
      // 
      this._searchPrevButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._searchPrevButton.Enabled = false;
      this._searchPrevButton.Image = ((System.Drawing.Image)(resources.GetObject("_searchPrevButton.Image")));
      this._searchPrevButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._searchPrevButton.Name = "_searchPrevButton";
      this._searchPrevButton.Size = new System.Drawing.Size(23, 22);
      this._searchPrevButton.Text = "Find Previous";
      // 
      // _searchNextButton
      // 
      this._searchNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this._searchNextButton.Enabled = false;
      this._searchNextButton.Image = ((System.Drawing.Image)(resources.GetObject("_searchNextButton.Image")));
      this._searchNextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this._searchNextButton.Name = "_searchNextButton";
      this._searchNextButton.Size = new System.Drawing.Size(23, 22);
      this._searchNextButton.Text = "Find Next";
      // 
      // _selectedMessageDetails
      // 
      this._selectedMessageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageDetails.Controls.Add(this._messageDetailsTab);
      this._selectedMessageDetails.Controls.Add(this._attributeDetailsTab);
      this._selectedMessageDetails.Controls.Add(this._rawDetailsTab);
      this._selectedMessageDetails.Location = new System.Drawing.Point(3, 3);
      this._selectedMessageDetails.Name = "_selectedMessageDetails";
      this._selectedMessageDetails.SelectedIndex = 0;
      this._selectedMessageDetails.Size = new System.Drawing.Size(1002, 220);
      this._selectedMessageDetails.TabIndex = 2;
      // 
      // _messageDetailsTab
      // 
      this._messageDetailsTab.Controls.Add(this._selectedMessageDetailsLayout);
      this._messageDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._messageDetailsTab.Name = "_messageDetailsTab";
      this._messageDetailsTab.Padding = new System.Windows.Forms.Padding(3);
      this._messageDetailsTab.Size = new System.Drawing.Size(994, 194);
      this._messageDetailsTab.TabIndex = 0;
      this._messageDetailsTab.Text = "Message";
      this._messageDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _selectedMessageDetailsLayout
      // 
      this._selectedMessageDetailsLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageDetailsLayout.ColumnCount = 4;
      this._selectedMessageDetailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this._selectedMessageDetailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this._selectedMessageDetailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this._selectedMessageDetailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageLevel, 1, 1);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageUsernameHeader, 3, 2);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageThreadHeader, 2, 2);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageSourceHeader, 2, 0);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageLevelHeader, 1, 0);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageIdHeader, 0, 0);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageId, 0, 1);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageTimestamp, 0, 3);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageTimestampHeader, 0, 2);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageText, 1, 5);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageTextHeader, 0, 4);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageProcessHeader, 1, 2);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageSource, 2, 1);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageProcess, 1, 3);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageThread, 2, 3);
      this._selectedMessageDetailsLayout.Controls.Add(this._selectedMessageUsername, 3, 3);
      this._selectedMessageDetailsLayout.Location = new System.Drawing.Point(6, 6);
      this._selectedMessageDetailsLayout.Margin = new System.Windows.Forms.Padding(0);
      this._selectedMessageDetailsLayout.Name = "_selectedMessageDetailsLayout";
      this._selectedMessageDetailsLayout.RowCount = 7;
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this._selectedMessageDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this._selectedMessageDetailsLayout.Size = new System.Drawing.Size(982, 182);
      this._selectedMessageDetailsLayout.TabIndex = 1;
      // 
      // _selectedMessageLevel
      // 
      this._selectedMessageLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageLevel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageLevel.Location = new System.Drawing.Point(248, 16);
      this._selectedMessageLevel.Name = "_selectedMessageLevel";
      this._selectedMessageLevel.ReadOnly = true;
      this._selectedMessageLevel.Size = new System.Drawing.Size(239, 20);
      this._selectedMessageLevel.TabIndex = 3;
      // 
      // _selectedMessageUsernameHeader
      // 
      this._selectedMessageUsernameHeader.AutoSize = true;
      this._selectedMessageUsernameHeader.Location = new System.Drawing.Point(738, 39);
      this._selectedMessageUsernameHeader.Name = "_selectedMessageUsernameHeader";
      this._selectedMessageUsernameHeader.Size = new System.Drawing.Size(58, 13);
      this._selectedMessageUsernameHeader.TabIndex = 12;
      this._selectedMessageUsernameHeader.Text = "Username:";
      // 
      // _selectedMessageThreadHeader
      // 
      this._selectedMessageThreadHeader.AutoSize = true;
      this._selectedMessageThreadHeader.Location = new System.Drawing.Point(493, 39);
      this._selectedMessageThreadHeader.Name = "_selectedMessageThreadHeader";
      this._selectedMessageThreadHeader.Size = new System.Drawing.Size(44, 13);
      this._selectedMessageThreadHeader.TabIndex = 10;
      this._selectedMessageThreadHeader.Text = "Thread:";
      // 
      // _selectedMessageSourceHeader
      // 
      this._selectedMessageSourceHeader.AutoSize = true;
      this._selectedMessageSourceHeader.Location = new System.Drawing.Point(493, 0);
      this._selectedMessageSourceHeader.Name = "_selectedMessageSourceHeader";
      this._selectedMessageSourceHeader.Size = new System.Drawing.Size(44, 13);
      this._selectedMessageSourceHeader.TabIndex = 4;
      this._selectedMessageSourceHeader.Text = "Source:";
      // 
      // _selectedMessageLevelHeader
      // 
      this._selectedMessageLevelHeader.AutoSize = true;
      this._selectedMessageLevelHeader.Location = new System.Drawing.Point(248, 0);
      this._selectedMessageLevelHeader.Name = "_selectedMessageLevelHeader";
      this._selectedMessageLevelHeader.Size = new System.Drawing.Size(36, 13);
      this._selectedMessageLevelHeader.TabIndex = 2;
      this._selectedMessageLevelHeader.Text = "Level:";
      // 
      // _selectedMessageIdHeader
      // 
      this._selectedMessageIdHeader.AutoSize = true;
      this._selectedMessageIdHeader.Location = new System.Drawing.Point(3, 0);
      this._selectedMessageIdHeader.Name = "_selectedMessageIdHeader";
      this._selectedMessageIdHeader.Size = new System.Drawing.Size(21, 13);
      this._selectedMessageIdHeader.TabIndex = 0;
      this._selectedMessageIdHeader.Text = "ID:";
      // 
      // _selectedMessageId
      // 
      this._selectedMessageId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageId.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageId.Location = new System.Drawing.Point(3, 16);
      this._selectedMessageId.Name = "_selectedMessageId";
      this._selectedMessageId.ReadOnly = true;
      this._selectedMessageId.Size = new System.Drawing.Size(239, 20);
      this._selectedMessageId.TabIndex = 1;
      // 
      // _selectedMessageTimestamp
      // 
      this._selectedMessageTimestamp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageTimestamp.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageTimestamp.Location = new System.Drawing.Point(3, 55);
      this._selectedMessageTimestamp.Name = "_selectedMessageTimestamp";
      this._selectedMessageTimestamp.ReadOnly = true;
      this._selectedMessageTimestamp.Size = new System.Drawing.Size(239, 20);
      this._selectedMessageTimestamp.TabIndex = 7;
      // 
      // _selectedMessageTimestampHeader
      // 
      this._selectedMessageTimestampHeader.AutoSize = true;
      this._selectedMessageTimestampHeader.Location = new System.Drawing.Point(3, 39);
      this._selectedMessageTimestampHeader.Name = "_selectedMessageTimestampHeader";
      this._selectedMessageTimestampHeader.Size = new System.Drawing.Size(58, 13);
      this._selectedMessageTimestampHeader.TabIndex = 6;
      this._selectedMessageTimestampHeader.Text = "Timestamp";
      // 
      // _selectedMessageText
      // 
      this._selectedMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageDetailsLayout.SetColumnSpan(this._selectedMessageText, 4);
      this._selectedMessageText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageText.Location = new System.Drawing.Point(3, 94);
      this._selectedMessageText.Name = "_selectedMessageText";
      this._selectedMessageText.ReadOnly = true;
      this._selectedMessageText.Size = new System.Drawing.Size(976, 85);
      this._selectedMessageText.TabIndex = 15;
      this._selectedMessageText.Text = "";
      this._selectedMessageText.WordWrap = false;
      // 
      // _selectedMessageTextHeader
      // 
      this._selectedMessageTextHeader.AutoSize = true;
      this._selectedMessageTextHeader.Location = new System.Drawing.Point(3, 78);
      this._selectedMessageTextHeader.Name = "_selectedMessageTextHeader";
      this._selectedMessageTextHeader.Size = new System.Drawing.Size(53, 13);
      this._selectedMessageTextHeader.TabIndex = 14;
      this._selectedMessageTextHeader.Text = "Message:";
      // 
      // _selectedMessageProcessHeader
      // 
      this._selectedMessageProcessHeader.AutoSize = true;
      this._selectedMessageProcessHeader.Location = new System.Drawing.Point(248, 39);
      this._selectedMessageProcessHeader.Name = "_selectedMessageProcessHeader";
      this._selectedMessageProcessHeader.Size = new System.Drawing.Size(48, 13);
      this._selectedMessageProcessHeader.TabIndex = 8;
      this._selectedMessageProcessHeader.Text = "Process:";
      // 
      // _selectedMessageSource
      // 
      this._selectedMessageSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageDetailsLayout.SetColumnSpan(this._selectedMessageSource, 2);
      this._selectedMessageSource.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageSource.Location = new System.Drawing.Point(493, 16);
      this._selectedMessageSource.Name = "_selectedMessageSource";
      this._selectedMessageSource.ReadOnly = true;
      this._selectedMessageSource.Size = new System.Drawing.Size(486, 20);
      this._selectedMessageSource.TabIndex = 5;
      // 
      // _selectedMessageProcess
      // 
      this._selectedMessageProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageProcess.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageProcess.Location = new System.Drawing.Point(248, 55);
      this._selectedMessageProcess.Name = "_selectedMessageProcess";
      this._selectedMessageProcess.ReadOnly = true;
      this._selectedMessageProcess.Size = new System.Drawing.Size(239, 20);
      this._selectedMessageProcess.TabIndex = 9;
      // 
      // _selectedMessageThread
      // 
      this._selectedMessageThread.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageThread.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageThread.Location = new System.Drawing.Point(493, 55);
      this._selectedMessageThread.Name = "_selectedMessageThread";
      this._selectedMessageThread.ReadOnly = true;
      this._selectedMessageThread.Size = new System.Drawing.Size(239, 20);
      this._selectedMessageThread.TabIndex = 11;
      // 
      // _selectedMessageUsername
      // 
      this._selectedMessageUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._selectedMessageUsername.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._selectedMessageUsername.Location = new System.Drawing.Point(738, 55);
      this._selectedMessageUsername.Name = "_selectedMessageUsername";
      this._selectedMessageUsername.ReadOnly = true;
      this._selectedMessageUsername.Size = new System.Drawing.Size(241, 20);
      this._selectedMessageUsername.TabIndex = 13;
      // 
      // _attributeDetailsTab
      // 
      this._attributeDetailsTab.Controls.Add(this._attributesText);
      this._attributeDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._attributeDetailsTab.Name = "_attributeDetailsTab";
      this._attributeDetailsTab.Padding = new System.Windows.Forms.Padding(3);
      this._attributeDetailsTab.Size = new System.Drawing.Size(994, 194);
      this._attributeDetailsTab.TabIndex = 1;
      this._attributeDetailsTab.Text = "Attributes";
      this._attributeDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _attributesText
      // 
      this._attributesText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._attributesText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._attributesText.Location = new System.Drawing.Point(6, 6);
      this._attributesText.Name = "_attributesText";
      this._attributesText.ReadOnly = true;
      this._attributesText.Size = new System.Drawing.Size(985, 182);
      this._attributesText.TabIndex = 1;
      this._attributesText.Text = "";
      this._attributesText.WordWrap = false;
      // 
      // _rawDetailsTab
      // 
      this._rawDetailsTab.Controls.Add(this._rawText);
      this._rawDetailsTab.Location = new System.Drawing.Point(4, 22);
      this._rawDetailsTab.Name = "_rawDetailsTab";
      this._rawDetailsTab.Size = new System.Drawing.Size(994, 194);
      this._rawDetailsTab.TabIndex = 2;
      this._rawDetailsTab.Text = "Raw";
      this._rawDetailsTab.UseVisualStyleBackColor = true;
      // 
      // _rawText
      // 
      this._rawText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this._rawText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this._rawText.Location = new System.Drawing.Point(6, 6);
      this._rawText.Name = "_rawText";
      this._rawText.ReadOnly = true;
      this._rawText.Size = new System.Drawing.Size(985, 183);
      this._rawText.TabIndex = 1;
      this._rawText.Text = "";
      this._rawText.WordWrap = false;
      // 
      // Main
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1008, 730);
      this.Controls.Add(this._splitContainer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(800, 600);
      this.Name = "Main";
      this.Text = "Main";
      this._splitContainer.Panel1.ResumeLayout(false);
      this._splitContainer.Panel1.PerformLayout();
      this._splitContainer.Panel2.ResumeLayout(false);
      this._splitContainer.ResumeLayout(false);
      this._mainToolStrip.ResumeLayout(false);
      this._mainToolStrip.PerformLayout();
      this._selectedMessageDetails.ResumeLayout(false);
      this._messageDetailsTab.ResumeLayout(false);
      this._selectedMessageDetailsLayout.ResumeLayout(false);
      this._selectedMessageDetailsLayout.PerformLayout();
      this._attributeDetailsTab.ResumeLayout(false);
      this._rawDetailsTab.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer _splitContainer;
    private System.Windows.Forms.TabControl _selectedMessageDetails;
    private System.Windows.Forms.TabPage _messageDetailsTab;
    private System.Windows.Forms.RichTextBox _selectedMessageText;
    private System.Windows.Forms.TabPage _attributeDetailsTab;
    private System.Windows.Forms.TabPage _rawDetailsTab;
    private System.Windows.Forms.RichTextBox _attributesText;
    private System.Windows.Forms.RichTextBox _rawText;
    private System.Windows.Forms.TableLayoutPanel _selectedMessageDetailsLayout;
    private System.Windows.Forms.TextBox _selectedMessageId;
    private System.Windows.Forms.TextBox _selectedMessageTimestamp;
    private System.Windows.Forms.Label _selectedMessageIdHeader;
    private System.Windows.Forms.Label _selectedMessageTimestampHeader;
    private System.Windows.Forms.Label _selectedMessageTextHeader;
    private System.Windows.Forms.TextBox _selectedMessageLevel;
    private System.Windows.Forms.Label _selectedMessageUsernameHeader;
    private System.Windows.Forms.Label _selectedMessageThreadHeader;
    private System.Windows.Forms.Label _selectedMessageSourceHeader;
    private System.Windows.Forms.Label _selectedMessageLevelHeader;
    private System.Windows.Forms.Label _selectedMessageProcessHeader;
    private System.Windows.Forms.TextBox _selectedMessageSource;
    private System.Windows.Forms.TextBox _selectedMessageProcess;
    private System.Windows.Forms.TextBox _selectedMessageThread;
    private System.Windows.Forms.TextBox _selectedMessageUsername;
    private System.Windows.Forms.ToolStrip _mainToolStrip;
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
    private System.Windows.Forms.ToolStripButton _exitButton;
    private System.Windows.Forms.ToolStripSeparator _separator1;
    private System.Windows.Forms.ToolStripButton _clearHistoryButton;
    private System.Windows.Forms.ToolStripButton _colorPickerButton;
    private System.Windows.Forms.ToolStripButton _scrollResumeButton;
    private System.Windows.Forms.ToolStripSeparator _separator2;
    private System.Windows.Forms.ToolStripButton _filterLevelButton;
    private System.Windows.Forms.ToolStripButton _filterProcessButton;
    private System.Windows.Forms.ToolStripButton _filterApplicationButton;
    private System.Windows.Forms.ToolStripButton _filterSourceButton;
    private System.Windows.Forms.ToolStripButton _filterUserButton;
    private System.Windows.Forms.ToolStripButton _filterMessageButton;
    private System.Windows.Forms.ToolStripSeparator _separator3;
    private System.Windows.Forms.ToolStripButton _searchButton;
    private System.Windows.Forms.ToolStripTextBox _searchText;
    private System.Windows.Forms.ToolStripButton _searchPrevButton;
    private System.Windows.Forms.ToolStripButton _searchNextButton;
  }
}