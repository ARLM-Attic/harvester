using System;
using System.Collections.Generic;
using Harvester.Core.Messages;
using Harvester.Core.Messages.Parsers;
using Harvester.Core.Processes;
using Moq;
using Xunit;
using Attribute = Harvester.Core.Messages.Attribute;

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

namespace Harvester.Core.Tests.Messages
{
  public class LogMessageTests
  {
    private readonly Mock<IMessageParser> _messageParser = new Mock<IMessageParser>(MockBehavior.Strict);
    private readonly Mock<IProcess> _process = new Mock<IProcess>(MockBehavior.Strict);

    [Fact]
    public void CtorBuildNewLogMessage()
    {
      SetupProcessExpectations();
      SetupBaseParserExpectations();

      var now = DateTime.Now;
      var message = new LogMessage(now, _process.Object, _messageParser.Object);

      Assert.True(message.MessageId > 0U);
      Assert.Equal(now, message.Timestamp);
      Assert.Equal(LogMessageLevel.Information, message.Level);
      Assert.Equal(2000, message.ProcessId);
      Assert.Equal("Harvester.exe", message.ProcessName);
      Assert.Equal("Main", message.Thread);
      Assert.Equal("CBaxter", message.Username);
      Assert.Equal("Harvester.Logger", message.Source);
      Assert.Equal("My Message", message.Message);
    }

    [Fact]
    public void AttributesCachedOnFirstLookup()
    {
      SetupProcessExpectations();
      SetupBaseParserExpectations();

      IEnumerable<Attribute> attributes = new List<Attribute>();

      _messageParser.Setup(mock => mock.GetAttributes()).Returns(attributes);

      var message = new LogMessage(DateTime.Now, _process.Object, _messageParser.Object);

      Assert.Same(attributes, message.Attributes);
      Assert.Same(attributes, message.Attributes);

      _messageParser.Verify(mock => mock.GetAttributes(), Times.Once());
    }

    [Fact]
    public void ExceptionCachedOnFirstLookup()
    {
      SetupProcessExpectations();
      SetupBaseParserExpectations();

      _messageParser.Setup(mock => mock.GetException()).Returns("My Exception");

      var message = new LogMessage(DateTime.Now, _process.Object, _messageParser.Object);

      Assert.Same("My Exception", message.Exception);
      Assert.Same("My Exception", message.Exception);

      _messageParser.Verify(mock => mock.GetException(), Times.Once());
    }

    [Fact]
    public void RawMessageCachedOnFirstLookup()
    {
      SetupProcessExpectations();
      SetupBaseParserExpectations();

      _messageParser.Setup(mock => mock.GetRawMessage()).Returns("RAW Message");

      var message = new LogMessage(DateTime.Now, _process.Object, _messageParser.Object);

      Assert.Same("RAW Message", message.RawMessage);
      Assert.Same("RAW Message", message.RawMessage);

      _messageParser.Verify(mock => mock.GetRawMessage(), Times.Once());
    }

    private void SetupProcessExpectations()
    {
      _process.SetupGet(mock => mock.Id).Returns(2000);
      _process.SetupGet(mock => mock.Name).Returns("Harvester.exe");
    }

    private void SetupBaseParserExpectations()
    {
      _messageParser.Setup(mock => mock.GetThread()).Returns("Main");
      _messageParser.Setup(mock => mock.GetUsername()).Returns("CBaxter");
      _messageParser.Setup(mock => mock.GetMessage()).Returns("My Message");
      _messageParser.Setup(mock => mock.GetSource()).Returns("Harvester.Logger");
      _messageParser.Setup(mock => mock.GetLevel()).Returns(LogMessageLevel.Information);
    }
  }
}
