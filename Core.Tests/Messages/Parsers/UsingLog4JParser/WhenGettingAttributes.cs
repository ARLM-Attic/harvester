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

namespace Harvester.Core.Tests.Messages.Parsers.UsingLog4JParser
{
  public class WhenGettingAttributes : Log4JParserTestBase
  {
    [Fact]
    public void ReturnEmptyEnumerationWhenNoPropertiesDefined()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event level=""INFO""></log4j:event>"), XmlNamespaceManager);

      Assert.Equal(LogMessageLevel.Information, parser.GetLevel());
      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void ReturnTimestampWhenDefined()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"<log4j:event timestamp=""2011-02-12T12:43:34.6377-07:00""></log4j:event>"), XmlNamespaceManager);

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void SortByAttributeName()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                             <log4j:event domain=""Harvester.exe"" timestamp=""2011-02-12T12:43:34.6377-07:00"">
                                               <log4j:properties>
                                                 <log4j:data name=""AnAttribute"" value=""ZValue"" />
                                                 <log4j:data name=""AnotherAttribute"" value=""ZnotherValue"" />
                                               </log4j:properties>
                                             </log4j:event>"),
                                             XmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(3, attributes.Count);
      VerifyAttribute(attributes[0], "anattribute", "ZValue");
      VerifyAttribute(attributes[1], "anotherattribute", "ZnotherValue");
      VerifyAttribute(attributes[2], "timestamp", "2011-02-12T12:43:34.6377-07:00");
    }

    [Fact]
    public void IgnoreIncorrectlyFormattedProperties()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                             <log4j:event>
                                               <log4j:properties>
                                                 <log4j:data key=""AnAttribute"" value=""ZValue"" />
                                               </log4j:properties>
                                             </log4j:event>"),
                                             XmlNamespaceManager
                                           );

      Assert.Equal(0, parser.GetAttributes().Count());
    }

    [Fact]
    public void StripOffNamespacePrefix()
    {
      var parser = new Log4JMessageParser(CreateXmlDocument(@"
                                             <log4j:event>
                                               <log4j:properties>
                                                 <log4j:data name=""log4japp"" value=""Harvester.exe"" />
                                               </log4j:properties>
                                             </log4j:event>"),
                                             XmlNamespaceManager
                                           );

      var attributes = parser.GetAttributes().ToList();

      Assert.Equal(1, attributes.Count);
      VerifyAttribute(attributes[0], "app", "Harvester.exe");
    }
  }
}
