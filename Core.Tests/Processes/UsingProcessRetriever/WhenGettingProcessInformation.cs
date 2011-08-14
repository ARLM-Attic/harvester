using System;
using System.Diagnostics;
using Harvester.Core.Processes;
using Xunit;

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

namespace Harvester.Core.Tests.Processes.UsingProcessRetriever
{
  public class WhenGettingProcessInformation
  {
    [Fact]
    public void ReturnNewProcessReferenceIfProcessNotFound()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.GetCurrentProcess())
      {
        var referencedProcess = processRetriever.GetProcessById(process.Id);

        Assert.Equal(process.Id, referencedProcess.Id);
        Assert.Equal(process.ProcessName, referencedProcess.Name);
        Assert.False(referencedProcess.HasExited);
        Assert.Null(referencedProcess.ExitTime);
      }
    }

    [Fact]
    public void ReturnSameProcessReferenceIfProcessFoundAndNotExited()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.GetCurrentProcess())
      {
        var referencedProcess1 = processRetriever.GetProcessById(process.Id);
        var referencedProcess2 = processRetriever.GetProcessById(process.Id);

        Assert.Same(referencedProcess1, referencedProcess2);
      }
    }

    [Fact]
    public void ReturnUnknownProcessIfProcessIdNotFound()
    {
      var processRetriever = new ProcessRetriever();

      var referencedProcess = processRetriever.GetProcessById(9999999);

      Assert.Equal(9999999, referencedProcess.Id);
      Assert.Equal(String.Empty, referencedProcess.Name);
      Assert.True(referencedProcess.HasExited);
      Assert.NotNull(referencedProcess.ExitTime);
    }

    [Fact]
    public void ReturnUnknownProcessIfProcessExitedBeforeWrapped()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        var processId = process.Id;
        var exitVerified = false;

        process.Kill();
        process.WaitForExit();

        // Wait For Exit occassionally lies.
        while (!exitVerified)
          try { Process.GetProcessById(processId); }
          catch (ArgumentException) { exitVerified = true; }

        var referencedProcess = processRetriever.GetProcessById(processId);

        Assert.Equal(processId, referencedProcess.Id);
        Assert.Equal(String.Empty, referencedProcess.Name);
        Assert.True(referencedProcess.HasExited);
        Assert.NotNull(referencedProcess.ExitTime);
      }
    }

    [Fact]
    public void ReturnCachedProcessReferenceIfProcessExits()
    {
      var processRetriever = new ProcessRetriever();

      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        var referencedProcess1 = processRetriever.GetProcessById(process.Id);

        process.Kill();
        process.WaitForExit();

        var referencedProcess2 = processRetriever.GetProcessById(process.Id);

        Assert.Equal(referencedProcess1.Id, referencedProcess2.Id);
        Assert.Equal(referencedProcess1.Name, referencedProcess2.Name);
        Assert.True(referencedProcess2.HasExited);
        Assert.NotNull(referencedProcess2.ExitTime);
      }
    }
  }
}
