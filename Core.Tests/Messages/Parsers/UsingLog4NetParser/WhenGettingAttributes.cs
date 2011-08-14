using System.Linq;
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
  public class WhenGettingAttributes : Log4NetParserTestBase
  {
    [Fact]
    public void GetAttributesReturnsEmptyEnumerationWhenNoPropertiesDefined()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event level=""INFO""></log4net:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void GetAttributesReturnsTimestampWhendefined()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event timestamp=""2011-02-12T12:43:34.6377-07:00""></log4net:event>"), XmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "Timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void GetAttributesReturnsDomainWhendefined()
    {
      var parser = new Log4NetMessageParser(CreateXmlDocument(@"<log4net:event domain=""Harvester.exe""></log4net:event>"), XmlNamespaceManager);

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
                                             XmlNamespaceManager
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
                                             XmlNamespaceManager
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
                                             XmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "Hostname", "localhost");
    }
  }
}
