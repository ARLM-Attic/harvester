using System;
using System.Security.Principal;
using Harvester.Core.Messages;
using Harvester.Core.Tracing;

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

namespace Harvester.Core
{
  public class WindowsMonitor : IDisposable
  {
    private readonly ITraceListener _dbWinLocal;
    private readonly ITraceListener _dbWinGlobal;
    private readonly ITraceListener _harvesterLocal;
    private readonly ITraceListener _harvesterGlobal;
    private readonly ITraceEventProcessor _traceEventProcessor;

    public WindowsMonitor(ILogMessageRenderer renderer)
    {
      var identity = WindowsIdentity.GetCurrent() ?? WindowsIdentity.GetAnonymous();
      var principal = new WindowsPrincipal(identity);

      _traceEventProcessor = new TraceEventProcessor();
      _traceEventProcessor.TraceEventsProcessed += (sender, e) => renderer.Render(e.LogMessages);

      _dbWinLocal = GetLocalDbWinListener();
      _harvesterLocal = GetLocalHarvesterListener();

      _dbWinGlobal = GetGlobalDbWinListener(principal);
      _harvesterGlobal = GetGlobalHarvesterListener(principal);
    }

    public void Dispose()
    {
      _dbWinLocal.Dispose();
      _harvesterLocal.Dispose();
      _dbWinGlobal.Dispose();
      _harvesterGlobal.Dispose();

      _traceEventProcessor.Dispose();
    }

    private ITraceListener GetLocalDbWinListener()
    {
      var listener = new OutputDebugStringListener("Local.OutputDebugString", "DBWinMutex", "Local\\DBWIN");
      
      listener.TraceEventReceived += HandleTraceEventReceived;

      return listener;
    }

    private ITraceListener GetGlobalDbWinListener(WindowsPrincipal principal)
    {
      if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
        return new NullListener();

      var listener = new OutputDebugStringListener("Local.OutputDebugString", "DBWinMutex", "Local\\DBWIN");
      
      listener.TraceEventReceived += HandleTraceEventReceived;

      return listener;
    }

    private ITraceListener GetLocalHarvesterListener()
    {
      return new NullListener();
    }

    private ITraceListener GetGlobalHarvesterListener(WindowsPrincipal principal)
    {
      return new NullListener();
    }

    private void HandleTraceEventReceived(Object sender, TraceEventArgs e)
    {
      _traceEventProcessor.ProcessEvent(e.Event);
    }
  }
}
