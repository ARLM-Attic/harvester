using System;
using System.Text;
using System.Threading;
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

namespace Harvester.Core.Tests.UsingSharedMemoryBuffer
{
  public class WhenReadingData : IDisposable
  {
    private readonly ManualResetEvent _syncEvent1;
    private readonly ManualResetEvent _syncEvent2;
    private readonly ManualResetEvent _syncEvent3;
    private readonly SharedMemoryBuffer _buffer;

    public WhenReadingData()
    {
      var guid = Guid.NewGuid();
      var bufferName = String.Format(@"Local\HRVSTR_{0}", guid);

      _buffer = new SharedMemoryBuffer(bufferName, 1024);
      _syncEvent1 = new ManualResetEvent(false);
      _syncEvent2 = new ManualResetEvent(false);
      _syncEvent3 = new ManualResetEvent(false);
    }

    public void Dispose()
    {
      _syncEvent3.Dispose();
      _syncEvent2.Dispose();
      _syncEvent1.Dispose();
      _buffer.Dispose();
    }

    [Fact]
    public void CannotReadFromDisposedObject()
    {
      _buffer.Dispose();

      Assert.Throws<ObjectDisposedException>(() => _buffer.Read());
    }

    [Fact]
    public void ReadBlocksUntilWrite()
    {
      ThreadPool.QueueUserWorkItem(state =>
                                     {
                                       _syncEvent1.Set();

                                       Assert.Contains("Sample Data", Encoding.ASCII.GetString(_buffer.Read()));

                                       _syncEvent2.Set();
                                     });

      _syncEvent1.WaitOne();

      Assert.False(_syncEvent2.WaitOne(TimeSpan.FromMilliseconds(100)));

      _buffer.Write(Encoding.ASCII.GetBytes("Sample Data"));

      Assert.True(_syncEvent2.WaitOne());
    }

    [Fact]
    public void CanReadImmediatelyIfBufferHasData()
    {
      _buffer.Write(Encoding.ASCII.GetBytes("Sample Data"));

      Assert.Contains("Sample Data", Encoding.ASCII.GetString(_buffer.Read()));
    }

    [Fact]
    public void CanOnlyReadDataOnce()
    {
      ThreadPool.QueueUserWorkItem(state =>
                                     {
                                       _buffer.Write(Encoding.ASCII.GetBytes("Sample Data 1"));

                                       _syncEvent1.Set();
                                       _syncEvent2.WaitOne();

                                       Assert.Contains("Sample Data 2", Encoding.ASCII.GetString(_buffer.Read()));

                                       _syncEvent3.Set();
                                     });

      _syncEvent1.WaitOne();

      Assert.Contains("Sample Data 1", Encoding.ASCII.GetString(_buffer.Read()));

      _syncEvent2.Set();

      Assert.False(_syncEvent3.WaitOne(TimeSpan.FromMilliseconds(100)));

      _buffer.Write(Encoding.ASCII.GetBytes("Sample Data 2"));

      Assert.True(_syncEvent3.WaitOne());
    }

    [Fact]
    public void BlockedReadThrowsObjectDisposedExceptionOnDisposeCall()
    {
      ThreadPool.QueueUserWorkItem(state =>
                                     {
                                       _syncEvent1.WaitOne();

                                       _buffer.Dispose();

                                       _syncEvent2.Set();
                                     });

      _syncEvent1.Set();

      Assert.Throws<ObjectDisposedException>(() => _buffer.Read());

      _syncEvent2.WaitOne();
    }
  }
}
