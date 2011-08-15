using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Harvester.Core;
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
  static class Program
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void Main(String[] args)
    {
      Boolean onlyInstance;

      Thread.CurrentThread.Name = "Main";

      using (new Mutex(true, "Harvester", out onlyInstance))
      {
        if (onlyInstance)
          StartApplication();
        else
          ExitApplication();
      }
    }

    private static void ExitApplication()
    {
      Log.Warn("Harvester session already active.");

      Console.WriteLine(Localization.DebuggerAlreadyActive);
      Console.ReadKey();
    }

    private static void StartApplication()
    {
      Log.Info("Opening Harvester session.");

      try
      {
        LogEnvironmentInformation();

        using (new WindowsMonitor(new ConsoleRenderer()))
          new ManualResetEvent(false).WaitOne();
      }
      catch (Exception ex)
      {
        Log.Fatal(ex);

        Console.BackgroundColor = ConsoleColor.Red;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(ex.ToString());
      }
    }

    public static void LogEnvironmentInformation()
    {
      var startupInfo = new StringBuilder();

      AppDomain.CurrentDomain.AssemblyLoad += (sender, e) => Log.Info("Assembly Loaded: " + e.LoadedAssembly.GetName().FullName);
      AppDomain.CurrentDomain.UnhandledException += (sender, e) => Log.Fatal(e.ExceptionObject.ToString());

      startupInfo.AppendLine("Harvester (Console) v" + Assembly.GetExecutingAssembly().GetName().Version);
      startupInfo.AppendLine("OS Version:\t\t" + Environment.OSVersion);
      startupInfo.AppendLine("Framework Version:\t" + Environment.Version);
      startupInfo.AppendLine();
      startupInfo.AppendLine("Loaded Assemblies:");
      foreach (var loadedAssemblyName in AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetName().FullName).OrderBy(name => name.ToLowerInvariant()))
        startupInfo.AppendLine('-' + loadedAssemblyName);

      Log.Info(startupInfo.ToString());
    }
  }
}
