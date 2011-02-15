using System;
using System.Runtime.InteropServices;
using System.Threading;
using Harvester.Core.Win32;
using Harvester.Core.Win32.Basic;

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

namespace Harvester.Core.Messages.Sources.DbWin
{
  internal class DbWinMessageProducer : IBackgroundWorker
  {
    private readonly Object _syncRoot = new Object();

    private readonly IEnqueuer<DbWinMessage> _messageEnqueuer;
    private readonly Handle _dbwinBufferReadyEvent;
    private readonly Handle _dbwinDataReadyEvent;
    private readonly Handle _dbwinBufferFile;
    private readonly Handle _dbwinBufferView;
    private readonly IWindowsApi _windowsApi;

    private Thread _dbwinBufferListener;
    private Boolean _listening;
    private Boolean _disposed;

    public DbWinMessageProducer(IEnqueuer<DbWinMessage> messageEnqueuer)
      : this(messageEnqueuer, WindowsApi.Instance)
    { }

    internal DbWinMessageProducer(IEnqueuer<DbWinMessage> messageEnqueuer, IWindowsApi windowsApi)
    {
      _windowsApi = windowsApi;
      _messageEnqueuer = messageEnqueuer;

      _windowsApi.Advanced.InitializeSecurityDescriptor();

      var securityAttributes = new SecurityAttributes();

      _dbwinBufferReadyEvent = _windowsApi.Basic.CreateLocalEvent(ref securityAttributes, "DBWIN_BUFFER_READY");
      _dbwinDataReadyEvent = _windowsApi.Basic.CreateLocalEvent(ref securityAttributes, "DBWIN_DATA_READY");
      _dbwinBufferFile = _windowsApi.Basic.CreateLocalFileMapping(ref securityAttributes, "DBWIN_BUFFER");
      _dbwinBufferView = _windowsApi.Basic.MapReadOnlyViewOfFile(_dbwinBufferFile);
    }

    public void Start()
    {
      lock (_syncRoot)
      {
        if (_disposed)
          throw new ObjectDisposedException(GetType().FullName);

        if (_listening)
          throw new InvalidOperationException(); //TODO: Set message

        _listening = true;

        _dbwinBufferListener = new Thread(CaptureOutputDebugStringData)
                                 {
                                   IsBackground = true,
                                   Name = "DbWin Message Producer"
                                 };
        _dbwinBufferListener.Start();
      }
    }

    public void Stop()
    {
      lock (_syncRoot)
      {
        if (!_listening)
          return;

        _listening = false;

        _windowsApi.Basic.PulseEvent(_dbwinDataReadyEvent);

        while (_dbwinBufferListener.IsAlive)
          Thread.Sleep(25);
      }
    }

    private void CaptureOutputDebugStringData()
    {
      var pidOffset = _dbwinBufferView;
      var messageOffset = pidOffset + Marshal.SizeOf(typeof(Int32));

      while (_listening)
      {
        _windowsApi.Basic.SetEvent(_dbwinBufferReadyEvent);

        Int32 ret = _windowsApi.Basic.WaitForSingleObject(_dbwinDataReadyEvent);

        if (!_listening)
          break;

        if (ret != 0)
          continue;

        var processId = Marshal.ReadInt32(pidOffset);
        var message = Marshal.PtrToStringAnsi(messageOffset);

        _messageEnqueuer.Enqueue(new DbWinMessage(processId, message));
      }
    }

    public void Dispose()
    {
      lock (_syncRoot)
      {
        if (_disposed)
          return;

        _disposed = true;
      }

      Stop();

      _dbwinBufferReadyEvent.Dispose();
      _dbwinDataReadyEvent.Dispose();
      _dbwinBufferFile.Dispose();
      _dbwinBufferView.Dispose();
    }
  }
}
