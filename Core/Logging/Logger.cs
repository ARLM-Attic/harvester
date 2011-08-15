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

namespace Harvester.Core.Logging
{
  public class Logger : ILog
  {
    private readonly log4net.ILog _logger;

    public Logger(log4net.ILog logger)
    {
      Verify.NotNull(logger);

      _logger = logger;
    }

    public void Fatal(String message)
    {
      _logger.Fatal(message);
    }

    public void Fatal(String message, Exception ex)
    {
      _logger.Fatal(message, ex);
    }

    public void FatalFormat(String format, params Object[] args)
    {
      _logger.FatalFormat(format, args);
    }

    public void Error(String message)
    {
      _logger.Error(message);
    }

    public void Error(String message, Exception ex)
    {
      _logger.Error(message, ex);
    }

    public void ErrorFormat(String format, params Object[] args)
    {
      _logger.ErrorFormat(format, args);
    }

    public void Warn(String message)
    {
      _logger.Warn(message);
    }

    public void Warn(String message, Exception ex)
    {
      _logger.Warn(message, ex);
    }

    public void WarnFormat(String format, params Object[] args)
    {
      _logger.WarnFormat(format, args);
    }

    public void Info(String message)
    {
      _logger.Info(message);
    }

    public void InfoFormat(String format, params Object[] args)
    {
      _logger.InfoFormat(format, args);
    }

    public void Debug(String message)
    {
      _logger.Debug(message);
    }

    public void DebugFormat(String format, params Object[] args)
    {
      _logger.DebugFormat(format, args);
    }
  }
}
