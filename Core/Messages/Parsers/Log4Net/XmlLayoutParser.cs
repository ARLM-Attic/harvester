using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

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
  public class XmlLayoutParser : IMessageParser
  {
    private readonly XmlDocument _document;
    private readonly XmlNamespaceManager _namespaceManager;

    public XmlLayoutParser(XmlDocument document, XmlNamespaceManager namespaceManager)
    {
      Verify.NotNull(document);
      Verify.NotNull(namespaceManager);

      _document = document;
      _namespaceManager = namespaceManager;
    }

    public LogMessageLevel GetLevel()
    {
      return MapLevelToEnum(GetEventAttributeOrDefault("level", String.Empty));
    }

    public String GetSource()
    {
      return GetEventAttributeOrDefault("logger", String.Empty);
    }

    public String GetThread()
    {
      return GetEventAttributeOrDefault("thread", String.Empty);
    }

    public String GetUsername()
    {
      return GetEventAttributeOrDefault("username", String.Empty);
    }

    public String GetMessage()
    {
      XmlNode node = _document.SelectSingleNode("./log4net:event/log4net:message/text()", _namespaceManager);

      return node == null ? String.Empty : node.Value ?? String.Empty;
    }

    public IEnumerable<Attribute> GetAttributes()
    {
      var attributes = new List<Attribute>
                         {
                           new Attribute(Localization.DomainAttributeLabel, GetEventAttributeOrDefault("domain", String.Empty))
                         };

      XmlNodeList nodes = _document.SelectNodes("./log4net:event/log4net:properties/log4net:data", _namespaceManager);
      if (nodes != null)
      {
        attributes.AddRange(from XmlNode node in nodes
                            let nameAttribute = GetNodeValueOrDefault(node.SelectSingleNode("./@name"), String.Empty)
                            let valueAttribute = GetNodeValueOrDefault(node.SelectSingleNode("./@value"), String.Empty)
                            where !String.IsNullOrEmpty(nameAttribute)
                            select new Attribute(EnsureNamespaceStrippedFromName(nameAttribute), valueAttribute)
                           );
      }

      attributes.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

      return attributes.AsReadOnly();
    }

    private static String EnsureNamespaceStrippedFromName(String attributeName)
    {
      return attributeName.StartsWith("log4net:") ? attributeName.Substring(8) : attributeName;
    }

    public String GetException()
    {
      XmlNode node = _document.SelectSingleNode("./log4net:event/log4net:exception/text()", _namespaceManager);

      return node == null ? String.Empty : node.Value ?? String.Empty;
    }

    public String GetRawMessage()
    {
      var sb = new StringBuilder();

      using (var stringWriter = new StringWriter(sb))
      using (var xmlTextWriter = new XmlTextWriter(stringWriter))
      {
        xmlTextWriter.Formatting = Formatting.Indented;

        _document.WriteTo(xmlTextWriter);
      }

      return sb.ToString();
    }

    private String GetEventAttributeOrDefault(String attribute, String defaultValue)
    {
      return GetNodeValueOrDefault(_document.SelectSingleNode("./log4net:event/@" + attribute, _namespaceManager), defaultValue);
    }

    private static String GetNodeValueOrDefault(XmlNode node, String defaultValue)
    {
      return node == null ? defaultValue : node.Value ?? defaultValue;
    }

    private static LogMessageLevel MapLevelToEnum(String level)
    {
      switch (level.ToLowerInvariant())
      {
        case "fatal": return LogMessageLevel.Fatal;
        case "error": return LogMessageLevel.Error;
        case "warn": return LogMessageLevel.Warning;
        case "info": return LogMessageLevel.Information;
        case "debug": return LogMessageLevel.Debug;
        default: return LogMessageLevel.Trace;
      }
    }
  }
}
