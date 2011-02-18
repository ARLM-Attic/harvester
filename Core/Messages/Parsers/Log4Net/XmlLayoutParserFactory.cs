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

namespace Harvester.Core.Messages.Parsers.Log4Net
{
  public class XmlLayoutParserFactory : IMessageParserFactory
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly XmlParserContext _xmlParserContext;
    private readonly XmlNamespaceManager _xmlNamespaceManager;

    public XmlLayoutParserFactory()
    {
      Log.Debug("Creating XmlLayoutParserFactory.");

      _xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      _xmlNamespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
      _xmlNamespaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
      _xmlNamespaceManager.AddNamespace("log4net", "http://logging.apache.org/log4net/");

      _xmlParserContext = new XmlParserContext(null, _xmlNamespaceManager, null, XmlSpace.None);
    }

    public Boolean CanCreateParser(String message)
    {
      const String xmlFragmentStart = "<log4net:event ";
      const String xmlFragmentEnd = "</log4net:event>";

      String trimmedMessage = (message ?? String.Empty).Trim();
      Boolean canParserMessage = !String.IsNullOrEmpty(trimmedMessage) && trimmedMessage.StartsWith(xmlFragmentStart) && trimmedMessage.EndsWith(xmlFragmentEnd);

      Log.DebugFormat("CanCreateParser: {0}.", canParserMessage);

      return canParserMessage;
    }

    public IMessageParser Create(String message)
    {
      Verify.NotWhitespace(message);

      var document = new XmlDocument();

      Log.Debug("Loading XmlDocument.");

      using (var reader = new XmlTextReader(message, XmlNodeType.Element, _xmlParserContext))
        document.Load(reader);

      return new XmlLayoutParser(document, _xmlNamespaceManager);
    }
  }
}
