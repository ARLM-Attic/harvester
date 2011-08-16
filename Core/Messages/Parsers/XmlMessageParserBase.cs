using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
  internal abstract class XmlMessageParserBase : IMessageParser
  {
    private readonly XmlNamespaceManager _namespaceManager;
    private readonly XmlDocument _document;
    private readonly ILog _log;

    protected XmlMessageParserBase(XmlDocument document, XmlNamespaceManager namespaceManager, ILog log)
    {
      Verify.NotNull(log);
      Verify.NotNull(document);
      Verify.NotNull(namespaceManager);

      _log = log;
      _document = document;
      _namespaceManager = namespaceManager;
    }

    public abstract LogMessageLevel GetLevel();
    public abstract String GetSource();
    public abstract String GetThread();
    public abstract String GetUsername();
    public abstract String GetMessage();
    public abstract String GetException();

    public IEnumerable<Attribute> GetAttributes()
    {
      _log.Debug("Getting message extended attributes.");

      List<Attribute> attributes = GetExtendedAttributes();

      _log.Debug("Sorting attributes by name.");

      attributes.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

      return attributes.AsReadOnly();
    }

    public String GetRawMessage()
    {
      _log.Debug("Formatting RAW message.");

      var sb = new StringBuilder();

      using (var stringWriter = new StringWriter(sb))
      using (var xmlTextWriter = new XmlTextWriter(stringWriter))
      {
        xmlTextWriter.Formatting = Formatting.Indented;

        _document.WriteTo(xmlTextWriter);
      }

      return sb.ToString();
    }

    protected String QuerySingleValue(String xPath)
    {
      return QuerySingleValue(xPath, _document);
    }

    protected String QuerySingleValue(String xPath, XmlNode node)
    {
      XmlNode result = node.SelectSingleNode(xPath, _namespaceManager);

      return result == null ? String.Empty : result.Value ?? String.Empty;
    }

    protected IEnumerable QueryMultipleValues(String xPath)
    {
      return _document.SelectNodes(xPath, _namespaceManager) ?? (IEnumerable)new XmlNode[0];
    }

    protected abstract List<Attribute> GetExtendedAttributes();
  }
}
