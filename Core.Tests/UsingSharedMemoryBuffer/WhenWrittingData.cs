using System;
using System.Text;
using System.Threading;
using Harvester.Core.Extensions;
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
  public class WhenWrittingData : IDisposable
  {
    private readonly Random _randomizer;
    private readonly SharedMemoryBuffer _buffer;

    public WhenWrittingData()
    {
      var guid = Guid.NewGuid();
      var mutexName = String.Format(@"HRVSTR_{0}_MUTEXT", guid);
      var bufferName = String.Format(@"Local\HRVSTR_{0}", guid);

      _randomizer = new Random(1);
      _buffer = new SharedMemoryBuffer(mutexName, bufferName, 1024);
    }

    public void Dispose()
    {
      _buffer.Dispose();
    }

    [Fact]
    public void CannotWriteToDisposedObject()
    {
      _buffer.Dispose();

      Assert.Throws<ObjectDisposedException>(() => _buffer.Write(new Byte[0]));
    }

    [Fact]
    public void CannotOverwriteData()
    {
      using(var syncEvent1 = new ManualResetEvent(false))
      using (var syncEvent2 = new ManualResetEvent(false))
      {

        ThreadPool.QueueUserWorkItem(state =>
                                       {
                                         syncEvent1.WaitOne();

                                         _buffer.Write(Encoding.ASCII.GetBytes("Sample Data 2"));

                                         syncEvent2.Set();
                                       });

        _buffer.Write(Encoding.ASCII.GetBytes("Sample Data 1"));

        syncEvent1.Set();

        Assert.False(syncEvent2.WaitOne(TimeSpan.FromMilliseconds(100)));
        Assert.Contains("Sample Data 1", Encoding.ASCII.GetString(_buffer.Read()));

        syncEvent2.WaitOne();
      }
    }

    [Fact]
    public void CannotExceedBufferLength()
    {
      var largeString = _randomizer.NextString(2560);

      _buffer.Write(Encoding.ASCII.GetBytes(largeString));

      Assert.Contains(largeString.Substring(0, 1024), Encoding.ASCII.GetString(_buffer.Read()));
    }

    [Fact]
    public void IgnoreNullData()
    {
      _buffer.Write(null);

      // Second write typically not allowed without an intermediate read.
      Assert.False(_buffer.Write(null));
    }

    [Fact]
    public void IgnoreEmptyData()
    {
      _buffer.Write(new Byte[0]);

      // Second write typically not allowed without an intermediate read.
      Assert.False(_buffer.Write(new Byte[0]));
    }

    [Fact]
    public void IgnoreIfBufferReadyEventNotReceived()
    {
      var data = Encoding.ASCII.GetBytes(_randomizer.NextString(32));

      _buffer.Write(data);

      // Second write typically not allowed without an intermediate read.
      Assert.False(_buffer.Write(data));
    }
  }
}
