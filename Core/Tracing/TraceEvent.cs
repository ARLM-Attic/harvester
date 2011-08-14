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

namespace Harvester.Core.Tracing
{
  public class TraceEvent
  {
    private readonly DateTime _timestamp;
    private readonly Int32 _processId;
    private readonly String _message;
    private readonly String _source;

    public DateTime Timestamp { get { return _timestamp; } }
    public Int32 ProcessId { get { return _processId; } }
    public String Message { get { return _message; } }
    public String Source { get { return _source; } }

    public TraceEvent(Int32 processId, String message, String source)
    {
      Verify.GreaterThanZero(processId);
      Verify.NotWhitespace(message);
      Verify.NotWhitespace(source);

      _timestamp = DateTime.Now;
      _processId = processId;
      _message = message;
      _source = source;
    }
  }
}
