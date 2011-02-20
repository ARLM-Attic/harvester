using System;
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

namespace Harvester.Core.Tests.Messages.Parsers
{
  public class Log4JMessageParserFactoryTests
  {
    [Fact]
    public void CanCreateParserReturnsFalseOnNullMessage()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.False(factory.CanCreateParser(null));
    }

    [Fact]
    public void CanCreateParserReturnsFalseOnEmptyMessage()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.False(factory.CanCreateParser(null));
    }

    [Fact]
    public void CanCreateParserReturnsFalseOnWhitespaceMessage()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.False(factory.CanCreateParser(String.Empty));
    }

    [Fact]
    public void CanCreateParserReturnsFalseWhenMessageDoesNotStartWithOpenTag()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.False(factory.CanCreateParser("Stuff Before <log4j:event logger"));
    }

    [Fact]
    public void CanCreateParserReturnsFalseWhenMessageDoesNotEndWithCloseTag()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.False(factory.CanCreateParser("<log4j:event ></log4j:event> After Stuff"));
    }

    [Fact]
    public void CanCreateParserReturnsTrueWhenMessageMatchesLog4NetXmlLayoutElement()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.True(factory.CanCreateParser("<log4j:event ></log4j:event>"));
    }

    [Fact]
    public void CreateReturnsXmlLayoutParser()
    {
      var factory = new Log4JMessageParserFactory();

      Assert.IsType(typeof(Log4JMessageParser), factory.Create("<log4j:event ></log4j:event>"));
    }
  }
}
