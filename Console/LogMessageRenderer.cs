using System;
using System.Collections.Generic;
using System.Text;
using Harvester.Core;
using Harvester.Core.Logging;
using Harvester.Core.Messages;

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
  internal class ConsoleRenderer : ILogMessageRenderer
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly StringBuilder _stringBuilder = new StringBuilder();

    private Int32 MaxSourceLength { get; set; }
    private Int32 MaxProcessNameLength { get; set; }
    private Int32 MaxProcessIdLength { get; set; }
    private Int32 MaxThreadLength { get; set; }
    
    public void Render(IEnumerable<ILogMessage> logMessages)
    {
      Log.Debug("Rendering log messages.");

      foreach (var logMessage in logMessages)
        RenderLogMessage(logMessage);
    }

    private void RenderLogMessage(ILogMessage logMessage)
    {
      SetConsoleBackgroundColor(logMessage);
      SetConsoleForegroundColor(logMessage);

      _stringBuilder.Clear();
      _stringBuilder.AppendFormat("{0:yyyy-MM-dd HH:mm:ss,fff}", logMessage.Timestamp);
      _stringBuilder.AppendFormat(" [{0}]", GetProcessId(logMessage));
      _stringBuilder.AppendFormat(" {0}", GetProcessName(logMessage));
      _stringBuilder.AppendFormat(" [{0}]", GetThread(logMessage));
      _stringBuilder.AppendFormat(" {0} ", GetSource(logMessage));
      _stringBuilder.AppendFormat(" [{0}] ", GetLevel(logMessage));
      _stringBuilder.Append(logMessage.Message);

      Console.WriteLine(_stringBuilder.ToString());

      _stringBuilder.Clear();
    }

    private String GetProcessId(ILogMessage logMessage)
    {
      var value = logMessage.ProcessId.ToString();

      MaxProcessIdLength = Math.Max(MaxProcessIdLength, value.Length);

      return value.PadLeft(MaxProcessIdLength, ' ');
    }

    private String GetProcessName(ILogMessage logMessage)
    {
      var value = logMessage.ProcessName ?? String.Empty;

      MaxProcessNameLength = Math.Max(MaxProcessNameLength, value.Length);

      return value.PadRight(MaxProcessNameLength, ' ');
    }

    private String GetThread(ILogMessage logMessage)
    {
      var value = logMessage.Thread ?? String.Empty;

      MaxThreadLength = Math.Max(MaxThreadLength, value.Length);

      return value.PadRight(MaxThreadLength, ' ');
    }

    private String GetSource(ILogMessage logMessage)
    {
      var value = logMessage.Source ?? String.Empty;

      MaxSourceLength = Math.Max(MaxSourceLength, value.Length);

      return value.PadRight(MaxSourceLength, ' ');
    }

    private static Char GetLevel(ILogMessage logMessage)
    {
      switch (logMessage.Level)
      {
        case LogMessageLevel.Fatal: return 'F';
        case LogMessageLevel.Error: return 'E';
        case LogMessageLevel.Warning: return 'W';
        case LogMessageLevel.Information: return 'I';
        case LogMessageLevel.Debug: return 'D';
        default: return 'T';
      }
    }
    
    private static void SetConsoleBackgroundColor(ILogMessage logMessage)
    {
      //TODO: Make Configurable
      Console.BackgroundColor = logMessage.Level == LogMessageLevel.Fatal ? ConsoleColor.Red : ConsoleColor.Black; 
    }

    private static void SetConsoleForegroundColor(ILogMessage logMessage)
    {
      //TODO: Make Configurable
      switch (logMessage.Level)
      {
        case LogMessageLevel.Fatal: Console.ForegroundColor = ConsoleColor.White; break;
        case LogMessageLevel.Error: Console.ForegroundColor = ConsoleColor.Red; break;
        case LogMessageLevel.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
        case LogMessageLevel.Information: Console.ForegroundColor = ConsoleColor.White; break;
        case LogMessageLevel.Debug: Console.ForegroundColor = ConsoleColor.DarkGray; break;
        default: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
      }
    }
  }
}
