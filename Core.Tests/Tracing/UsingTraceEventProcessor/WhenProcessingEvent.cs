using System;
using System.Threading;
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

namespace Harvester.Core.Tests.Tracing.UsingTraceEventProcessor
{
  public class WhenProcessingEvent : TraceEventProcessorTestBase
  {
    [Fact]
    public void DeferEventProcessing()
    {
      var blockingQueue = new Mock<IBlockingQueue<TraceEvent>>();
      var traceEvent = new TraceEvent(1, "Message", "Source");

      using(var processor = new TraceEventProcessor(blockingQueue.Object, LogMessageFactory))
        processor.ProcessEvent(traceEvent);

      blockingQueue.Verify(mock => mock.Enqueue(traceEvent), Times.Once());
    }

    [Fact]
    public void GracefullyHandleNoEventHandlers()
    {
      var attempsRemaining = 5;

      Processor.ProcessEvent(new TraceEvent(1, "Message", "Source"));

      while (BlockingQueue.Count > 0 && --attempsRemaining > 0)
        Thread.Sleep(25);
        
      Assert.True(attempsRemaining > 0);
      Assert.Equal(0, BlockingQueue.Count);
    }

    [Fact]
    public void EventProcessedOnSeparateThread()
    {
      Int32 threadId = Thread.CurrentThread.ManagedThreadId;

      Processor.TraceEventsProcessed += (sender, e) =>
                                          {
                                            threadId = Thread.CurrentThread.ManagedThreadId;
                                            ManualSyncEvent.Set();
                                          };

      Processor.ProcessEvent(new TraceEvent(1, "Message", "Source"));
      ManualSyncEvent.WaitOne();

      Assert.NotEqual(Thread.CurrentThread.ManagedThreadId, threadId);
    }
  }
}
