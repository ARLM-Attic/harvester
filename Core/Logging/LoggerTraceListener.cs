using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.Logging;

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
  public class LoggerTraceListener : FileLogTraceListener
  {
    private static readonly Int32 MaxTypeNameLength;
    public static readonly LoggerTraceListener Instance;

    static LoggerTraceListener()
    {
      MaxTypeNameLength = Convert.ToInt32(Math.Ceiling((typeof(ILog).Assembly.GetTypes().Max(type => (type.FullName ?? String.Empty).Length) / 5.0)) * 5);
      Instance = new LoggerTraceListener();
    }

    private LoggerTraceListener()
    {
      Append = true;
      AutoFlush = true;
      BaseFileName = "Harvester";
      Location = LogFileLocation.LocalUserApplicationDirectory;
      LogFileCreationSchedule = LogFileCreationScheduleOption.None;
    }

    public static void PurgeOldLogFiles()
    {
      String fileName;
      using (var listener = new LoggerTraceListener())
        fileName = listener.FullLogFileName;

      var directory = Path.GetDirectoryName(fileName);
      if (String.IsNullOrEmpty(directory))
        return;

      var files = Directory.GetFiles(directory, Path.GetFileNameWithoutExtension(fileName) + "*" + Path.GetExtension(fileName));
      foreach(var file in files)
      {
        if (File.Exists(file))
          File.Delete(file);
      }
    }

    public override void TraceEvent(TraceEventCache eventCache, String source, TraceEventType eventType, Int32 id, String message)
    {
      var logMessage = new StringBuilder();

      logMessage.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
      logMessage.Append(" [");
      switch (eventType)
      {
        case TraceEventType.Critical: logMessage.Append("FATAL"); break;
        case TraceEventType.Error: logMessage.Append("ERROR"); break;
        case TraceEventType.Warning: logMessage.Append("WARN "); break;
        case TraceEventType.Information: logMessage.Append("INFO "); break;
        default: logMessage.Append("DEBUG"); break;
      }
      logMessage.Append("] [");
      logMessage.Append(source.PadRight(MaxTypeNameLength, ' '));
      logMessage.Append("] [");
      logMessage.Append(Thread.CurrentThread.Name.PadRight(20, ' ').Substring(0, 16));
      logMessage.Append("] ");
      logMessage.Append(message);

      WriteLine(logMessage.ToString());
    }

    public string PathFullLogFileName { get; set; }
  }
}
