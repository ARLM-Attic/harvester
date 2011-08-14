using Harvester.Core.Messages;
using Harvester.Core.Messages.Parsers;
using Harvester.Core.Processes;
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

namespace Harvester.Core.Tests.Messages.UsingLogMessage
{
  public abstract class LogMessageTestBase
  {
    protected readonly Mock<IMessageParser> MessageParser = new Mock<IMessageParser>(MockBehavior.Strict);
    protected readonly Mock<IProcess> Process = new Mock<IProcess>(MockBehavior.Strict);

    protected LogMessageTestBase()
    {
      SetupProcessExpectations();
      SetupBaseParserExpectations();
    }

    private void SetupProcessExpectations()
    {
      Process.SetupGet(mock => mock.Id).Returns(2000);
      Process.SetupGet(mock => mock.Name).Returns("Harvester.exe");
    }

    private void SetupBaseParserExpectations()
    {
      MessageParser.Setup(mock => mock.GetThread()).Returns("Main");
      MessageParser.Setup(mock => mock.GetUsername()).Returns("CBaxter");
      MessageParser.Setup(mock => mock.GetMessage()).Returns("My Message");
      MessageParser.Setup(mock => mock.GetSource()).Returns("Harvester.Logger");
      MessageParser.Setup(mock => mock.GetLevel()).Returns(LogMessageLevel.Information);
    }
  }
}
