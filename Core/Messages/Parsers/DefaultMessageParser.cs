﻿using System;
using System.Collections.Generic;
using NLog;

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

namespace Harvester.Core.Messages.Parsers
{
  internal class DefaultMessageParser : IMessageParser
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly static IEnumerable<Attribute> EmptyAttributes = new List<Attribute>().AsReadOnly();

    private readonly String _message;
    private readonly String _source;

    public DefaultMessageParser(String source, String message)
    {
      Log.Debug("Creating DefaultMessageParser.");

      Verify.NotWhitespace(source);
      Verify.NotNull(message);

      _message = message;
      _source = source;
    }

    public LogMessageLevel GetLevel()
    {
      return LogMessageLevel.Trace;
    }

    public String GetSource()
    {
      return _source;
    }

    public String GetThread()
    {
      return String.Empty; 
    }
    
    public String GetUsername()
    {
      return String.Empty; 
    }

    public String GetMessage()
    {
      return _message;
    }

    public IEnumerable<Attribute> GetAttributes()
    {
      return EmptyAttributes;
    }

    public String GetException()
    {
      return String.Empty;
    }

    public String GetRawMessage()
    {
      return _message;
    }
  }
}
