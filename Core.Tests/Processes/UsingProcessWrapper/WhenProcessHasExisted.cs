﻿using System;
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

namespace Harvester.Core.Tests.Processes.UsingProcessWrapper
{
  public class WhenProcessHasExisted
  {
    [Fact]
    public void ThrowInvalidOperationExceptionIfWrappingProcess()
    {
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

        Assert.True(process.HasExited);

        var ex = Assert.Throws<InvalidOperationException>(() => new ProcessWrapper(process));
        Assert.Equal("Process has exited, so the requested information is not available.", ex.Message);
      }
    }

    [Fact]
    public void SetExitTimeWhenHasExitedChecked()
    {
      using (var process = Process.Start(new ProcessStartInfo("cmd") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden }))
      {
        var processWrapper = new ProcessWrapper(process);

        process.Kill();
        process.WaitForExit();

        Assert.Null(processWrapper.ExitTime);
        Assert.False(processWrapper.HasExited);
        Assert.True(processWrapper.HasExited);
        Assert.NotNull(processWrapper.ExitTime);
      }
    }
  }
}