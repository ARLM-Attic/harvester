using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Harvester.Core.Tests.Threading.UsingBlockingQueue
{
  public class WhenTringToDequeueItem : BlockingQueueTestBase
  {
    [Fact]
    public void ReturnTrueIfItemDequeuedWithInifinitTimeout()
    {
      Object item1 = String.Empty;
      Object item2;

      UnderlyingQueue.Enqueue(item1);

      Assert.True(BlockingQueue.TryDequeue(out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void ReturnTrueIfItemDequeuedWithTimespanTimeout()
    {
      Object item1 = String.Empty;
      Object item2;

      UnderlyingQueue.Enqueue(item1);

      Assert.True(BlockingQueue.TryDequeue(TimeSpan.FromMilliseconds(10), out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void ReturnTrueIfItemDequeuedWithMsTimeout()
    {
      Object item1 = String.Empty;
      Object item2;

      UnderlyingQueue.Enqueue(item1);

      Assert.True(BlockingQueue.TryDequeue(Timeout.Infinite, out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void ReturnFalseIfQueueDisposed()
    {
      Object item1 = String.Empty;
      Object item2;

      Monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      UnderlyingQueue.Enqueue(item1);
      BlockingQueue.Dispose();

      Assert.False(BlockingQueue.TryDequeue(Timeout.Infinite, out item2));
      Assert.Null(item2);
    }

    [Fact]
    public void BlockUntilItemAdded()
    {
      Object item1 = String.Empty;
      Object item2;

      Monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => UnderlyingQueue.Enqueue(item1));

      Assert.True(BlockingQueue.TryDequeue(Timeout.Infinite, out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void BlockUntilQueueDisposed()
    {
      Object item;

      Monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      Monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => BlockingQueue.Dispose());

      Assert.False(BlockingQueue.TryDequeue(Timeout.Infinite, out item));
      Assert.Null(item);
    }
  }
}
