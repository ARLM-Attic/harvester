using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Harvester.Core.Messages.Sources.DbWin
{
  internal class DbWinMessageConsumer
  {
    private readonly Action<IEnumerable<ILogMessage>> _messagesReceivedCallback;
    private readonly ILogMessageFactory _logMessageFactory = new LogMessageFactory("Debug.Output.Local");
    private readonly IDequeuer<DbWinMessage> _messageDequeuer;
    private readonly Object _syncRoot = new Object();

    private Thread _dbwinMessageProcessor;
    private Boolean _listening;
    private Boolean _disposed;

    public DbWinMessageConsumer(IDequeuer<DbWinMessage> messageDequeuer, Action<IEnumerable<ILogMessage>> messagesReceivedCallback)
    {
      _messageDequeuer = messageDequeuer;
      _messagesReceivedCallback = messagesReceivedCallback;
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

        _dbwinMessageProcessor = new Thread(CaptureOutputDebugStringData)
                                   {
                                     IsBackground = true,
                                     Name = "DbWin Message Consumer"
                                   };
        _dbwinMessageProcessor.Start();
      }
    }

    public void Stop()
    {
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
        IList<DbWinMessage> dbWinMessages;

        if (!_messageDequeuer.TryDequeueAll(out dbWinMessages))
          break;

        if (!_listening)
          break;

        _messagesReceivedCallback.Invoke(dbWinMessages.Select(dbwinMessage => _logMessageFactory.Create(dbwinMessage.Timestamp, dbwinMessage.ProcessId, dbwinMessage.Message)));
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
    }
  }
}
