﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harvester.Core.Messages;
using Harvester.Core.Messages.Parsers;
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

namespace Harvester.Core.Tests.Messages.Parsers.UsingLog4JParser
{
  public class WhenCreatingParser : Log4JParserTestBase
  {
    [Fact]
    public void EmptyMessageParsedAsTrace()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event ></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(0, parser.GetAttributes().Count());
      Assert.Equal(String.Empty, parser.GetException());
      Assert.Equal(LogMessageLevel.Trace, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetMessage());
      Assert.Equal(@"<log4j:event xmlns:log4j=""http://logging.apache.org/log4j/"">" + Environment.NewLine + "</log4j:event>", parser.GetRawMessage());
      Assert.Equal(String.Empty, parser.GetSource());
      Assert.Equal(String.Empty, parser.GetThread());
      Assert.Equal(String.Empty, parser.GetUsername());
    }

    [Fact]
    public void FatalLevelMapsToFatal()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""FATAL""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Fatal, parser.GetLevel());
    }

    [Fact]
    public void ErrorLevelMapsToError()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""ERROR""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
    }

    [Fact]
    public void WarnLevelMapsToWarning()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""WARN""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Warning, parser.GetLevel());
    }

    [Fact]
    public void InfoLevelMapsToInformation()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""INFO""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
    }

    [Fact]
    public void DebugLevelMapsToDebug()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""DEBUG""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Debug, parser.GetLevel());
    }

    [Fact]
    public void UnknownLevelMapsToTrace()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""UNKNOWN""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Trace, parser.GetLevel());
    }
  }
}
