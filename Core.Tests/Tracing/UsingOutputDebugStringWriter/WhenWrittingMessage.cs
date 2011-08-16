using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Harvester.Core.Extensions;
using Harvester.Core.Tracing;
using Moq;
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

namespace Harvester.Core.Tests.Tracing.UsingOutputDebugStringWriter
{
  public class WhenWrittingMessage
  {
    private readonly Mock<IBuffer> _buffer = new Mock<IBuffer>();
    private readonly Random _randomizer = new Random();
    private readonly OutputDebugStringWriter _writer;
    private readonly String _mutexName;

    public WhenWrittingMessage()
    {
      _mutexName = _randomizer.NextString(10) + "Mutex";
      _writer = new OutputDebugStringWriter(_mutexName, _buffer.Object);

      _buffer.Setup(mock => mock.Capacity).Returns(1024);
    }

    [Fact]
    public void IgnoreIfMutexCreated()
    {
      Assert.False(_writer.Write("Some Message."));
      
      _buffer.Verify(mock => mock.Write(It.IsAny<Byte[]>()), Times.Never());
    }

    [Fact]
    public void TimeoutIfMutexDoesNotReceiveSignal()
    {
      using (var syncEvent1 = new ManualResetEvent(false))
      using (var syncEvent2 = new ManualResetEvent(false))
      {
        ThreadPool.QueueUserWorkItem(state =>
                                       {
                                         using(var mutex = new Mutex(false, _mutexName))
                                         {
                                           mutex.WaitOne();

                                           syncEvent1.Set();
                                           syncEvent2.WaitOne();

                                           mutex.ReleaseMutex();
                                         }
                                       });

        syncEvent1.WaitOne();
        
        Assert.False(_writer.Write("Some Message."));

        syncEvent2.Set();
      }

      _buffer.Verify(mock => mock.Write(It.IsAny<Byte[]>()), Times.Never());
    }

    [Fact]
    public void MessageBytesStartWithCurrentProcessId()
    {
      var processId = Process.GetCurrentProcess().Id;
      
      using(new Mutex(false, _mutexName))
        Assert.True(_writer.Write("Some Message."));

      _buffer.Verify(mock => mock.Write(It.Is<Byte[]>(value => BitConverter.ToInt32(value, 0) == processId)), Times.Once());
    }

    [Fact]
    public void MessageBytesEndWithSpecifiedMessage()
    {
      var message = _randomizer.NextString(32);

      using (new Mutex(false, _mutexName))
        Assert.True(_writer.Write(message));

      _buffer.Verify(mock => mock.Write(It.Is<Byte[]>(value => Encoding.ASCII.GetString(value, sizeof(Int32), value.Length - sizeof(Int32) - 1) == message)), Times.Once());
    }

    [Fact]
    public void MessageSplitInToMultipleChunksIfExceedsBufferCapacity()
    {
      var messagePart1 = _randomizer.NextString(1019);
      var messagePart2 = _randomizer.NextString(1019);
      var messagePart3 = _randomizer.NextString(10);

      using (new Mutex(false, _mutexName))
        Assert.True(_writer.Write(messagePart1 + messagePart2 + messagePart3));

      _buffer.Verify(mock => mock.Write(It.Is<Byte[]>(value => Encoding.ASCII.GetString(value, sizeof(Int32), value.Length - sizeof(Int32) - 1) == messagePart1)), Times.Once());
      _buffer.Verify(mock => mock.Write(It.Is<Byte[]>(value => Encoding.ASCII.GetString(value, sizeof(Int32), value.Length - sizeof(Int32) - 1) == messagePart2)), Times.Once());
      _buffer.Verify(mock => mock.Write(It.Is<Byte[]>(value => Encoding.ASCII.GetString(value, sizeof(Int32), value.Length - sizeof(Int32) - 1) == messagePart3)), Times.Once());
    }
  }
}
