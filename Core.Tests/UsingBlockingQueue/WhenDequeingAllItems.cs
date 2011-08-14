using System;
using System.Collections.Generic;
using System.Threading;
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

namespace Harvester.Core.Tests.UsingBlockingQueue
{
  public class WhenDequeingAllItems : BlockingQueueTestBase
  {
    [Fact]
    public void ReturnItemIfItemDequeueAlldWithInifinitTimeout()
    {
      UnderlyingQueue.Enqueue(String.Empty);

      IList<Object> list = BlockingQueue.DequeueAll();

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void ReturnItemIfItemDequeueAlldWithTimespanTimeout()
    {
      UnderlyingQueue.Enqueue(String.Empty);

      IList<Object> list = BlockingQueue.DequeueAll(TimeSpan.FromMilliseconds(10));

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void ReturnItemIfItemDequeueAlldWithMsTimeout()
    {
      UnderlyingQueue.Enqueue(String.Empty);

      IList<Object> list = BlockingQueue.DequeueAll(Timeout.Infinite);

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void ThrowExceptionIfQueueDisposed()
    {
      Monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      UnderlyingQueue.Enqueue(String.Empty);
      BlockingQueue.Dispose();

      Assert.Throws<ObjectDisposedException>(() => BlockingQueue.DequeueAll(Timeout.Infinite));
    }

    [Fact]
    public void BlockUntilItemAdded()
    {
      Monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => UnderlyingQueue.Enqueue(String.Empty));

      IList<Object> list = BlockingQueue.DequeueAll(Timeout.Infinite);

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void BlockUntilQueueDisposedAndThenThrowException()
    {
      Monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      Monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => BlockingQueue.Dispose());

      Assert.Throws<ObjectDisposedException>(() => BlockingQueue.DequeueAll(Timeout.Infinite));
    }
  }
}
