using System;
using System.ComponentModel;
using System.Windows.Forms;
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
  public partial class ColorPicker : Form
  {
    public ColorPicker()
    {
      InitializeComponent();

      _primaryBackColorButton.Click += (sender, e) => PickColor(_primaryBackColorDisplay);

      _fatalForeColorButton.Click += (sender, e) => PickColor(_fatalForeColorDisplay);
      _fatalBackColorButton.Click += (sender, e) => PickColor(_fatalBackColorDisplay);

      _errorForeColorButton.Click += (sender, e) => PickColor(_errorForeColorDisplay);
      _errorBackColorButton.Click += (sender, e) => PickColor(_errorBackColorDisplay);

      _warningForeColorButton.Click += (sender, e) => PickColor(_warningForeColorDisplay);
      _warningBackColorButton.Click += (sender, e) => PickColor(_warningBackColorDisplay);

      _informationForeColorButton.Click += (sender, e) => PickColor(_informationForeColorDisplay);
      _informationBackColorButton.Click += (sender, e) => PickColor(_informationBackColorDisplay);

      _debugForeColorButton.Click += (sender, e) => PickColor(_debugForeColorDisplay);
      _debugBackColorButton.Click += (sender, e) => PickColor(_debugBackColorDisplay);

      _traceForeColorButton.Click += (sender, e) => PickColor(_traceForeColorDisplay);
      _traceBackColorButton.Click += (sender, e) => PickColor(_traceBackColorDisplay);
    }

    private void PickColor(Control displayControl)
    {
      _colorDialog.Color = displayControl.BackColor;

      if (_colorDialog.ShowDialog(this) == DialogResult.OK)
        displayControl.BackColor = _colorDialog.Color;
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      _primaryBackColorDisplay.BackColor = MessageColor.Default.PrimaryBackColor;

      _fatalForeColorDisplay.BackColor = MessageColor.Default.FatalForeColor;
      _fatalBackColorDisplay.BackColor = MessageColor.Default.FatalBackColor;

      _errorForeColorDisplay.BackColor = MessageColor.Default.ErrorForeColor;
      _errorBackColorDisplay.BackColor = MessageColor.Default.ErrorBackColor;

      _warningForeColorDisplay.BackColor = MessageColor.Default.WarningForeColor;
      _warningBackColorDisplay.BackColor = MessageColor.Default.WarningBackColor;

      _informationForeColorDisplay.BackColor = MessageColor.Default.InformationForeColor;
      _informationBackColorDisplay.BackColor = MessageColor.Default.InformationBackColor;

      _debugForeColorDisplay.BackColor = MessageColor.Default.DebugForeColor;
      _debugBackColorDisplay.BackColor = MessageColor.Default.DebugBackColor;

      _traceForeColorDisplay.BackColor = MessageColor.Default.TraceForeColor;
      _traceBackColorDisplay.BackColor = MessageColor.Default.TraceBackColor;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);

      if (DialogResult != DialogResult.OK)
        return;

      MessageColor.Default.PrimaryBackColor = _primaryBackColorDisplay.BackColor;

      MessageColor.Default.FatalForeColor = _fatalForeColorDisplay.BackColor;
      MessageColor.Default.FatalBackColor = _fatalBackColorDisplay.BackColor;

      MessageColor.Default.ErrorForeColor = _errorForeColorDisplay.BackColor;
      MessageColor.Default.ErrorBackColor = _errorBackColorDisplay.BackColor;

      MessageColor.Default.WarningForeColor = _warningForeColorDisplay.BackColor;
      MessageColor.Default.WarningBackColor = _warningBackColorDisplay.BackColor;

      MessageColor.Default.InformationForeColor = _informationForeColorDisplay.BackColor;
      MessageColor.Default.InformationBackColor = _informationBackColorDisplay.BackColor;

      MessageColor.Default.DebugForeColor = _debugForeColorDisplay.BackColor;
      MessageColor.Default.DebugBackColor = _debugBackColorDisplay.BackColor;

      MessageColor.Default.TraceForeColor = _traceForeColorDisplay.BackColor;
      MessageColor.Default.TraceBackColor = _traceBackColorDisplay.BackColor;

      MessageColor.Default.Save();
    }
  }
}
