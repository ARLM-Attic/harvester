using System;
using Harvester.Core.Messages;
using Harvester.Core.Messages.Parsers;
using Harvester.Core.Processes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
  [TestClass]
  public class LogMessageFactoryTests
  {

    private readonly Mock<IProcess> _process = new Mock<IProcess>(MockBehavior.Strict);
    private readonly Mock<IMessageParser> _messageParser = new Mock<IMessageParser>(MockBehavior.Strict);
    private readonly Mock<IProcessRetriever> _processRetriever = new Mock<IProcessRetriever>(MockBehavior.Strict);
    private readonly Mock<IMessageParserFactory> _messageParserFactory = new Mock<IMessageParserFactory>(MockBehavior.Strict);

    [TestMethod]
    public void CtorCreatesNewFactory()
    {
      Assert.IsNotNull(new LogMessageFactory("Test.Source"));
    }

    [TestMethod]
    public void CreateUsesXmlLayoutParserWhenCanCreateReturnsTrue()
    {
      const String message = "<log4net:event ></log4net:event>";

      var factory = new LogMessageFactory("Test Source", _processRetriever.Object, _messageParserFactory.Object);
      var now = DateTime.Now;

      SetupProcessExpectations();
      SetupMessageParserExpectations();

      _messageParserFactory.Setup(mock => mock.Create(message)).Returns(_messageParser.Object);
      _messageParserFactory.Setup(mock => mock.CanCreateParser(message)).Returns(true);
      _processRetriever.Setup(mock => mock.GetProcessById(2000)).Returns(_process.Object);

      ILogMessage logMessage = factory.Create(now, 2000, message);

      Assert.IsNotNull(logMessage);
    }

    [TestMethod]
    public void CreateUsesDefaultParserWhenCanCreateReturnsFalse()
    {
      const String message = "<log4net:event ></log4net:event>";

      var factory = new LogMessageFactory("Test Source", _processRetriever.Object, _messageParserFactory.Object);
      var now = DateTime.Now;

      SetupProcessExpectations();

      _messageParserFactory.Setup(mock => mock.CanCreateParser(message)).Returns(false);
      _processRetriever.Setup(mock => mock.GetProcessById(2000)).Returns(_process.Object);

      ILogMessage logMessage = factory.Create(now, 2000, message);

      Assert.IsNotNull(logMessage);
      Assert.AreEqual("Test Source", logMessage.Source);
    }

    private void SetupProcessExpectations()
    {
      _process.SetupGet(mock => mock.Id).Returns(2000);
      _process.SetupGet(mock => mock.Name).Returns("Harvester.exe");
    }

    private void SetupMessageParserExpectations()
    {
      _messageParser.Setup(mock => mock.GetThread()).Returns("Main");
      _messageParser.Setup(mock => mock.GetUsername()).Returns("CBaxter");
      _messageParser.Setup(mock => mock.GetMessage()).Returns("My Message");
      _messageParser.Setup(mock => mock.GetSource()).Returns("Harvester.Logger");
      _messageParser.Setup(mock => mock.GetLevel()).Returns(LogMessageLevel.Information);
    }
  }
}
