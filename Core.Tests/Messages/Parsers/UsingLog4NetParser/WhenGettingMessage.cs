using System;
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

namespace Harvester.Core.Tests.Messages.Parsers.UsingLog4NetParser
{
  public class WhenGettingMessage : Log4NetParserTestBase
  {
    [Fact]
    public void ReturnMessageNodeText()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""><log4net:message>My Message</log4net:message></log4net:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal("My Message", parser.GetMessage());
    }

    [Fact]
    public void ReturnEmptyStringOnEmptyMessageNodeText()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""><log4net:message /></log4net:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetMessage());
    }

    [Fact]
    public void IgnoreIncorrectlyFormattedElement()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"
                                             <log4net:event>
                                               <log4net:message>
                                                 <log4net:data key=""AnAttribute"" value=""ZValue"">Some Text</log4net:data>
                                               </log4net:message>
                                             </log4net:event>"),
                                             XmlNamespaceManager
                                           );

      Assert.Equal(String.Empty, parser.GetMessage());
    }
  }
}
