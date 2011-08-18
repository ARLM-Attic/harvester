using System;
using Harvester.Core.Tracing;
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

namespace Harvester.Core.Tests.Tracing.UsingOutputDebugString
{
  public class WhenCombiningOutputDebugStrings
  {
    [Fact]
    public void CanAddTwoOutputDebugStringsTogether()
    {
      var result = new OutputDebugString(1, "LHS Message Part <-") + new OutputDebugString(1, "-> RHS Message Part");

      Assert.Equal(1, result.ProcessId);
      Assert.Equal("LHS Message Part <--> RHS Message Part", result.Message);
    }

    [Fact]
    public void CanAddOutputDebugStringWithNulLValue()
    {
      var result = new OutputDebugString(1, "LHS Message Part") + null;

      Assert.Equal(1, result.ProcessId);
      Assert.Equal("LHS Message Part", result.Message);
    }

    [Fact]
    public void CanAddNullValueWithOutputDebugString()
    {
      var result = null + new OutputDebugString(1, "RHS Message Part");

      Assert.Equal(1, result.ProcessId);
      Assert.Equal("RHS Message Part", result.Message);
    }

    [Fact]
    public void CannotAddOutputDebugStringsFromDifferentProcesses()
    {
      Assert.Throws<InvalidOperationException>(() => new OutputDebugString(1, "LHS Message Part <-") + new OutputDebugString(2, "-> RHS Message Part"));
    }
  }
}
