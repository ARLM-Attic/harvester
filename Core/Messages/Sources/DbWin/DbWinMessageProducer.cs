using System;
using System.Runtime.InteropServices;
using System.Threading;
using Harvester.Core.Logging;
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
    private static readonly ILog Log = LogManager.CreateClassLogger();
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
      Verify.NotNull(messageEnqueuer);
      Verify.NotNull(windowsApi);

      _windowsApi = windowsApi;
      _messageEnqueuer = messageEnqueuer;

      Log.Debug("Creating Win32 Securitydescriptor.");

      _windowsApi.Advanced.InitializeSecurityDescriptor();

      var securityAttributes = new SecurityAttributes();

      Log.Debug("Creating Win32 handles.");

      _dbwinBufferReadyEvent = _windowsApi.Basic.CreateLocalEvent(ref securityAttributes, "DBWIN_BUFFER_READY");
      _dbwinDataReadyEvent = _windowsApi.Basic.CreateLocalEvent(ref securityAttributes, "DBWIN_DATA_READY");
      _dbwinBufferFile = _windowsApi.Basic.CreateLocalFileMapping(ref securityAttributes, "DBWIN_BUFFER");
      _dbwinBufferView = _windowsApi.Basic.MapReadOnlyViewOfFile(_dbwinBufferFile);
    }

    public void Start()
    {
      Log.Debug("Starting producer.");

      lock (_syncRoot)
      {
        if (_disposed)
          throw new ObjectDisposedException(GetType().FullName);

        if (_listening)
          throw new InvalidOperationException(Localization.DbWinMessageProducerAlreadyStarted);

        _listening = true;

        Log.Debug("Creating producer thread.");

        _dbwinBufferListener = new Thread(CaptureOutputDebugStringData)
                                 {
                                   IsBackground = true,
                                   Name = "DbWin Reader"
                                 };
        _dbwinBufferListener.Start();

        Log.Debug("Producer thread started.");
      }
    }

    public void Stop()
    {
      Log.Debug("Stopping producer.");

      lock (_syncRoot)
      {
        if (!_listening)
          return;

        _listening = false;

        Log.Debug("Pulsing DBWIN_DATA_READY buffer.");

        _windowsApi.Basic.PulseEvent(_dbwinDataReadyEvent);

        Log.Debug("Waiting for producer thread to exit.");

        while (_dbwinBufferListener.IsAlive)
          Thread.Sleep(25);

        Log.Debug("Producer thread exited.");
      }
    }

    private void CaptureOutputDebugStringData()
    {
      var pidOffset = _dbwinBufferView;
      var messageOffset = pidOffset + Marshal.SizeOf(typeof(Int32));

      while (_listening)
      {
        try
        {
          Log.Debug("Setting DBWIN_BUFFER_READY buffer.");

          _windowsApi.Basic.SetEvent(_dbwinBufferReadyEvent);

          Log.Debug("Waiting on DBWIN_DATA_READY buffer.");

          Int32 ret = _windowsApi.Basic.WaitForSingleObject(_dbwinDataReadyEvent);

          Log.DebugFormat("DBWIN_DATA_READY buffer ready ({0}).", ret);

          if (!_listening)
            break;

          if (ret != 0)
            continue;

          Log.Debug("Reading DBWIN_DATA_READY buffer.");

          var processId = Marshal.ReadInt32(pidOffset);
          var message = Marshal.PtrToStringAnsi(messageOffset);

          Log.Debug("Adding DbWinMessage to queue.");

          _messageEnqueuer.Enqueue(new DbWinMessage(processId, message));
        }
        catch (Exception ex)
        {
          Log.Fatal(ex.Message, ex);
          throw;
        }
      }
    }

    public void Dispose()
    {
      Log.Debug("Dispose invoked.");

      lock (_syncRoot)
      {
        if (_disposed)
          return;

        _disposed = true;
      }

      Stop();

      Log.Debug("Dispose Win32 handles.");

      _dbwinBufferReadyEvent.Dispose();
      _dbwinDataReadyEvent.Dispose();
      _dbwinBufferFile.Dispose();
      _dbwinBufferView.Dispose();
    }
  }
}
