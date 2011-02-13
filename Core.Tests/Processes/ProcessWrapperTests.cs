using System;
using System.Diagnostics;
using Harvester.Core.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

namespace Harvester.Core.Tests.Processes
{
  [TestClass]
  public class ProcessWrapperTests
  {
    [TestMethod]
    public void ProcessWrapperWrapDiagnositcProcess()
    {
      using (var process = Process.GetCurrentProcess())
      {
        var processWrapper = new ProcessWrapper(process);

        Assert.AreEqual(process.Id, processWrapper.Id);
        Assert.AreEqual(process.ProcessName, processWrapper.Name);
        Assert.IsFalse(processWrapper.HasExited);
        Assert.IsNull(processWrapper.ExitTime);
      }
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException), "Process has exited, so the requested information is not available.")]
    public void ProcessShouldThrowInvalidOperationExceptionIfWrappingExitedProcess()
    {
      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        process.Kill();
        process.WaitForExit();

        Assert.IsTrue(process.HasExited);
        
        new ProcessWrapper(process);
      }
    }

    [TestMethod]
    public void ProcessShouldSetExitTimeWhenHasExitedChecked()
    {
      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        var processWrapper = new ProcessWrapper(process);

        process.Kill();
        process.WaitForExit();

        Assert.IsNull(processWrapper.ExitTime);
        Assert.IsFalse(processWrapper.HasExited);
        Assert.IsTrue(processWrapper.HasExited);
        Assert.IsNotNull(processWrapper.ExitTime);
      }
    }
  }
}
