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
  public class WhenCreatingFromRawBytes
  {
    [Fact]
    public void GracefullyHandleNullyByteArray()
    {
      var outputDebugString = new OutputDebugString(null);

      Assert.Equal(0, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandleEmptyByteArray()
    {
      var outputDebugString = new OutputDebugString(new Byte[0]);

      Assert.Equal(0, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandlePartialProcessId()
    {
      var outputDebugString = new OutputDebugString(new Byte[] { 1, 2 });

      Assert.Equal(0, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandleMissingMessage()
    {
      var outputDebugString = new OutputDebugString(BitConverter.GetBytes(1234));

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandleNullMessage()
    {
      var rawData = GetRawData(BitConverter.GetBytes(1234), new Byte[] { 0 });
      var outputDebugString = new OutputDebugString(rawData);

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal(String.Empty, outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandleSingleByteMessageWithMissingNullByteTerminator()
    {
      var rawData = GetRawData(BitConverter.GetBytes(1234), new Byte[] { 65 });
      var outputDebugString = new OutputDebugString(rawData);

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal("A", outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandleSingleByteMessageWithNullByteTerminatorFollowedByOtherData()
    {
      var rawData = GetRawData(BitConverter.GetBytes(1234), new Byte[] { 65, 0, 66 });
      var outputDebugString = new OutputDebugString(rawData);

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal("A", outputDebugString.Message);
    }

    [Fact]
    public void GracefullyHandleMultiByteMessage()
    {
      var rawData = GetRawData(BitConverter.GetBytes(1234), new Byte[] { 65, 66, 0 });
      var outputDebugString = new OutputDebugString(rawData);

      Assert.Equal(1234, outputDebugString.ProcessId);
      Assert.Equal("AB", outputDebugString.Message);
    }

    private static Byte[] GetRawData(Byte[] processIdBytes, Byte[] messageBytes)
    {
      var rawData = new Byte[processIdBytes.Length + messageBytes.Length];

      Buffer.BlockCopy(processIdBytes, 0, rawData, 0, processIdBytes.Length);
      Buffer.BlockCopy(messageBytes, 0, rawData, processIdBytes.Length, messageBytes.Length);

      return rawData;
    }
  }
}
