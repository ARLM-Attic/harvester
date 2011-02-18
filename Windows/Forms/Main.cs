using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Harvester.Core.Messages;
using Harvester.Core.Messages.Sources.DbWin;
using Harvester.Windows.Extensions;
using Harvester.Windows.Properties;

/* Copyright (c) 2011 CBaxter
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
 * IN THE SOFTWARE. 
 */

namespace Harvester.Windows.Forms
{
  public partial class Main : Form
  {
    private readonly ILogMessageSource _logMessageSource = new DbWinMessageSource();

    public Main()
    {
      InitializeComponent();

      _colorsToolStripMenuItem.Click += OnShowColorPickerClicked;

      _messageHistory.EnableDoubleBuffer();
      _messageHistory.SelectedIndexChanged += OnSelectedMessageChanged;
      _messageHistory.Resize += OnMessageHistoryResized;

      _logMessageSource.OnMessagesReceived += OnMessagesReceived;
      _logMessageSource.Connect();
    }

    private void OnMessagesReceived(Object sender, LogMessagesReceivedEventArgs e)
    {
      _messageHistory.Invoke(new Action(() =>
      {
        var scrollToEnd = _messageHistory.SelectedIndices.Count == 0 || _messageHistory.SelectedIndices[0] == (_messageHistory.Items.Count - 1);

        _messageHistory.BeginUpdate();
        _messageHistory.SuspendLayout();

        foreach (var message in e.Messages)
        {
          var listViewItem = new ListViewItem(new String[] {
                                message.MessageId.ToString(),
                                message.Timestamp.ToString("yyyy-MM-dd HH:mm:ss,fff"),
                                message.Level.ToString(),
                                message.ProcessId.ToString(),
                                message.ProcessName,
                                message.Thread,
                                message.Source,
                                message.Username,
                                message.Message
                              });

          listViewItem.ForeColor = GetForegroundColor(message.Level);
          listViewItem.BackColor = GetBackgroundColor(message.Level);
          listViewItem.Tag = message;

          _messageHistory.Items.Add(listViewItem);
        }

        if (scrollToEnd)
        {
          _messageHistory.SelectedIndices.Clear();
          _messageHistory.EnsureVisible(_messageHistory.Items.Count - 1);
        }

        _messageHistory.ResumeLayout();
        _messageHistory.EndUpdate();
      }));
    }

    private void OnMessageHistoryResized(Object sender, EventArgs e)
    {
      ExtendMessageColumn();
    }

    private void ExtendMessageColumn()
    {
      Int32 totalWidth = _messageHistory.Columns.Cast<ColumnHeader>().Sum(column => column.Width);
      Int32 messageColumnWidth = _messageHistory.Width - (totalWidth - _messageColumn.Width) - SystemInformation.VerticalScrollBarWidth - 4;

      _messageColumn.Width = Math.Max(60, messageColumnWidth);
    }

    private void OnSelectedMessageChanged(Object sender, EventArgs e)
    {
      if (_messageHistory.SelectedItems.Count > 0)
      {
        var message = _messageHistory.SelectedItems[0].Tag as ILogMessage;

        if (message == null)
          ClearSelectedMessage();
        else
          DisplaySelectedMessage(message);
      }
      else
      {
        ClearSelectedMessage();
      }
    }

    private void ClearSelectedMessage()
    {
      _selectedMessageDetails.SelectedIndex = 0;

      _rawText.Text = String.Empty;

      _selectedMessageId.Text = String.Empty;
      _selectedMessageLevel.Text = String.Empty;
      _selectedMessageSource.Text = String.Empty;
      _selectedMessageTimestamp.Text = String.Empty;
      _selectedMessageProcess.Text = String.Empty;
      _selectedMessageThread.Text = String.Empty;
      _selectedMessageUsername.Text = String.Empty;
      _selectedMessageText.Text = String.Empty;

      _attributesText.Text = String.Empty;
    }

    private void DisplaySelectedMessage(ILogMessage message)
    {
      _rawText.Text = message.RawMessage;
      
      _selectedMessageId.Text = message.MessageId.ToString();
      _selectedMessageLevel.Text = message.Level.ToString();
      _selectedMessageSource.Text = message.Source;
      _selectedMessageTimestamp.Text = message.Timestamp.ToString("yyyy-MM-dd HH:mm:ss,fff");
      _selectedMessageProcess.Text = "[" + message.ProcessId + "] " + message.ProcessName;
      _selectedMessageThread.Text = message.Thread;
      _selectedMessageUsername.Text = message.Username;
      _selectedMessageText.Text = message.Message.Trim();

      if (!String.IsNullOrEmpty(message.Exception))
        _selectedMessageText.Text += Environment.NewLine + message.Exception;

      var sb = new StringBuilder();
      foreach (var attribute in message.Attributes)
      {
        var value = (attribute.Value ?? String.Empty).ToString();

        sb.Append(attribute.Name);
        sb.AppendLine(":");
        sb.AppendLine(String.IsNullOrEmpty(value) ? "<Not Set>" : value);
        sb.AppendLine();
      }

      _attributesText.Text = sb.ToString();
    }

