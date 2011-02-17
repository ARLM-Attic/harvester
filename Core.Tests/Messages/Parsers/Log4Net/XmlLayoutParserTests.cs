using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Harvester.Core.Messages;
using Harvester.Core.Messages.Parsers;
using Harvester.Core.Messages.Parsers.Log4Net;
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

namespace Harvester.Core.Tests.Messages.Parsers.Log4Net
{
  public class XmlLayoutParserTests
  {
    private const String FatalMessage = @"<log4net:event logger=""Harvester.Test.Logger"" timestamp=""2011-02-12T12:43:34.6377-07:00"" level=""FATAL"" thread=""3"" domain=""Harvester.exe"" username=""CBaxter"">
                                            <log4net:message>My FATAL Message</log4net:message>
                                            <log4net:exception>My Exception</log4net:exception>
                                            <log4net:properties>
                                              <log4net:data name=""log4net:HostName"" value=""localhost"" />
                                              <log4net:data name=""AnotherAttribute"" value=""AnotherValue"" />
                                            </log4net:properties>
                                          </log4net:event>";

    private const String ErrorMessage = @"<log4net:event logger=""Harvester.Test.Logger"" timestamp=""2011-02-12T12:43:34.6377-07:00"" level=""ERROR"" thread=""3"" domain=""Harvester.exe"" username=""CBaxter"">
                                            <log4net:message>My ERROR Message</log4net:message>
                                            <log4net:exception>My Exception</log4net:exception>
                                            <log4net:properties>
                                              <log4net:data name=""log4net:HostName"" value=""localhost"" />
                                            </log4net:properties>
                                          </log4net:event>";

    private const String WarnMessage = @"<log4net:event logger=""Harvester.Test.Logger"" timestamp=""2011-02-12T12:43:34.6377-07:00"" level=""WARN"" thread=""3"" domain=""Harvester.exe"" username=""CBaxter"">
                                           <log4net:message>My WARN Message</log4net:message>
                                           <log4net:properties>
                                             <log4net:data name=""log4net:HostName"" value=""localhost"" />
                                           </log4net:properties>
                                         </log4net:event>";

    private const String InfoMessage = @"<log4net:event logger=""Harvester.Test.Logger"" timestamp=""2011-02-12T12:43:34.6377-07:00"" level=""INFO"" thread=""3"" domain=""Harvester.exe"" username=""CBaxter"">
                                           <log4net:message>My INFO Message</log4net:message>
                                           <log4net:properties>
                                             <log4net:data name=""log4net:HostName"" value=""localhost"" />
                                           </log4net:properties>
                                         </log4net:event>";

    private const String DebugMessage = @"<log4net:event logger=""Harvester.Test.Logger"" timestamp=""2011-02-12T12:43:34.6377-07:00"" level=""DEBUG"" thread=""3"" domain=""Harvester.exe"" username=""CBaxter"">
                                            <log4net:message>My DEBUG Message</log4net:message>
                                            <log4net:properties>
                                              <log4net:data name=""log4net:HostName"" value=""localhost"" />
                                            </log4net:properties>
                                          </log4net:event>";

    private const String TraceMessage = @"<log4net:event logger=""Harvester.Test.Logger"" timestamp=""2011-02-12T12:43:34.6377-07:00"" thread=""3"" domain=""Harvester.exe"" username=""CBaxter"">
                                            <log4net:message>My TRACE Message</log4net:message>
                                          </log4net:event>";

    private readonly XmlParserContext _xmlParserContext;
    private readonly XmlNamespaceManager _xmlNamespaceManager;

    public XmlLayoutParserTests()
    {
      _xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      _xmlNamespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
      _xmlNamespaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
      _xmlNamespaceManager.AddNamespace("log4net", "http://logging.apache.org/log4net/");

      _xmlParserContext = new XmlParserContext(null, _xmlNamespaceManager, null, XmlSpace.None);
    }

    [Fact]
    public void ParsesFatalMessageWithException()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(FatalMessage), _xmlNamespaceManager);

      VerifyParser(parser, LogMessageLevel.Fatal, "My FATAL Message", FatalMessage);

      Assert.Equal("My Exception", parser.GetException());
    }

    [Fact]
    public void ParsesErrorMessageWithException()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(ErrorMessage), _xmlNamespaceManager);

      VerifyParser(parser, LogMessageLevel.Error, "My ERROR Message", ErrorMessage);

      Assert.Equal("My Exception", parser.GetException());
    }

    [Fact]
    public void ParsesWarnMessageWithNoException()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(WarnMessage), _xmlNamespaceManager);

      VerifyParser(parser, LogMessageLevel.Warning, "My WARN Message", WarnMessage);

      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void ParsesInfoMessageWithNoException()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(InfoMessage), _xmlNamespaceManager);

      VerifyParser(parser, LogMessageLevel.Information, "My INFO Message", InfoMessage);

      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void ParsesDebugMessageWithNoException()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(DebugMessage), _xmlNamespaceManager);

      VerifyParser(parser, LogMessageLevel.Debug, "My DEBUG Message", DebugMessage);

      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void ParsesUnknownMessageWithNoExceptionAsTrace()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(TraceMessage), _xmlNamespaceManager);

      VerifyParser(parser, LogMessageLevel.Trace, "My TRACE Message", TraceMessage);

      Assert.Equal(String.Empty, parser.GetException());
    }

    [Fact]
    public void GetAttributesHandlesIgnoresMissingExtendedPropertiesElement()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(TraceMessage), _xmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);

      VerifyAttribute(attributes[0], "Domain", "Harvester.exe");
    }

    [Fact]
    public void GetAttributesReturnsSortedList()
    {
      var parser = new XmlLayoutParser(CreateXmlDocument(FatalMessage), _xmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(3, attributes.Count);

      VerifyAttribute(attributes[0], "AnotherAttribute", "AnotherValue");
      VerifyAttribute(attributes[1], "Domain", "Harvester.exe");
      VerifyAttribute(attributes[2], "HostName", "localhost");
    }

    private XmlDocument CreateXmlDocument(String xmlFragment)
    {
      var document = new XmlDocument();

      using (var reader = new XmlTextReader(xmlFragment, XmlNodeType.Element, _xmlParserContext))
        document.Load(reader);

      return document;
    }

    private String FormatXml(String rawMessage)
    {
      var document = CreateXmlDocument(rawMessage);

      var sb = new StringBuilder();

      using (var stringWriter = new StringWriter(sb))
      using (var xmlTextWriter = new XmlTextWriter(stringWriter))
      {
        xmlTextWriter.Formatting = Formatting.Indented;
        document.WriteTo(xmlTextWriter);
      }

      return sb.ToString();
    }

    private void VerifyParser(IMessageParser parser, LogMessageLevel level, String message, String rawMessage)
    {
      Assert.Equal(level, parser.GetLevel());
      Assert.Equal("Harvester.Test.Logger", parser.GetSource());
      Assert.Equal("3", parser.GetThread());
      Assert.Equal("CBaxter", parser.GetUsername());
      Assert.Equal(message, parser.GetMessage());
      Assert.Equal(FormatXml(rawMessage), parser.GetRawMessage());
    }

    private static void VerifyAttribute(Attribute attribute, String name, Object value)
    {
      Assert.Equal(name, attribute.Name);
      Assert.Equal(value, attribute.Value);
    }
  }
}
