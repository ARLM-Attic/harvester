using System;
using System.Xml;
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

namespace Harvester.Core.Tests.Messages.Parsers.UsingLog4JParser
{
  public abstract class Log4JParserTestBase
  {
    protected readonly XmlParserContext XmlParserContext;
    protected readonly XmlNamespaceManager XmlNamespaceManager;

    protected Log4JParserTestBase()
    {
      XmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      XmlNamespaceManager.AddNamespace("log4j", "http://logging.apache.org/log4j/");

      XmlParserContext = new XmlParserContext(null, XmlNamespaceManager, null, XmlSpace.None);
    }

    protected XmlDocument CreateXmlDocument(String xmlFragment)
    {
      var document = new XmlDocument();

      using (var reader = new XmlTextReader(xmlFragment, XmlNodeType.Element, XmlParserContext))
        document.Load(reader);

      return document;
    }

    protected static void VerifyAttribute(Attribute attribute, String name, Object value)
    {
      Assert.Equal(name, attribute.Name);
      Assert.Equal(value, attribute.Value);
    }
  }
}
