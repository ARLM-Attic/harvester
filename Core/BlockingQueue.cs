using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NLog;

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

namespace Harvester.Core
{
  public sealed class BlockingQueue<T> : IBlockingQueue<T>
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly IMonitor _monitor;
    private readonly Queue<T> _queue;
    private Boolean _disposed;

    public Object SyncRoot { get { return ((ICollection)_queue).SyncRoot; } }

    public BlockingQueue()
      : this(new Queue<T>(), MonitorWrapper.Instance)
    { }

    public BlockingQueue(Int32 capacity)
      : this(new Queue<T>(capacity), MonitorWrapper.Instance)
    { }

    public BlockingQueue(IEnumerable<T> collection)
      : this(new Queue<T>(collection), MonitorWrapper.Instance)
    { }

    internal BlockingQueue(Queue<T> queue, IMonitor monitor)
    {
      Verify.NotNull(queue); 
      Verify.NotNull(monitor);

      _queue = queue;
      _monitor = monitor;
    }

    public void Dispose()
    {
      Log.Debug("Dispose invoked.");

      lock (SyncRoot)
      {
        if (_disposed)
          return;

        _queue.Clear();
        _disposed = true;

        Log.Debug("Pulsing all threads.");

        _monitor.PulseAll(SyncRoot);
      }

      GC.SuppressFinalize(this);
    }

    public void Clear()
    {
      Log.Debug("Clearing all items from queue.");

      lock (SyncRoot)
      {
        _queue.Clear();
        
        Log.Debug("Pulsing all threads.");

        _monitor.PulseAll(SyncRoot);
      }
    }

    public Int32 Count
    {
      get
      {
        Log.Debug("Retrieving queue item count.");

        lock (SyncRoot)
          return _queue.Count;
      }
    }

    public T Dequeue()
    {
      return Dequeue(Timeout.Infinite);
    }

    public T Dequeue(TimeSpan timeout)
    {
      return Dequeue(Convert.ToInt32(timeout.TotalMilliseconds));
    }

    public T Dequeue(Int32 millisecondsTimeout)
    {
      Log.Debug("Attempting to dequeue item (Dequeue).");

      T result;

      if (TryDequeue(millisecondsTimeout, out result))
        return result;
      
      //TryDequeue will only return false is if _disposed == true
      throw new ObjectDisposedException(GetType().FullName);
    }

    public Boolean TryDequeue(out T result)
    {
      return TryDequeue(Timeout.Infinite, out result);
    }

    public Boolean TryDequeue(TimeSpan timeout, out T result)
    {
      return TryDequeue(Convert.ToInt32(timeout.TotalMilliseconds), out result);
    }

    public Boolean TryDequeue(Int32 millisecondsTimeout, out T result)
    {
      Log.Debug("Attempting to dequeue item (TryDequeue).");

      lock (SyncRoot)
      {
        while (!_disposed && _queue.Count == 0)
        {
          Log.Debug("Queue empty; entering Wait.");

          _monitor.Wait(SyncRoot, millisecondsTimeout);

          Log.Debug("Exiting wait.");
        }

        Log.Debug("One or more items ready to be dequeued.");

        result = default(T);

        if (_disposed)
          return false;

        Log.Debug("Dequeing item.");

        result = _queue.Dequeue();

        return true;
      }
    }

    public IList<T> DequeueAll()
    {
      return DequeueAll(Timeout.Infinite);
    }

    public IList<T> DequeueAll(TimeSpan timeout)
    {
      return DequeueAll(Convert.ToInt32(timeout.TotalMilliseconds));
    }

    public IList<T> DequeueAll(Int32 millisecondsTimeout)
    {
      Log.Debug("Attempting to dequeue all items (DequeueAll).");

      IList<T> result;

      if (TryDequeueAll(millisecondsTimeout, out result))
        return result;

      //TryDequeueAll will only return false is if _disposed == true
      throw new ObjectDisposedException(GetType().FullName);
    }

    public Boolean TryDequeueAll(out IList<T> result)
    {
      return TryDequeueAll(Timeout.Infinite, out result);
    }

    public Boolean TryDequeueAll(TimeSpan timeout, out IList<T> result)
    {
      return TryDequeueAll(Convert.ToInt32(timeout.TotalMilliseconds), out result);
    }

    public Boolean TryDequeueAll(Int32 millisecondsTimeout, out IList<T> result)
    {
      Log.Debug("Attempting to dequeue all items (TryDequeueAll).");

      lock (SyncRoot)
      {
        while (!_disposed && _queue.Count == 0)
        {
          Log.Debug("Queue empty; entering Wait.");

          _monitor.Wait(SyncRoot, millisecondsTimeout);

          Log.Debug("Exiting wait.");
        }

        Log.Debug("One or more items ready to be dequeued.");

        result = new List<T>(_queue.Count);

        if (_disposed)
          return false;
        
        Log.Debug("Dequeing items.");
        
        while (_queue.Count > 0)
          result.Add(_queue.Dequeue());

        return true;
      }
    }

    public void Enqueue(T item)
    {
      Log.Debug("Attempting to enqueue item.");

      lock (SyncRoot)
      {
        _queue.Enqueue(item);

        if (_queue.Count != 1) 
          return;

        Log.Debug("Pulsing all threads.");

        _monitor.PulseAll(SyncRoot);
      }
    }
  }
}
