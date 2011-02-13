using System;
using System.Diagnostics;
using System.Threading;
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
  public class ProcessRetrieverTests
  {
    [TestMethod]
    public void GetProcessByIdReturnsRunningNewProcessReferenceWhenNotFound()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.GetCurrentProcess())
      {
        var referencedProcess = processRetriever.GetProcessById(process.Id);

        Assert.AreEqual(process.Id, referencedProcess.Id);
        Assert.AreEqual(process.ProcessName, referencedProcess.Name);
        Assert.IsFalse(referencedProcess.HasExited);
        Assert.IsNull(referencedProcess.ExitTime);
      }
    }

    [TestMethod]
    public void GetProcessByIdReturnsRunningSameProcessReferenceWhenFoundAndNotExited()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.GetCurrentProcess())
      {
        var referencedProcess1 = processRetriever.GetProcessById(process.Id);
        var referencedProcess2 = processRetriever.GetProcessById(process.Id);

        Assert.AreSame(referencedProcess1, referencedProcess2);
      }
    }

    [TestMethod]
    public void GetProcessByIdReturnsUnknownProcessWhenProcessIdNotFound()
    {
      var processRetriever = new ProcessRetriever();

      var referencedProcess = processRetriever.GetProcessById(9999999);

      Assert.AreEqual(9999999, referencedProcess.Id);
      Assert.AreEqual(String.Empty, referencedProcess.Name);
      Assert.IsTrue(referencedProcess.HasExited);
      Assert.IsNotNull(referencedProcess.ExitTime);
    }

    [TestMethod]
    public void GetProcessByIdReturnsUnknownProcessWhenProcessExitedBeforeWrapped()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        var processId = process.Id;

        process.Kill();
        process.WaitForExit();

        var referencedProcess = processRetriever.GetProcessById(processId);

        Assert.AreEqual(processId, referencedProcess.Id);
        Assert.AreEqual(String.Empty, referencedProcess.Name);
        Assert.IsTrue(referencedProcess.HasExited);
        Assert.IsNotNull(referencedProcess.ExitTime);
      }
    }
    
    [TestMethod]
    public void GetProcessByIdReturnsCachedProcessReferenceIfProcessExits()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        var referencedProcess1 = processRetriever.GetProcessById(process.Id);

        process.Kill();
        process.WaitForExit();

        var referencedProcess2 = processRetriever.GetProcessById(process.Id);

        Assert.AreEqual(referencedProcess1.Id, referencedProcess2.Id);
        Assert.AreEqual(referencedProcess1.Name, referencedProcess2.Name);
        Assert.IsTrue(referencedProcess2.HasExited);
        Assert.IsNotNull(referencedProcess2.ExitTime);
      }
    }

  }
}
