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
  public class Log4JMessageParserTests
  {
    private readonly XmlParserContext _xmlParserContext;
    private readonly XmlNamespaceManager _xmlNamespaceManager;

    public Log4JMessageParserTests()
    {
      _xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      _xmlNamespaceManager.AddNamespace("log4j", "http://logging.apache.org/log4j/");

      _xmlParserContext = new XmlParserContext(null, _xmlNamespaceManager, null, XmlSpace.None);
    }

    [Fact]
    public void EmptyMessageParsedAsTrace()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event ></log4j:event>"), _xmlNamespaceManager);

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
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""FATAL""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Fatal, parser.GetLevel());
    }

    [Fact]
    public void ErrorLevelMapsToError()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""ERROR""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
    }

    [Fact]
    public void WarnLevelMapsToWarning()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""WARN""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Warning, parser.GetLevel());
    }

    [Fact]
    public void InfoLevelMapsToInformation()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""INFO""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
    }

    [Fact]
    public void DebugLevelMapsToDebug()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""DEBUG""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Debug, parser.GetLevel());
    }

    [Fact]
    public void UnknownLevelMapsToTrace()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""UNKNOWN""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Trace, parser.GetLevel());
    }

    [Fact]
    public void GetMessageReturnsExceptionNodeText()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""ERROR""><log4j:message>My Message</log4j:message></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal("My Message", parser.GetMessage());
    }

    [Fact]
    public void GetMessageReturnsEmptyStringOnEmptyMessageNodeText()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""ERROR""><log4j:message /></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetMessage());
    }

    [Fact]
    public void GetMessageIgnoresIncorrectlyFormattedElement()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                              <log4j:event>
                                                <log4j:message>
                                                  <log4j:data key=""AnAttribute"" value=""ZValue"">Some Text</log4j:data>
                                                </log4j:message>
                                              </log4j:event>"),
                                             _xmlNamespaceManager
                                           );

      Assert.Equal(String.Empty, parser.GetMessage());
    }

    [Fact]
    public void GetExceptionReturnsExceptionNodeText()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""ERROR""><log4j:exception>My Exception</log4j:exception></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal("My Exception", parser.GetException());
    }

    [Fact]
    public void GetExceptionReturnsEmptyStringOnEmptyExceptionNodeText()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""ERROR""><log4j:exception /></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Error, parser.GetLevel());
      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void GetExceptionIgnoresIncorrectlyFormattedElement()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                              <log4j:event>
                                                <log4j:exception>
                                                  <log4j:data key=""AnAttribute"" value=""ZValue"">Some Text</log4j:data>
                                                </log4j:exception>
                                              </log4j:event>"),
                                             _xmlNamespaceManager
                                           );

      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void GetAttributesReturnsEmptyEnumerationWhenNoPropertiesDefined()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""INFO""></log4j:event>"), _xmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void GetAttributesReturnsTimestampWhendefined()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event timestamp=""2011-02-12T12:43:34.6377-07:00""></log4j:event>"), _xmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();
      
      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void GetAttributesSortsByAttributeName()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                              <log4j:event domain=""Harvester.exe"" timestamp=""2011-02-12T12:43:34.6377-07:00"">
                                                <log4j:properties>
                                                  <log4j:data name=""AnAttribute"" value=""ZValue"" />
                                                  <log4j:data name=""AnotherAttribute"" value=""ZnotherValue"" />
                                                </log4j:properties>
                                              </log4j:event>"), 
                                             _xmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(3, attributes.Count);
      VerifyAttribute(attributes[0], "anattribute", "ZValue");
      VerifyAttribute(attributes[1], "anotherattribute", "ZnotherValue");
      VerifyAttribute(attributes[2], "timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void GetAttributesIgnoresIncorrectlyFormattedProperties()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                              <log4j:event>
                                                <log4j:properties>
                                                  <log4j:data key=""AnAttribute"" value=""ZValue"" />
                                                </log4j:properties>
                                              </log4j:event>"),
                                             _xmlNamespaceManager
                                           );

      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void GetAttributesStripsOffNamespacePrefix()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                              <log4j:event>
                                                <log4j:properties>
                                                  <log4j:data name=""log4japp"" value=""Harvester.exe"" />
                                                </log4j:properties>
                                              </log4j:event>"),
                                             _xmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "app", "Harvester.exe");
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
