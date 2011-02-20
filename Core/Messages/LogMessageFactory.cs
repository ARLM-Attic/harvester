using System;
using System.Linq;
using Harvester.Core.Logging;
using Harvester.Core.Messages.Parsers;
using Harvester.Core.Processes;

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

namespace Harvester.Core.Messages
{
  public class LogMessageFactory : ILogMessageFactory
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly IMessageParserFactory[] _messageParserFactories;
    private readonly IProcessRetriever _processRetriever;
    private readonly String _source;

    public LogMessageFactory(String source)
      : this(source, new ProcessRetriever(), new Log4NetMessageParserFactory(), new Log4JMessageParserFactory())
    { }

    internal LogMessageFactory(String source, IProcessRetriever processRetriever, params IMessageParserFactory[] messageParserFactories)
    {
      Verify.NotWhitespace(source);
      Verify.NotNull(processRetriever);
      Verify.NotNull(messageParserFactories);

      _source = source;
      _processRetriever = processRetriever;
      _messageParserFactories = messageParserFactories;
    }

    public ILogMessage Create(DateTime timestamp, Int32 processId, String message)
    {
      Log.DebugFormat("Attempting to create new LogMessage with: Date={0}; ProcessId={1}; Message={2};", timestamp, processId, message);

      Verify.GreaterThanZero(processId);
      
      String messageText = (message ?? String.Empty).Trim();
      IProcess process = _processRetriever.GetProcessById(processId);

      IMessageParserFactory messageParserFactory = _messageParserFactories.FirstOrDefault(factory => factory.CanCreateParser(messageText));
      IMessageParser messageParser = messageParserFactory != null ? messageParserFactory.Create(messageText) : new DefaultMessageParser(_source, messageText);

      return new LogMessage(timestamp, process, messageParser);
    }
  }
}
