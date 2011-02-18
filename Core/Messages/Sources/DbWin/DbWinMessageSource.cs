using System;
using System.Collections.Generic;
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
  public sealed class DbWinMessageSource : ILogMessageSource
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly IBlockingQueue<DbWinMessage> _blockingQueue = new BlockingQueue<DbWinMessage>();
    private readonly IBackgroundWorker _messageConsumer;
    private readonly IBackgroundWorker _messageProducer;

    public event EventHandler<LogMessagesReceivedEventArgs> OnMessagesReceived;

    public DbWinMessageSource()
    {
      _messageProducer = new DbWinMessageProducer(_blockingQueue);
      _messageConsumer = new DbWinMessageConsumer(_blockingQueue, MessagesReceivedCallback);
    }

    internal DbWinMessageSource(IBackgroundWorker messageProducer, IBackgroundWorker messageConsumer)
    {
      Verify.NotNull(messageProducer);
      Verify.NotNull(messageConsumer);
      
      _messageProducer = messageProducer;
      _messageConsumer = messageConsumer;
    }

    private void MessagesReceivedCallback(IEnumerable<ILogMessage> messages)
    {
      Log.Debug("MessageReceivedCallback invoked.");

      if (OnMessagesReceived != null)
        OnMessagesReceived.Invoke(this, new LogMessagesReceivedEventArgs(messages));
    }

    public void Connect()
    {
      Log.Debug("Connect invoked.");

      _messageConsumer.Start();
      _messageProducer.Start();
    }

    public void Disconnect()
    {
      Log.Debug("Disconnect invoked.");

      _messageProducer.Stop();
      _messageConsumer.Stop();
    }

    public void Dispose()
    {
      Log.Debug("Dispose invoked.");

      Disconnect();

      _messageProducer.Dispose();
      _messageConsumer.Dispose();
      _blockingQueue.Dispose();

      GC.SuppressFinalize(this);
    }
  }
}
