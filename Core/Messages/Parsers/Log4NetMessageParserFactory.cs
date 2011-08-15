using System;
using System.Xml;
using NLog;

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
  internal class Log4NetMessageParserFactory : XmlMessageParserFactoryBase
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public Log4NetMessageParserFactory()
      : base(GetXmlNamespaceManager())
    { }

    private static XmlNamespaceManager GetXmlNamespaceManager()
    {
      var xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
      xmlNamespaceManager.AddNamespace("log4net", "http://logging.apache.org/log4j/");

      return xmlNamespaceManager;
    }

    public override Boolean CanCreateParser(String message)
    {
      return message != null && message.StartsWith("<log4net:event ") && message.EndsWith("</log4net:event>");
    }

    protected override IMessageParser CreateMessageParser(XmlDocument xmlDocument, XmlNamespaceManager xmlNamespaceManager)
    {
      Log.Debug("Creating Log4NetMessageParser.");

      return new Log4NetMessageParser(xmlDocument, xmlNamespaceManager);
    }
  }
}
