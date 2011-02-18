using System;
using System.Collections.Generic;
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
  public class ProcessRetriever : IProcessRetriever
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly IDictionary<Int32, IProcess> _processCache = new Dictionary<Int32, IProcess>();

    public IProcess GetProcessById(Int32 processId)
    {
      Log.DebugFormat("Atempting to get process by id: {0}.", processId);

      Verify.GreaterThanZero(processId);

      lock (_processCache)
      {
        IProcess process;

        if (_processCache.TryGetValue(processId, out process) && !process.HasExited)
          return process;

        Log.Debug("Process not found in cache; retrieving process information.");

        try
        {
          _processCache[processId] = process = new ProcessWrapper(Process.GetProcessById(processId));
        }
        catch (InvalidOperationException ex) { Log.Warn(ex.Message, ex); }
        catch (ArgumentException ex) { Log.Warn(ex.Message, ex); }

        return process ?? new UnknownProcess(processId);
      }
    }
  }
}
