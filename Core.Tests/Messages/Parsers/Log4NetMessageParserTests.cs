using System;
using System.Linq;
using System.Xml;
using Harvester.Core.Messages;
using Harvester.Core.Messages.Parsers;
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

namespace Harvester.Core.Tests.Messages.Parsers
{
  public class Log4NetMessageParserTests
  {
    private readonly XmlParserContext _xmlParserContext;
    private readonly XmlNamespaceManager _xmlNamespaceManager;

    public Log4NetMessageParserTests()
    {
      _xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      _xmlNamespaceManager.AddNamespace("log4net", "http://logging.apache.org/log4net/");

      _xmlParserContext = new XmlParserContext(null, _xmlNamespaceManager, null, XmlSpace.None);
    }

    [Fact]
    public void EmptyMessageParsedAsTrace()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event ></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(0, parser.GetAttributes().Count());
      Assert.Equal(String.Empty, parser.GetException());
      Assert.Equal(LogMessageLevel.Trace, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetMessage());
      Assert.Equal(@"<log4net:event xmlns:log4net=""http://logging.apache.org/log4net/"">" + Environment.NewLine + "</log4net:event>", parser.GetRawMessage());
      Assert.Equal(String.Empty, parser.GetSource());
      Assert.Equal(String.Empty, parser.GetThread());
      Assert.Equal(String.Empty, parser.GetUsername());
    }

    [Fact]
    public void FatalLevelMapsToFatal()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""FATAL""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Fatal, parser.GetLevel());
    }

    [Fact]
    public void ErrorLevelMapsToError()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
    }

    [Fact]
    public void WarnLevelMapsToWarning()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""WARN""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Warning, parser.GetLevel());
    }

    [Fact]
    public void InfoLevelMapsToInformation()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""INFO""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
    }

    [Fact]
    public void DebugLevelMapsToDebug()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""DEBUG""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Debug, parser.GetLevel());
    }

    [Fact]
    public void UnknownLevelMapsToTrace()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""UNKNOWN""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Trace, parser.GetLevel());
    }

    [Fact]
    public void GetMessageReturnsExceptionNodeText()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""><log4net:message>My Message</log4net:message></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal("My Message", parser.GetMessage());
    }

    [Fact]
    public void GetMessageReturnsEmptyStringOnEmptyMessageNodeText()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""><log4net:message /></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetMessage());
    }

    [Fact]
    public void GetMessageIgnoresIncorrectlyFormattedElement()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"
                                              <log4net:event>
                                                <log4net:message>
                                                  <log4net:data key=""AnAttribute"" value=""ZValue"">Some Text</log4net:data>
                                                </log4net:message>
                                              </log4net:event>"),
                                             _xmlNamespaceManager
                                           );

      Assert.Equal(String.Empty, parser.GetMessage());
    }

    [Fact]
    public void GetExceptionReturnsExceptionNodeText()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""><log4net:exception>My Exception</log4net:exception></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal("My Exception", parser.GetException());
    }

    [Fact]
    public void GetExceptionReturnsEmptyStringOnEmptyExceptionNodeText()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""ERROR""><log4net:exception /></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void GetExceptionIgnoresIncorrectlyFormattedElement()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"
                                              <log4net:event>
                                                <log4net:exception>
                                                  <log4net:data key=""AnAttribute"" value=""ZValue"">Some Text</log4net:data>
                                                </log4net:exception>
                                              </log4net:event>"),
                                             _xmlNamespaceManager
                                           );

      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void GetAttributesReturnsEmptyEnumerationWhenNoPropertiesDefined()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""INFO""></log4net:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void GetAttributesReturnsTimestampWhendefined()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event timestamp=""2011-02-12T12:43:34.6377-07:00""></log4net:event>"), _xmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();
      
      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "Timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void GetAttributesReturnsDomainWhendefined()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event domain=""Harvester.exe""></log4net:event>"), _xmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "Domain", "Harvester.exe");
    }

    [Fact]
    public void GetAttributesSortsByAttributeName()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"
                                              <log4net:event domain=""Harvester.exe"" timestamp=""2011-02-12T12:43:34.6377-07:00"">
                                                <log4net:properties>
                                                  <log4net:data name=""AnAttribute"" value=""ZValue"" />
                                                  <log4net:data name=""AnotherAttribute"" value=""ZnotherValue"" />
                                                </log4net:properties>
                                              </log4net:event>"), 
                                             _xmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(4, attributes.Count);
      VerifyAttribute(attributes[0], "AnAttribute", "ZValue");
      VerifyAttribute(attributes[1], "AnotherAttribute", "ZnotherValue");
      VerifyAttribute(attributes[2], "Domain", "Harvester.exe");
      VerifyAttribute(attributes[3], "Timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void GetAttributesIgnoresIncorrectlyFormattedProperties()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"
                                              <log4net:event>
                                                <log4net:properties>
                                                  <log4net:data key=""AnAttribute"" value=""ZValue"" />
                                                </log4net:properties>
                                              </log4net:event>"),
                                             _xmlNamespaceManager
                                           );

      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void GetAttributesStripsOffNamespacePrefix()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"
                                              <log4net:event>
                                                <log4net:properties>
                                                  <log4net:data name=""log4net:Hostname"" value=""localhost"" />
                                                </log4net:properties>
                                              </log4net:event>"),
                                             _xmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "Hostname", "localhost");
    }

    private XmlDocument CreateXmlDocument(String xmlFragment)
    {
      var document = new XmlDocument();

      using (var reader = new XmlTextReader(xmlFragment, XmlNodeType.Element, _xmlParserContext))
        document.Load(reader);

      return document;
    }

    private static void VerifyAttribute(Attribute attribute, String name, Object value)
    {
      Assert.Equal(name, attribute.Name);
      Assert.Equal(value, attribute.Value);
    }
  }
}
