using System;
using System.Diagnostics;
using Harvester.Core.Logging;

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
  public class ProcessWrapper : IProcess
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly String _processName;
    private readonly Int32 _processId;
    private DateTime? _exitTime;
    private Process _process;

    public Int32 Id { get { return _processId; } }
    public String Name { get { return _processName; } }
    public DateTime? ExitTime { get { return _exitTime; } }
    public Boolean HasExited { get { return _process == null ? true : _process.HasExited; } }

    public ProcessWrapper(Process process)
    {
      Log.DebugFormat("Creating process wrapper: {0}.", process.Id);

      Verify.NotNull(process);

      _process = process;
      _process.Exited += OnProcessExited;

      _processId = process.Id;
      _processName = process.ProcessName;
    }

    private void OnProcessExited(Object sender, EventArgs e)
    {
      Log.DebugFormat("Process exited: {0}.", _processId);

      _exitTime = DateTime.Now;

      _process.Exited -= OnProcessExited;
      _process.Dispose();
      _process = null;
    }
  }
}
