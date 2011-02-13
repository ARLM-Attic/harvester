using System;

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

namespace Harvester.Core.Win32.UI
{
  [Flags]
  public enum ListViewExtendedStyles
  {
    GridLines = 0x00000001,
    SubItemImages = 0x00000002,
    CheckBoxes = 0x00000004,
    TrackSelect = 0x00000008,
    HeaderDragDrop = 0x00000010,
    FullRowSelect = 0x00000020,
    OneClickActivate = 0x00000040,
    TwoClickActivate = 0x00000080,
    FlatsB = 0x00000100,
    Regional = 0x00000200,
    InfoTip = 0x00000400,
    UnderlineHot = 0x00000800,
    UnderlineCold = 0x00001000,
    MultilWorkAreas = 0x00002000,
    LabelTip = 0x00004000,
    BorderSelect = 0x00008000,
    DoubleBuffer = 0x00010000,
    HideLabels = 0x00020000,
    SingleRow = 0x00040000, 
    SnapToGrid = 0x00080000,
    SimpleSelect = 0x00100000
  }
}
