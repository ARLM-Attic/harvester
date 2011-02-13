using System;
using System.Collections.Generic;
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
  public class LogMessage : ILogMessage
  {
    private static readonly Sequence Sequence = new Sequence();

    private readonly IMessageParser _messageParser;
    private readonly LogMessageLevel _level;
    private readonly DateTime _timestamp;
    private readonly UInt32 _messageId;
    private readonly Int32 _processId;
    private readonly String _processName;
    private readonly String _thread;
    private readonly String _username;
    private readonly String _source;
    private readonly String _message;

    private String _exception;
    private String _rawMessage;
    private IEnumerable<Attribute> _attributes;

    public LogMessageLevel Level { get { return _level; } }
    public DateTime Timestamp { get { return _timestamp; } }
    public UInt32 MessageId { get { return _messageId; } }
    public Int32 ProcessId { get { return _processId; } }
    public String ProcessName { get { return _processName; } }
    public String Thread { get { return _thread; } }
    public String Username { get { return _username; } }
    public String Source { get { return _source; } }
    public String Message { get { return _message; } }

    public String Exception { get { return _exception ?? (_exception = _messageParser.GetException()); } }
    public String RawMessage { get { return _rawMessage ?? (_rawMessage = _messageParser.GetRawMessage()); } }
    public IEnumerable<Attribute> Attributes { get { return _attributes ?? (_attributes = _messageParser.GetAttributes()); } }

    public LogMessage(DateTime timestamp, IProcess process, IMessageParser messageParser)
    {
      Verify.NotNull(messageParser);
      Verify.NotNull(process);

      _messageId = Sequence.GetNextId();
      _timestamp = timestamp;
      _processId = process.Id;
      _processName = process.Name;

      _level = messageParser.GetLevel();
      _thread = messageParser.GetThread();
      _source = messageParser.GetSource();
      _message = messageParser.GetMessage();
      _username = messageParser.GetUsername();

      _messageParser = messageParser;
    }
  }
}
