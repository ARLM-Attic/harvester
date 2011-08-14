using System;
using System.Collections.Generic;
using Harvester.Core.Messages;
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

namespace Harvester.Core.Tests.Messages.UsingLogMessage
{
  public class WhenRetrievingMessageProperties : LogMessageTestBase
  {
    [Fact]
    public void AttributesCachedOnFirstLookup()
    {
      IEnumerable<Attribute> attributes = new List<Attribute>();

      MessageParser.Setup(mock => mock.GetAttributes()).Returns(attributes);

      var message = new LogMessage(DateTime.Now, Process.Object, MessageParser.Object);

      Assert.Same(attributes, message.Attributes);
      Assert.Same(attributes, message.Attributes);

      MessageParser.Verify(mock => mock.GetAttributes(), Times.Once());
    }

    [Fact]
    public void ExceptionCachedOnFirstLookup()
    {
      MessageParser.Setup(mock => mock.GetException()).Returns("My Exception");

      var message = new LogMessage(DateTime.Now, Process.Object, MessageParser.Object);

      Assert.Same("My Exception", message.Exception);
      Assert.Same("My Exception", message.Exception);

      MessageParser.Verify(mock => mock.GetException(), Times.Once());
    }

    [Fact]
    public void RawMessageCachedOnFirstLookup()
    {
      MessageParser.Setup(mock => mock.GetRawMessage()).Returns("RAW Message");

      var message = new LogMessage(DateTime.Now, Process.Object, MessageParser.Object);

      Assert.Same("RAW Message", message.RawMessage);
      Assert.Same("RAW Message", message.RawMessage);

      MessageParser.Verify(mock => mock.GetRawMessage(), Times.Once());
    }
  }
}
