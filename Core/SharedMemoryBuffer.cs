using System;
using System.IO.MemoryMappedFiles;
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
  internal sealed class SharedMemoryBuffer : IBuffer
  {
    private readonly Mutex _writeMutex;
    private readonly MemoryMappedFile _bufferFile;
    private readonly EventWaitHandle _dataReadyEvent;
    private readonly EventWaitHandle _bufferReadyEvent;
    private readonly MemoryMappedViewAccessor _bufferView;
    private readonly Byte[] _buffer;

    private Boolean Disposed { get; set; }

    public SharedMemoryBuffer(String mutexName, String baseObjectName, Int64 capacity)
    {
      Verify.NotWhitespace(baseObjectName);
      Verify.GreaterThanZero(capacity);

      _buffer = new Byte[capacity];
      _writeMutex = new Mutex(false, mutexName);
      _dataReadyEvent = new EventWaitHandle(false, EventResetMode.AutoReset, baseObjectName + "_DATA_READY");
      _bufferReadyEvent = new EventWaitHandle(true, EventResetMode.AutoReset, baseObjectName + "_BUFFER_READY");
      _bufferFile = MemoryMappedFile.CreateOrOpen(baseObjectName + "_BUFFER", capacity, MemoryMappedFileAccess.ReadWrite);
      _bufferView = _bufferFile.CreateViewAccessor(0, 0, MemoryMappedFileAccess.ReadWrite);   
    }

    public void Dispose()
    {
      lock (_buffer)
      {
        if (Disposed)
          return;

        Disposed = true;
      }

      _dataReadyEvent.Set();

      _bufferReadyEvent.Dispose();
      _dataReadyEvent.Dispose();
      _bufferView.Dispose();
      _bufferFile.Dispose();
      _writeMutex.Dispose();
    }

    public Byte[] Read()
    {
      EnsureNotDisposed();

      _dataReadyEvent.WaitOne();

      var bytesRead = _bufferView.ReadArray(0, _buffer, 0, _buffer.Length);
      var result = new Byte[bytesRead];

      Buffer.BlockCopy(_buffer, 0, result, 0, bytesRead);

      _bufferReadyEvent.Set();

      return result;
    }

    public void Write(Byte[] buffer)
    {
      EnsureNotDisposed();

      if (buffer == null || buffer.Length == 0)
        return;

      _writeMutex.WaitOne();
      try
      {
        _bufferReadyEvent.WaitOne();

        _bufferView.WriteArray(0, buffer, 0, Math.Min(buffer.Length, _buffer.Length));

        _dataReadyEvent.Set();
      }
      finally
      {
        _writeMutex.ReleaseMutex();
      }
    }

    private void EnsureNotDisposed()
    {
      if (Disposed)
        throw new ObjectDisposedException(GetType().FullName);
    }
  }
}
