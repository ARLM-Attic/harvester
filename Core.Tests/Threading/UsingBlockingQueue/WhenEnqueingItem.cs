using System;
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
  public class WhenEnqueingItem : BlockingQueueTestBase
  {
    [Fact]
    public void PulseAllThreadsOnFirstItemAdded()
    {
      var item = new Object();

      Monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      BlockingQueue.Enqueue(item);
      Monitor.Verify(mock => mock.PulseAll(It.IsAny<Object>()), Times.Once());

      Assert.Equal(1, UnderlyingQueue.Count);
    }

    [Fact]
    public void DoNotPulseAllThreadsOnSubsequentItemsAdded()
    {
      var item1 = new Object();
      var item2 = new Object();

      Monitor.Setup(mock => mock.PulseAll(It.IsAny<Object>()));
      BlockingQueue.Enqueue(item1);
      BlockingQueue.Enqueue(item2);
      Monitor.Verify(mock => mock.PulseAll(It.IsAny<Object>()), Times.Once());

      Assert.Equal(2, UnderlyingQueue.Count);
    }
  }
}
