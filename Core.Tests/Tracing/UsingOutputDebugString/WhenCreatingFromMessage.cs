using System;
using System.Text;
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
  public class WhenCreatingFromMessage
  {
    [Fact]
    public void GracefullyHandleNullMessage()
    {
      var outputDebugString = new OutputDebugString(1234, null);

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
      Assert.Equal("0gQAAAA=", Convert.ToBase64String(outputDebugString));
    }

    [Fact]
    public void GracefullyHandleEmptyMessage()
    {
      var outputDebugString = new OutputDebugString(1234, String.Empty);

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
      Assert.Equal("0gQAAAA=", Convert.ToBase64String(outputDebugString));
    }

    [Fact]
    public void RawDataStartsWithProcessId()
    {
      var outputDebugString = new OutputDebugString(1234, "I will be terminated with a NULL byte.");
      var rawData = (Byte[])outputDebugString;

      Assert.Equal(1234, BitConverter.ToInt32(rawData, 0));
    }

    [Fact]
    public void RawDataContainsAsciiMessage()
    {
      var outputDebugString = new OutputDebugString(1234, "I will be terminated with a NULL byte.");
      var rawData = (Byte[])outputDebugString;

      Assert.Equal("I will be terminated with a NULL byte.", Encoding.ASCII.GetString(rawData, sizeof(Int32), rawData.Length - sizeof(Int32) - 1));
    }

    [Fact]
    public void RawDataEndsWithNullByte()
    {
      var outputDebugString = new OutputDebugString(1234, "I will be terminated with a NULL byte.");
      var rawData = (Byte[])outputDebugString;

      Assert.Equal(0, rawData[rawData.Length - 1]);
    }
  }
}