    private void OnShowColorPickerClicked(Object sender, EventArgs e)
    {
      using (var colorPicker = new ColorPicker())
      {
        if (colorPicker.ShowDialog(this) == DialogResult.OK)
          _messageHistory.BackColor = MessageColor.Default.PrimaryBackColor;
      }
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);

      WindowLayout.Default.WindowSize = Size;
      WindowLayout.Default.WindowLocation = Location;
      WindowLayout.Default.WindowState = WindowState == FormWindowState.Minimized ? FormWindowState.Normal : WindowState;

      WindowLayout.Default.MessageIdWidth = _idColumn.Width;
      WindowLayout.Default.TimestampWidth = _timestampColumn.Width;
      WindowLayout.Default.LevelWidth = _levelColumn.Width;
      WindowLayout.Default.ProcessIdWidth = _processIdColumn.Width;
      WindowLayout.Default.ProcessNameWidth = _processNameColumn.Width;
      WindowLayout.Default.ThreadWidth = _threadColumn.Width;
      WindowLayout.Default.SourceWidth = _sourceColumn.Width;
      WindowLayout.Default.UsernameWidth = _userColumn.Width;

      WindowLayout.Default.SplitPosition = _splitContainer.SplitterDistance;

      WindowLayout.Default.Save();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      Text = String.Format("{0} v{1}", Application.ProductName, Application.ProductVersion);

      var formSize = WindowLayout.Default.WindowSize;
      var formLocation = WindowLayout.Default.WindowLocation;
      var formDimensions = new Rectangle(formLocation, formSize);
      var workingArea = Screen.GetWorkingArea(formDimensions);

      //Set Form Dimensions
      Size = formSize;
      Location = workingArea.Contains(formLocation) ? formLocation : workingArea.Location;
      WindowState = WindowLayout.Default.WindowState;

      //Set Column Widths
      _idColumn.Width = Math.Max(60, WindowLayout.Default.MessageIdWidth);
      _timestampColumn.Width = Math.Max(60, WindowLayout.Default.TimestampWidth);
      _levelColumn.Width = Math.Max(60, WindowLayout.Default.LevelWidth);
      _processIdColumn.Width = Math.Max(60, WindowLayout.Default.ProcessIdWidth);
      _processNameColumn.Width = Math.Max(60, WindowLayout.Default.ProcessNameWidth);
      _threadColumn.Width = Math.Max(60, WindowLayout.Default.ThreadWidth);
      _sourceColumn.Width = Math.Max(60, WindowLayout.Default.SourceWidth);
      _userColumn.Width = Math.Max(60, WindowLayout.Default.UsernameWidth);

      if (_splitContainer.Height != 0)
        _splitContainer.SplitterDistance = Math.Max(_splitContainer.Panel1MinSize, Math.Min(WindowLayout.Default.SplitPosition, _splitContainer.Height - _splitContainer.Panel2MinSize));

      _messageHistory.BackColor = MessageColor.Default.PrimaryBackColor;

      ExtendMessageColumn();
    }

    private static Color GetBackgroundColor(LogMessageLevel level)
    {
      switch (level)
      {
        case LogMessageLevel.Fatal: return MessageColor.Default.FatalBackColor;
        case LogMessageLevel.Error: return MessageColor.Default.ErrorBackColor;
        case LogMessageLevel.Warning: return MessageColor.Default.WarningBackColor;
        case LogMessageLevel.Information: return MessageColor.Default.InformationBackColor;
        case LogMessageLevel.Debug: return MessageColor.Default.DebugBackColor;
        default: return MessageColor.Default.TraceBackColor;
      }
    }

    private static Color GetForegroundColor(LogMessageLevel level)
    {
      switch (level)
      {
        case LogMessageLevel.Fatal: return MessageColor.Default.FatalForeColor;
        case LogMessageLevel.Error: return MessageColor.Default.ErrorForeColor;
        case LogMessageLevel.Warning: return MessageColor.Default.WarningForeColor;
        case LogMessageLevel.Information: return MessageColor.Default.InformationForeColor;
        case LogMessageLevel.Debug: return MessageColor.Default.DebugForeColor;
        default: return MessageColor.Default.TraceForeColor;
      }
    }

  }
}
