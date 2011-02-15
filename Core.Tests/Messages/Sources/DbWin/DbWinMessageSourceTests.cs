using Harvester.Core.Messages.Sources.DbWin;
using Moq;
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

namespace Harvester.Core.Tests.Messages.Sources.DbWin
{
  public class DbWinMessageSourceTests
  {
    private readonly Mock<IBackgroundWorker> _producer = new Mock<IBackgroundWorker>(MockBehavior.Strict);
    private readonly Mock<IBackgroundWorker> _consumer = new Mock<IBackgroundWorker>(MockBehavior.Strict);

    [Fact]
    public void CtorShouldCreateNewDbWinMessageSource()
    {
      // Unit Test cannot be run while instance of Harvester running (Will thrown Win32 Error 183 - ErrorAlreadyExists)
      using (new DbWinMessageSource())
      { }
    }

    [Fact]
    public void ConnectStartsProducerAndConsumer()
    {
      var source = new DbWinMessageSource(_producer.Object, _consumer.Object);

      _consumer.Setup(mock => mock.Start());
      _producer.Setup(mock => mock.Start());

      source.Connect();
    }

    [Fact]
    public void DisconnectStopsProducerAndConsumer()
    {
      var source = new DbWinMessageSource(_producer.Object, _consumer.Object);

      _consumer.Setup(mock => mock.Stop());
      _producer.Setup(mock => mock.Stop());

      source.Disconnect();
    }

    [Fact]
    public void DisposeDisconnectsAndThenDisposesProducerAndConsumer()
    {
      var source = new DbWinMessageSource(_producer.Object, _consumer.Object);

      _consumer.Setup(mock => mock.Stop());
      _consumer.Setup(mock => mock.Dispose());
      _producer.Setup(mock => mock.Stop());
      _producer.Setup(mock => mock.Dispose());

      source.Dispose();
    }
  }
}
