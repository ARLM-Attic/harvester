using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Harvester.Core.Messages.Sources.DbWin
{
  internal class DbWinMessageConsumer : IBackgroundWorker
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly Action<IEnumerable<ILogMessage>> _messagesReceivedCallback;
    private readonly IDequeuer<DbWinMessage> _messageDequeuer;
    private readonly ILogMessageFactory _logMessageFactory;
    private readonly Object _syncRoot = new Object();

    private Thread _dbwinMessageProcessor;
    private Boolean _listening;
    private Boolean _disposed;

    public DbWinMessageConsumer(IDequeuer<DbWinMessage> messageDequeuer, Action<IEnumerable<ILogMessage>> messagesReceivedCallback)
      : this(messageDequeuer, messagesReceivedCallback, new LogMessageFactory("Debug.Output.Local"))
    { }

    internal DbWinMessageConsumer(IDequeuer<DbWinMessage> messageDequeuer, Action<IEnumerable<ILogMessage>> messagesReceivedCallback, ILogMessageFactory logMessageFactory)
    {
      Verify.NotNull(messageDequeuer);
      Verify.NotNull(logMessageFactory);
      Verify.NotNull(messagesReceivedCallback);

      _messageDequeuer = messageDequeuer;
      _logMessageFactory = logMessageFactory;
      _messagesReceivedCallback = messagesReceivedCallback;
    }

    public void Start()
    {
      Log.Debug("Starting consumer.");

      lock (_syncRoot)
      {
        if (_disposed)
          throw new ObjectDisposedException(GetType().FullName);

        if (_listening)
          throw new InvalidOperationException(Localization.DbWinMessageConsumerAlreadyStarted);

        _listening = true;

        Log.Debug("Creating consumer thread.");

        _dbwinMessageProcessor = new Thread(CaptureOutputDebugStringData)
                                   {
                                     IsBackground = true,
                                     Name = "DbWin Notifier"
                                   };
        _dbwinMessageProcessor.Start();

        Log.Debug("Consumer thread started.");
      }
    }

    public void Stop()
    {
      Log.Debug("Stopping consumer.");

      lock (_syncRoot)
      {
        if (!_listening)
          return;

        _listening = false;
      }
    }

    private void CaptureOutputDebugStringData()
    {
      while (_listening)
      {
        try
        {
          IList<DbWinMessage> dbWinMessages;

          Log.Debug("Waiting for DbWinMessages.");

          // TryDequeueAll will only return false if underlying Queue has been disposed; break out of loop and exit thread.
          if (!_messageDequeuer.TryDequeueAll(out dbWinMessages))
            break;

          Log.Debug("One or more DbWinMessages dequeued.");

          if (!_listening)
            break;

          Log.Debug("Invoking MessagesReceivedCallback.");

          _messagesReceivedCallback.Invoke(dbWinMessages.Select(dbwinMessage => _logMessageFactory.Create(dbwinMessage.Timestamp, dbwinMessage.ProcessId, dbwinMessage.Message)));
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
    }
  }
}
