using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Harvester.Core;
using Harvester.Core.Logging;
using Harvester.Core.Messages;
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


  public partial class Main : FormBase, ILogMessageRenderer
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly WindowsMonitor _windowsMonitor;

    public Main()
    {
      InitializeComponent();

      //NOTE: Explicitly moved the following statement out of the 'Windows Form Designer generated code' region
      //      as IDE would always place the setting of Panel2MinSize prior to setting of SplitterDistance. As 
      //      such, any time the designer was modified (i.e., adding new control), the following error would be 
      //      raised: SplitterDistance must be between Panel1MinSize and Width - Panel2MinSize.
      _splitContainer.Panel2MinSize = 200;

      _exitButton.Click += OnExitApplicationClicked;
      _clearHistoryButton.Click += OnClearMessageHistoryClicked;
      _colorPickerButton.Click += OnShowColorPickerClicked;
      _scrollResumeButton.Click += OnScrollResumeClicked;

      _messageHistory.EnableDoubleBuffer();
      _messageHistory.SelectedIndexChanged += OnSelectedMessageChanged;
      _messageHistory.Resize += OnMessageHistoryResized;

      _windowsMonitor = new WindowsMonitor(this);
    }

    public void Render(IEnumerable<ILogMessage> logMessages)
    {
      Log.Info("One or more messages received.");
      HandleEvent(() => ProcessMessages(logMessages));
    }

    private void OnClearMessageHistoryClicked(Object sender, EventArgs e)
    {
      Log.Info("Clearing message history.");
      HandleEvent(() =>
                    {
                      ClearSelectedMessage();

                      _messageHistory.Items.Clear();
                    });
    }

    private void OnShowColorPickerClicked(Object sender, EventArgs e)
    {
      Log.Info("Showing color picker.");
      HandleEvent(() =>
                    {
                      using (var colorPicker = new ColorPicker())
                      {
                        if (colorPicker.ShowDialog(this) == DialogResult.OK)
                          _messageHistory.BackColor = MessageColor.Default.PrimaryBackColor;
                      }
                    });
    }

    private void OnScrollResumeClicked(Object sender, EventArgs e)
    {
      Log.Info("Resuming auto-scroll.");
      HandleEvent(() =>
                    {
                      _messageHistory.SelectedIndices.Clear();

                      if (_messageHistory.Items.Count > 0)
                        _messageHistory.EnsureVisible(_messageHistory.Items.Count - 1);
                    });
    }

    private void OnExitApplicationClicked(Object sender, EventArgs e)
    {
      Log.Info("Exiting application.");
      HandleEvent(Application.Exit);
    }

    private void OnSelectedMessageChanged(Object sender, EventArgs e)
    {
      HandleEvent(() =>
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
                    });
    }

    private void OnMessageHistoryResized(Object sender, EventArgs e)
    {
      Log.Info("Resizing message history ListView.");
      HandleEvent(ExtendMessageColumn);
    }

    private void ExtendMessageColumn()
    {
      Int32 totalWidth = _messageHistory.Columns.Cast<ColumnHeader>().Sum(column => column.Width);
      Int32 messageColumnWidth = _messageHistory.Width - (totalWidth - _messageColumn.Width) - SystemInformation.VerticalScrollBarWidth - 4;

      _messageColumn.Width = Math.Max(60, messageColumnWidth);
    }

    private void ProcessMessages(IEnumerable<ILogMessage> messages)
    {
      var scrollToEnd = _messageHistory.SelectedIndices.Count == 0 || _messageHistory.SelectedIndices[0] == (_messageHistory.Items.Count - 1);

      _messageHistory.BeginUpdate();
      _messageHistory.SuspendLayout();

      foreach (var message in messages)
        _messageHistory.Items.Add(CreateListViewItem(message));

      if (scrollToEnd)
      {
        _messageHistory.SelectedIndices.Clear();
        _messageHistory.EnsureVisible(_messageHistory.Items.Count - 1);
      }

      _messageHistory.ResumeLayout();
      _messageHistory.EndUpdate();
    }

    private static ListViewItem CreateListViewItem(ILogMessage message)
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

      listViewItem.Tag = message;
      listViewItem.ForeColor = GetForegroundColor(message.Level);
      listViewItem.BackColor = GetBackgroundColor(message.Level);

      return listViewItem;
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

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);

      HandleEvent(() =>
                    {
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

                      _windowsMonitor.Dispose();
                    });
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      HandleEvent(() =>
                    {
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
                    });
    }

    protected override Boolean ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (PerformClickIfAccelerator(keyData, Keys.Control | Keys.Shift | Keys.C, _clearHistoryButton))
        return true;

      if (PerformClickIfAccelerator(keyData, Keys.Control | Keys.Shift | Keys.P, _colorPickerButton))
        return true;

      if (PerformClickIfAccelerator(keyData, Keys.Control | Keys.Shift | Keys.V, _scrollResumeButton))
        return true;

      return base.ProcessCmdKey(ref msg, keyData);
    }

    private static Boolean PerformClickIfAccelerator(Keys keyData, Keys keys, ToolStripButton button)
    {
      var expectedAccelerator = keyData == keys;


      Log.InfoFormat("{0} == {1}", keyData, keys);

      if (expectedAccelerator)
        button.PerformClick();

      return expectedAccelerator;
    }
  }
}
