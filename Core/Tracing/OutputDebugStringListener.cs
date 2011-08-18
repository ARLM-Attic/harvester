using System;
using System.Threading;
using Harvester.Core.Logging;

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

namespace Harvester.Core.Tracing
{
  internal class OutputDebugStringListener : ITraceListener
  {
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();
    private readonly OutputDebugStringReader _reader;
    private readonly IBuffer _sharedMemoryBuffer;
    private readonly Thread _listenerThread;
    private readonly String _listenerName;
    private readonly Mutex _mutex;

    public event EventHandler<TraceEventArgs> TraceEventReceived;
    public String Name { get { return _listenerName; } }
    private Boolean Disposed { get; set; }

    public OutputDebugStringListener(String friendlyName, String mutexName, String baseObjectName)
      : this(friendlyName, mutexName, new SharedMemoryBuffer(baseObjectName, 4096))
    { }

    internal OutputDebugStringListener(String friendlyName, String mutexName, IBuffer sharedMemoryBuffer)
    {
      Verify.NotNull(sharedMemoryBuffer);
      Verify.NotWhitespace(friendlyName);
      Verify.NotWhitespace(mutexName);

      _listenerName = friendlyName;
      _mutex = new Mutex(false, mutexName);
      _sharedMemoryBuffer = sharedMemoryBuffer;
      _reader = new OutputDebugStringReader(OutputDebugString.GetMaxCharCount(_sharedMemoryBuffer.Capacity));
      _listenerThread = new Thread(ReadBufferData) { Name = sharedMemoryBuffer.Name + " Listener", Priority = ThreadPriority.Highest, IsBackground = true };
      _listenerThread.Start();
    }

    public void Dispose()
    {
      Log.Debug("Disposing Monitor");

      Disposed = true;

      _sharedMemoryBuffer.Dispose();
      _mutex.Dispose();

      if (_listenerThread.IsAlive)
        _listenerThread.Join();
    }

    private void ReadBufferData()
    {
      while (!Disposed)
      {
        Log.Debug("Reading from shared memory.");

        try
        {
          foreach (var outputDebugString in _reader.GetOutputDebugStrings(_sharedMemoryBuffer.Read()))
            RaiseTraceEventReceived(outputDebugString);
        }
        catch (ObjectDisposedException)
        {
          break;
        }
      }
    }

    private void RaiseTraceEventReceived(OutputDebugString outputDebugString)
    {
      Log.Debug("Raising trace event rececived.");

      var defensiveCopy = TraceEventReceived;
      if (defensiveCopy != null)
        defensiveCopy.Invoke(this, new TraceEventArgs(outputDebugString.ProcessId, outputDebugString.Message, Name));
    }
  }
}
