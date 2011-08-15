using System;
using NLog;

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

namespace Harvester.Core.Processes
{
  internal class UnknownProcess : IProcess
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly Int32 _processId;
    private readonly DateTime _exitTime;
    
    public Int32 Id { get { return _processId; } }
    public String Name { get { return String.Empty; } }
    public DateTime? ExitTime { get { return _exitTime; } }
    public Boolean HasExited { get { return true; } }

    public UnknownProcess(Int32 processId)
    {
      Log.Debug("Creating process wrapper: {0}.", processId);

      Verify.GreaterThanZero(processId);

      _processId = processId;
      _exitTime = DateTime.Now;
    }
  }
}
