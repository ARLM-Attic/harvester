﻿using System;
using System.Collections.Generic;

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
  public interface IBlockingQueue<T> : IDisposable
  {
    Int32 Count { get; }

    void Clear();
    void Enqueue(T item);

    T Dequeue();
    T Dequeue(TimeSpan timeout);
    T Dequeue(Int32 millisecondsTimeout);

    IList<T> DequeueAll();
    IList<T> DequeueAll(TimeSpan timeout);
    IList<T> DequeueAll(Int32 millisecondsTimeout);

    Boolean TryDequeue(out T result);
    Boolean TryDequeue(TimeSpan timeout, out T result);
    Boolean TryDequeue(Int32 millisecondsTimeout, out T result);

    Boolean TryDequeueAll(out IList<T> result);
    Boolean TryDequeueAll(TimeSpan timeout, out IList<T> result);
    Boolean TryDequeueAll(Int32 millisecondsTimeout, out IList<T> result);
  }
}
