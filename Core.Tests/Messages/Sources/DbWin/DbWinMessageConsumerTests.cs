using System;
using System.Linq;
using Harvester.Core.Messages;
using Moq;
using Xunit;
using Harvester.Core.Messages.Sources.DbWin;
using System.Collections.Generic;
using Harvester.Core.Messages.Parsers;
using Harvester.Core.Processes;
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

namespace Harvester.Core.Tests.Messages.Sources.DbWin
{
  public class DbWinMessageConsumerTests
  {
    private readonly Mock<IDequeuer<DbWinMessage>> _dequeuer = new Mock<IDequeuer<DbWinMessage>>(MockBehavior.Strict);
    private readonly Mock<ILogMessageFactory> _logMessageFactory = new Mock<ILogMessageFactory>(MockBehavior.Strict);

    [Fact]
    public void StartShouldThrowObjectDisposedExWhenDisposed()
    {
      var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => { });

      consumer.Dispose();

      Assert.Throws<ObjectDisposedException>(() => consumer.Start());
    }

    [Fact]
    public void StartShouldThrowInvalidOperationExWhenAlreadyStarted()
    {
      IList<DbWinMessage> dbWinMessages;

      // Ensure Thread doesn't win race to second .Start() call.
      _dequeuer.Setup(mock => mock.TryDequeueAll(out dbWinMessages)).Returns(false);

      using (var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => { }))
      {
        consumer.Start();

        var ex = Assert.Throws<InvalidOperationException>(() => consumer.Start());

        Assert.Equal("DbWinMessageConsumer has already been started; unable to start DbWinMessageConsumer.", ex.Message);
      }
    }

    [Fact]
    public void StopCallIgnoredWhenNotStarted()
    {
      using (var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => { }))
        Assert.DoesNotThrow(consumer.Stop);
    }

    [Fact]
    public void StopCallIgnoredWhenDisposed()
    {
      var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => { });

      consumer.Dispose();

      Assert.DoesNotThrow(consumer.Stop);
    }

    [Fact]
    public void StopCallIgnoredWhenAlreadyStopped()
    {
      using (var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => { }))
      {
        consumer.Stop();
        Assert.DoesNotThrow(consumer.Stop);
      }
    }

    [Fact]
    public void DisposeIgnoresMultipleCalls()
    {
      using (var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => { }))
        consumer.Dispose();
    }

    [Fact]
    public void InvokesCallbackWhenMessagesDequeued()
    {
      var waitCount = 0;
      var dbWinMessage = new DbWinMessage(1000, "MyMessage");
      var logMessage = new LogMessage(dbWinMessage.Timestamp, new UnknownProcess(dbWinMessage.ProcessId), new DefaultMessageParser("MySource", dbWinMessage.Message));

// ReSharper disable RedundantAssignment
      IList<DbWinMessage> dbWinMessages = new List<DbWinMessage> { dbWinMessage };
// ReSharper restore RedundantAssignment

      _dequeuer.Setup(mock => mock.TryDequeueAll(out dbWinMessages)).Returns(true);
      _logMessageFactory.Setup(mock => mock.Create(dbWinMessage.Timestamp, dbWinMessage.ProcessId, dbWinMessage.Message)).Returns(logMessage);

      var capturedMessages = (IEnumerable<ILogMessage>)null;
      using (var consumer = new DbWinMessageConsumer(_dequeuer.Object, messages => capturedMessages = messages, _logMessageFactory.Object))
      {
        consumer.Start();


        while (capturedMessages == null && ++waitCount < 10)
          Thread.Sleep(100);

        Assert.True(capturedMessages != null);
// ReSharper disable AssignNullToNotNullAttribute
        Assert.Equal(1, capturedMessages.Count());
        Assert.Same(logMessage, capturedMessages.First());
// ReSharper restore AssignNullToNotNullAttribute
      }
    }
  }
}
