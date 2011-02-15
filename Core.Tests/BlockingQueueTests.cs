using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;

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

namespace Harvester.Core.Tests
{
  public class BlockingQueueTests
  {
    private readonly Mock<IMonitor> _monitor = new Mock<IMonitor>(MockBehavior.Strict);
    private readonly Queue<Object> _underlyingQueue = new Queue<Object>();
    private readonly IBlockingQueue<Object> _blockingQueue;

    public BlockingQueueTests()
    {
      _blockingQueue = new BlockingQueue<object>(_underlyingQueue, _monitor.Object);
    }

    #region Ctor Facts

    [Fact]
    public void DefaultCtorCreatesEmptyQueue()
    {
      var queue = new BlockingQueue<Object>();

      Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void CapacityCtorCreatesEmptyQueue()
    {
      var queue = new BlockingQueue<Object>(10);

      Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void CopyCtorCreatesPrePopulatedQueue()
    {
      var queue = new BlockingQueue<Object>(new[] { new Object() });

      Assert.Equal(1, queue.Count);
    }

    #endregion

    #region Dispose Facts

    [Fact]
    public void DisposeIgnoresMultipleCalls()
    {
      var queue = new BlockingQueue<Object>();

      queue.Dispose();

      Assert.DoesNotThrow(queue.Dispose);
    }

    #endregion

    #region Clear Facts

    [Fact]
    public void ClearRemovesAllItems()
    {
      var queue = new BlockingQueue<Object>(new[] { new Object() });

      queue.Clear();

      Assert.Equal(0, queue.Count);
    }

    #endregion

    #region Enqueue Facts

    [Fact]
    public void EnqueueShouldPulseAllOnFirstItem()
    {
      var queue = new BlockingQueue<Object>(new[] { new Object() });

      queue.Clear();

      Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void EnqueShouldPulseAllThreadsOnFirstItemAdded()
    {
      var item = new Object();

      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _blockingQueue.Enqueue(item);
      _monitor.Verify(mock => mock.PulseAll(It.IsAny<Object>()), Times.Once());

      Assert.Equal(1, _underlyingQueue.Count);
    }

    [Fact]
    public void EnqueShouldNotPulseAllThreadsOnSubsequentItemsAdded()
    {
      var item1 = new Object();
      var item2 = new Object();

      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _blockingQueue.Enqueue(item1);
      _blockingQueue.Enqueue(item2);
      _monitor.Verify(mock => mock.PulseAll(It.IsAny<Object>()), Times.Once());

      Assert.Equal(2, _underlyingQueue.Count);
    }

    #endregion

    #region TryDequeue Facts

    [Fact]
    public void TryDequeueShouldReturnTrueWhenItemDequeuedWithInifinitTimeout()
    {
      Object item1 = String.Empty;
      Object item2;

      _underlyingQueue.Enqueue(item1);

      Assert.True(_blockingQueue.TryDequeue(out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void TryDequeueShouldReturnTrueWhenItemDequeuedWithTimespanTimeout()
    {
      Object item1 = String.Empty;
      Object item2;

      _underlyingQueue.Enqueue(item1);

      Assert.True(_blockingQueue.TryDequeue(TimeSpan.FromMilliseconds(10), out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void TryDequeueShouldReturnTrueWhenItemDequeuedWithMsTimeout()
    {
      Object item1 = String.Empty;
      Object item2;

      _underlyingQueue.Enqueue(item1);

      Assert.True(_blockingQueue.TryDequeue(Timeout.Infinite, out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void TryDequeueShouldReturnFalseWhenQueueDisposed()
    {
      Object item1 = String.Empty;
      Object item2;

      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _underlyingQueue.Enqueue(item1);
      _blockingQueue.Dispose();

      Assert.False(_blockingQueue.TryDequeue(Timeout.Infinite, out item2));
      Assert.Null(item2);
    }

    [Fact]
    public void TryDequeueShouldBlockUntilItemAdded()
    {
      Object item1 = String.Empty;
      Object item2;

      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _underlyingQueue.Enqueue(item1));

      Assert.True(_blockingQueue.TryDequeue(Timeout.Infinite, out item2));
      Assert.Same(item1, item2);
    }

    [Fact]
    public void TryDequeueShouldBlockUntilQueueDisposed()
    {
      Object item;

      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _blockingQueue.Dispose());

      Assert.False(_blockingQueue.TryDequeue(Timeout.Infinite, out item));
      Assert.Null(item);
    }

    #endregion

    #region TryDequeueAll Facts

    [Fact]
    public void TryDequeueAllShouldReturnTrueWhenItemDequeuedWithInifinitTimeout()
    {
      IList<Object> items;

      _underlyingQueue.Enqueue(String.Empty);

      Assert.True(_blockingQueue.TryDequeueAll(out items));
      Assert.Equal(1, items.Count);
      Assert.Equal(String.Empty, items[0]);
    }

    [Fact]
    public void TryDequeueAllShouldReturnTrueWhenItemDequeuedWithTimespanTimeout()
    {
      IList<Object> items;

      _underlyingQueue.Enqueue(String.Empty);

      Assert.True(_blockingQueue.TryDequeueAll(TimeSpan.FromMilliseconds(10), out items));
      Assert.Equal(1, items.Count);
      Assert.Equal(String.Empty, items[0]);
    }

    [Fact]
    public void TryDequeueAllShouldReturnTrueWhenItemDequeuedWithMsTimeout()
    {
      IList<Object> items;

      _underlyingQueue.Enqueue(String.Empty);

      Assert.True(_blockingQueue.TryDequeueAll(10, out items));
      Assert.Equal(1, items.Count);
      Assert.Equal(String.Empty, items[0]);
    }

    [Fact]
    public void TryDequeueAllShouldReturnFalseWhenQueueDisposed()
    {
      IList<Object> items2;

      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _underlyingQueue.Enqueue(String.Empty);
      _blockingQueue.Dispose();

      Assert.False(_blockingQueue.TryDequeueAll(Timeout.Infinite, out items2));
      Assert.Equal(0, items2.Count);
    }

    [Fact]
    public void TryDequeueAllShouldBlockUntilItemAdded()
    {
      IList<Object> items;

      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _underlyingQueue.Enqueue(String.Empty));

      Assert.True(_blockingQueue.TryDequeueAll(Timeout.Infinite, out items));
      Assert.Equal(1, items.Count);
      Assert.Equal(String.Empty, items[0]);
    }

    [Fact]
    public void TryDequeueAllShouldBlockUntilQueueDisposed()
    {
      IList<Object> items;

      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _blockingQueue.Dispose());

      Assert.False(_blockingQueue.TryDequeueAll(Timeout.Infinite, out items));
      Assert.Equal(0, items.Count);
    }

    #endregion

    #region Dequeue Facts

    [Fact]
    public void DequeueShouldReturnItemWhenItemDequeuedWithInifinitTimeout()
    {
      _underlyingQueue.Enqueue(String.Empty);

      Assert.Same(String.Empty, _blockingQueue.Dequeue());
    }

    [Fact]
    public void DequeueShouldReturnItemWhenItemDequeuedWithTimespanTimeout()
    {
      _underlyingQueue.Enqueue(String.Empty);

      Assert.Same(String.Empty, _blockingQueue.Dequeue(TimeSpan.FromMilliseconds(10)));
    }

    [Fact]
    public void DequeueShouldReturnItemWhenItemDequeuedWithMsTimeout()
    {
      _underlyingQueue.Enqueue(String.Empty);

      Assert.Same(String.Empty, _blockingQueue.Dequeue(Timeout.Infinite));
    }

    [Fact]
    public void DequeueShouldThrowExWhenQueueDisposed()
    {
      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _underlyingQueue.Enqueue(String.Empty);
      _blockingQueue.Dispose();

      Assert.Throws<ObjectDisposedException>(() => _blockingQueue.Dequeue(Timeout.Infinite));
    }

    [Fact]
    public void DequeueShouldBlockUntilItemAdded()
    {
      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _underlyingQueue.Enqueue(String.Empty));

      Assert.Equal(String.Empty, _blockingQueue.Dequeue(Timeout.Infinite));
    }

    [Fact]
    public void DequeueShouldBlockUntilQueueDisposedAndThenThrowEx()
    {
      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _blockingQueue.Dispose());

      Assert.Throws<ObjectDisposedException>(() => _blockingQueue.Dequeue(Timeout.Infinite));
    }

    #endregion

    #region DequeueAll Facts

    [Fact]
    public void DequeueAllShouldReturnItemWhenItemDequeueAlldWithInifinitTimeout()
    {
      _underlyingQueue.Enqueue(String.Empty);

      IList<Object> list = _blockingQueue.DequeueAll();

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void DequeueAllShouldReturnItemWhenItemDequeueAlldWithTimespanTimeout()
    {
      _underlyingQueue.Enqueue(String.Empty);

      IList<Object> list = _blockingQueue.DequeueAll(TimeSpan.FromMilliseconds(10));

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void DequeueAllShouldReturnItemWhenItemDequeueAlldWithMsTimeout()
    {
      _underlyingQueue.Enqueue(String.Empty);

      IList<Object> list = _blockingQueue.DequeueAll(Timeout.Infinite);

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void DequeueAllShouldThrowExWhenQueueDisposed()
    {
      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _underlyingQueue.Enqueue(String.Empty);
      _blockingQueue.Dispose();

      Assert.Throws<ObjectDisposedException>(() => _blockingQueue.DequeueAll(Timeout.Infinite));
    }

    [Fact]
    public void DequeueAllShouldBlockUntilItemAdded()
    {
      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _underlyingQueue.Enqueue(String.Empty));
      
      IList<Object> list = _blockingQueue.DequeueAll(Timeout.Infinite);

      Assert.Equal(1, list.Count);
      Assert.Equal(String.Empty, list[0]);
    }

    [Fact]
    public void DequeueAllShouldBlockUntilQueueDisposedAndThenThrowEx()
    {
      _monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      _monitor.Setup(mock => mock.Wait(It.IsAny<Object>(), Timeout.Infinite)).Returns(false).Callback(() => _blockingQueue.Dispose());

      Assert.Throws<ObjectDisposedException>(() => _blockingQueue.DequeueAll(Timeout.Infinite));
    }

    #endregion

  }
}
