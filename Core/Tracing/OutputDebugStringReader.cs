using System;
using System.Collections.Generic;

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

namespace Harvester.Core.Tracing
{
  public sealed class OutputDebugStringReader
  {
    private OutputDebugString OutputDebugString { get; set; }
    private readonly Int32 _fragmentCharCount;

    public OutputDebugStringReader(Int32 fragmentCharCount)
    {
      Verify.GreaterThanZero(fragmentCharCount);

      _fragmentCharCount = fragmentCharCount;
    }

    public IEnumerable<OutputDebugString> GetOutputDebugStrings(Byte[] data)
    {
      
      var fragment = new OutputDebugString(data);
      if (NewMessageFragment(fragment))
        yield return FlushOutputDebugString();

      if (String.IsNullOrWhiteSpace(fragment.Message))
        yield break;

      OutputDebugString += fragment;

      if (LastFragment(fragment) || ExceedsMaxMessageLength(OutputDebugString))
        yield return FlushOutputDebugString();
    }

    private Boolean NewMessageFragment(OutputDebugString fragment)
    {
      return OutputDebugString != null && (OutputDebugString.ProcessId != fragment.ProcessId || String.IsNullOrEmpty(fragment.Message));
    }

    private Boolean ExceedsMaxMessageLength(OutputDebugString fragment)
    {
      // HACK: As we are forced to rely on a FULL buffer denoting a message fragment, guard against all messages
      //       being exactly _fragmentCharCount. In the case of OutputDebugString we will cap at 64K before we
      //       force flush the message regardless. 
      return fragment.Message.Length >= (_fragmentCharCount * 16);
    }

    private Boolean LastFragment(OutputDebugString fragment)
    {
      // HACK: Unfortunately, the good folks who wrote the implementation of OutputDebugString did not feel 
      //       compelled to include a message length in the serialized data. As such, let's assume that any 
      //       message that exactly fills the buffer is likely just a fragment provided the next message comes
      //       from the same process. This may be a lie, but it is far more likely that a message overflows the
      //       buffer than exactly fills the buffer (worst case: formatting is lost - no data will be lost).

      return fragment.Message.Length < _fragmentCharCount;
    }

    private OutputDebugString FlushOutputDebugString()
    {
      var result = OutputDebugString;

      OutputDebugString = null;

      return result;
    }
  }
}
