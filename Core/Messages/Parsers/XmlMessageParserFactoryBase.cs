using System;
using System.Xml;
using Harvester.Core.Logging;

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
  internal abstract class XmlMessageParserFactoryBase : IMessageParserFactory
  {
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();
    private readonly XmlNamespaceManager _xmlNamespaceManager;
    private readonly XmlParserContext _xmlParserContext;

    protected XmlMessageParserFactoryBase(XmlNamespaceManager xmlNamespaceManager)
    {
      Verify.NotNull(xmlNamespaceManager);

      _xmlNamespaceManager = xmlNamespaceManager;
      _xmlParserContext = new XmlParserContext(null, _xmlNamespaceManager, null, XmlSpace.None);
    }

    public abstract Boolean CanCreateParser(String message);

    public IMessageParser Create(String message)
    {
      Verify.NotWhitespace(message);

      Log.Debug("Creating XmlMessageParser");

      var document = new XmlDocument();

      using (var reader = new XmlTextReader(message, XmlNodeType.Element, _xmlParserContext))
        document.Load(reader);

      return CreateMessageParser(document, _xmlNamespaceManager);
    }

    protected abstract IMessageParser CreateMessageParser(XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager);
  }
}
