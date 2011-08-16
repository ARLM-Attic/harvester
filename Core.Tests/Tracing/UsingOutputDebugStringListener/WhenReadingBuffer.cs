using System;
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

namespace Harvester.Core.Tests.Tracing.UsingOutputDebugStringListener
{
  public class WhenReadingBuffer : OutputDebugStringListenerTestBase
  {
    private readonly Mock<IEventHandler> _eventHandler = new Mock<IEventHandler>();

    [Fact]
    public void IgnoreNullOrEmptyMessage()
    {
      Listener.TraceEventReceived += _eventHandler.Object.HandleEvent;

      Buffer.WaitBufferReady();
      Buffer.SignalDataReady(new OutputDebugString(1234, String.Empty));
      Buffer.WaitBufferReady();

      _eventHandler.Verify(mock => mock.HandleEvent(Listener, It.IsAny<TraceEventArgs>()), Times.Never());
    }

    [Fact]
    public void RaiseTraceEventReceivedWhenMessageNotNullOrEmpty()
    {
      Listener.TraceEventReceived += _eventHandler.Object.HandleEvent;

      Buffer.WaitBufferReady();
      Buffer.SignalDataReady(new OutputDebugString(1234, "A test message!"));
      Buffer.WaitBufferReady();

      _eventHandler.Verify(mock => mock.HandleEvent(Listener, It.Is<TraceEventArgs>(e => e.Event.ProcessId == 1234 && e.Event.Message == "A test message!")), Times.Once());
    }

    public interface IEventHandler
    {
      void HandleEvent(Object sender, TraceEventArgs e);
    }
  }
}
