using System;
using Harvester.Core;
using Harvester.Core.Logging;
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

namespace Harvester
{
  public class LoggerWrapper : ILog
  {
    private readonly Logger _log;

    public LoggerWrapper(Logger log)
    {
      Verify.NotNull(log);

      _log = log;
    }

    public void Fatal(Object value)
    {
      _log.Fatal(value);
    }

    public void Fatal(String message)
    {
      _log.Fatal(message);
    }

    public void Fatal(String format, params Object[] args)
    {
      _log.Fatal(format, args);
    }

    public void Error(Object value)
    {
      _log.Error(value);
    }

    public void Error(String message)
    {
      _log.Error(message);
    }

    public void Error(String format, params Object[] args)
    {
      _log.Error(format, args);
    }

    public void Warn(Object value)
    {
      _log.Warn(value);
    }

    public void Warn(String message)
    {
      _log.Warn(message);
    }

    public void Warn(String format, params Object[] args)
    {
      _log.Warn(format, args);
    }

    public void Info(String message)
    {
      _log.Info(message);
    }

    public void Info(String format, params Object[] args)
    {
      _log.Info(format, args);
    }

    public void Debug(String message)
    {
      _log.Debug(message);
    }

    public void Debug(String format, params Object[] args)
    {
      _log.Debug(format, args);
    }

    public void Trace(String message)
    {
      _log.Trace(message);
    }

    public void Trace(String format, params Object[] args)
    {
      _log.Trace(format, args);
    }
  }
}