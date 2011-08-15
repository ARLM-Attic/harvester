using System;
using System.Threading;
using Harvester.Core.Tracing;
using Moq;

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

namespace Harvester.Core.Tests.Tracing.UsingOutputDebugStringListener
{
  public abstract class OutputDebugStringListenerTestBase : IDisposable
  {
    protected readonly FakeBuffer Buffer = new FakeBuffer();
    protected readonly ITraceListener Listener;

    protected OutputDebugStringListenerTestBase()
    {
      Listener = new OutputDebugStringListener("UnitTest", "Local\\HRVSTR_UNIT_TEST", Buffer);
    }

    public void Dispose()
    {
      Listener.Dispose();
    }

    public class FakeBuffer : IBuffer
    {
      private readonly ManualResetEvent _bufferReadyEvent = new ManualResetEvent(false);
      private readonly ManualResetEvent _dataReadyEvent = new ManualResetEvent(false);
      private Boolean _disposed;
      private Byte[] _buffer;

      public void SignalDataReady(Byte[] data)
      {
        _buffer = data;

        _dataReadyEvent.Set();
      }

      public void WaitBufferReady()
      {
        _bufferReadyEvent.WaitOne();
        _bufferReadyEvent.Reset();
      }

      public Byte[] Read()
      {
        _bufferReadyEvent.Set();
        _dataReadyEvent.WaitOne();
        _dataReadyEvent.Reset();

        return _buffer;
      }

      public Boolean Write(Byte[] buffer)
      {
        throw new NotImplementedException();
      }

      public void Dispose()
      {
        if (_disposed)
          return;

        SignalDataReady(new Byte[0]);
        
        _bufferReadyEvent.Dispose();
        _dataReadyEvent.Dispose();
        _disposed = true;
      }
    }
  }
}
