using System;
using System.Collections;
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

namespace Harvester.Core
{
  public sealed class BlockingQueue<T> : IBlockingQueue<T>
  {
    private readonly Queue<T> _queue;
    private Boolean _disposed;

    public Object SyncRoot { get { return ((ICollection)_queue).SyncRoot; } }

    public BlockingQueue()
    {
      _queue = new Queue<T>();
    }

    public BlockingQueue(Int32 capacity)
    {
      _queue = new Queue<T>(capacity);
    }

    public BlockingQueue(IEnumerable<T> collection)
    {
      _queue = new Queue<T>(collection);
    }

    public void Dispose()
    {
      lock (SyncRoot)
      {
        if (_disposed)
          return;

        _queue.Clear();
        _disposed = true;
        Monitor.PulseAll(SyncRoot);
      }

      GC.SuppressFinalize(this);
    }

    public void Clear()
    {
      lock (SyncRoot)
      {
        _queue.Clear();
        Monitor.PulseAll(SyncRoot);
      }
    }

    public Int32 Count
    {
      get
      {
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
      T result;

      if (TryDequeue(millisecondsTimeout, out result))
        return result;

      if (_disposed)
        throw new ObjectDisposedException(GetType().FullName);

      throw new Exception(); //TODO: What should be thrown?
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
      lock (SyncRoot)
      {
        while (!_disposed && _queue.Count == 0)
          Monitor.Wait(SyncRoot, millisecondsTimeout);

        result = default(T);

        if (_disposed)
          return false;

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
      IList<T> result;

      if (TryDequeueAll(millisecondsTimeout, out result))
        return result;

      if (_disposed)
        throw new ObjectDisposedException(GetType().FullName);

      throw new Exception(); //TODO: What should be thrown?
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
      lock (SyncRoot)
      {
        while (!_disposed && _queue.Count == 0)
          Monitor.Wait(SyncRoot, millisecondsTimeout);

        result = new List<T>(_queue.Count);

        if (_disposed)
          return false;

        while (_queue.Count > 0)
          result.Add(_queue.Dequeue());

        return true;
      }
    }

    public void Enqueue(T item)
    {
      lock (SyncRoot)
      {
        _queue.Enqueue(item);

        if (_queue.Count == 1)
          Monitor.PulseAll(SyncRoot);
      }
    }
  }
}
