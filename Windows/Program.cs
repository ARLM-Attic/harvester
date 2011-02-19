using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Harvester.Core;
using Harvester.Windows.Forms;
using Harvester.Core.Logging;
using System.Threading;

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

namespace Harvester.Windows
{
  static class Program
  {
    [STAThread]
    static void Main(String[] args)
    {
      try
      {
        Thread.CurrentThread.Name = "Main";
        LogManager.Initialize(ConfigureLogLevel(args));

        LogEnvironmentInformation();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Main());
      }
      catch (Win32Exception ex)
      {
        MessageBox.Show(ex.NativeErrorCode == Core.Win32.Basic.BasicApi.ErrorAlreadyExists ? Localization.DebuggerAlreadyActive : ex.Message, Application.ProductName);
      }
      catch (Exception ex)
      {
        LogManager.CreateClassLogger().Fatal(ex.Message, ex);
        MessageBox.Show(ex.Message, Application.ProductName);
      }
    }

    private static SourceLevels ConfigureLogLevel(String[] args)
    {
      return args != null && args.Length > 0 && String.Compare("-trace", args[0], StringComparison.OrdinalIgnoreCase) == 0
               ? SourceLevels.All
               : SourceLevels.Information;
    }

    private static void LogEnvironmentInformation()
    {
      var log = LogManager.CreateClassLogger();
      var startupInfo = new StringBuilder();

      AppDomain.CurrentDomain.AssemblyLoad += (sender, e) => log.Info("Assembly Loaded: " + e.LoadedAssembly.GetName().FullName);

      startupInfo.AppendLine("Application Start");
      startupInfo.AppendLine("************************************************************************************************************************");
      startupInfo.AppendLine(Application.ProductName + " v" + Application.ProductVersion);
      startupInfo.AppendLine();
      startupInfo.AppendLine("MachineName:\t\t" + Environment.MachineName);
      startupInfo.AppendLine("Processor Count:\t" + Environment.ProcessorCount);
      startupInfo.AppendLine("OS Version:\t\t" + Environment.OSVersion);
      startupInfo.AppendLine("Framework Version:\t" + Environment.Version);

      using (var process = Process.GetCurrentProcess())
      {
        startupInfo.AppendLine("Process ID:\t\t" + process.Id);
        startupInfo.AppendLine("Process Name:\t\t" + process.ProcessName);
      }

      startupInfo.AppendLine();
      startupInfo.AppendLine("Loaded Assemblies:");
      foreach (var loadedAssemblyName in AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetName().FullName).OrderBy(assemblyName => assemblyName.ToLowerInvariant()))
        startupInfo.AppendLine('-' + loadedAssemblyName);

      startupInfo.Append("************************************************************************************************************************");

      log.Info(startupInfo.ToString());
    }
  }
}
