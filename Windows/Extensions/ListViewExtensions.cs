using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

namespace Harvester.Extensions
{
  public static class ListViewExtensions
  {
    private enum ListViewMessages
    {
      First = 0x1000,
      SetExtendedStyle = (First + 54),
      GetExtendedStyle = (First + 55),
    }

    [Flags]
    private enum ListViewExtendedStyles
    {
      BorderSelect = 0x00008000,
      DoubleBuffer = 0x00010000
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern Int32 SendMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, Int32 lParam);
    
    public static void EnableDoubleBuffer(this ListView listView)
    {
      var styles = (ListViewExtendedStyles)SendMessage(listView.Handle, (Int32)ListViewMessages.GetExtendedStyle, 0, 0);

      styles |= ListViewExtendedStyles.DoubleBuffer | ListViewExtendedStyles.BorderSelect;

      SendMessage(listView.Handle, (Int32)ListViewMessages.SetExtendedStyle, 0, (Int32)styles);
    }

    public static void DisableDoubleBuffer(this ListView listView)
    {
      var styles = (ListViewExtendedStyles)SendMessage(listView.Handle, (Int32)ListViewMessages.GetExtendedStyle, 0, 0);

      styles -= styles & ListViewExtendedStyles.DoubleBuffer; styles -= styles & ListViewExtendedStyles.BorderSelect;

      SendMessage(listView.Handle, (Int32)ListViewMessages.SetExtendedStyle, 0, (Int32)styles);
    }
  }
}
