using System;
using System.Diagnostics;

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

namespace Harvester.Core.Logging
{
  public class Logger : ILog
  {
    private readonly TraceSource _traceSource;

    public Logger(TraceSource traceSource)
    {
      Verify.NotNull(traceSource);

      _traceSource = traceSource;
    }

    public void Fatal(String message)
    {
      Trace(TraceEventType.Critical, message, null);
    }

    public void Fatal(String message, Exception ex)
    {
      Trace(TraceEventType.Critical, message, ex);
    }

    public void FatalFormat(String format, params Object[] args)
    {
      TraceFormat(TraceEventType.Critical, format, args);
    }

    public void Error(String message)
    {
      Trace(TraceEventType.Error, message, null);
    }

    public void Error(String message, Exception ex)
    {
      Trace(TraceEventType.Error, message, ex);
    }

    public void ErrorFormat(String format, params Object[] args)
    {
      TraceFormat(TraceEventType.Error, format, args);
    }

    public void Warn(String message)
    {
      Trace(TraceEventType.Warning, message, null);
    }

    public void Warn(String message, Exception ex)
    {
      Trace(TraceEventType.Warning, message, ex);
    }

    public void WarnFormat(String format, params Object[] args)
    {
      TraceFormat(TraceEventType.Warning, format, args);
    }

    public void Info(String message)
    {
      Trace(TraceEventType.Information, message, null);
    }

    public void Info(String message, Exception ex)
    {
      Trace(TraceEventType.Information, message, ex);
    }

    public void InfoFormat(String format, params Object[] args)
    {
      TraceFormat(TraceEventType.Information, format, args);
    }

    public void Debug(String message)
    {
      Trace(TraceEventType.Verbose, message, null);
    }

    public void Debug(String message, Exception ex)
    {
      Trace(TraceEventType.Verbose, message, ex);
    }

    public void DebugFormat(String format, params Object[] args)
    {
      TraceFormat(TraceEventType.Verbose, format, args);
    }

    private void Trace(TraceEventType eventType, String message, Exception ex)
    {
      if (!_traceSource.Switch.ShouldTrace(eventType))
        return;

      String trimmedMessage = (message ?? String.Empty).Trim();
      if (ex != null)
        trimmedMessage += Environment.NewLine + ex;

      _traceSource.TraceEvent(eventType, 0, trimmedMessage);
    }

    private void TraceFormat(TraceEventType eventType, String message, params Object[] args)
    {
      if (!_traceSource.Switch.ShouldTrace(eventType))
        return;

      _traceSource.TraceEvent(eventType, 0, String.Format(message, args));
    }
  }
}
