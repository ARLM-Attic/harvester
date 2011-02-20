using System;
using System.Collections.Generic;
using System.Linq;
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
  public class Log4JMessageParser : XmlMessageParserBase
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();

    public Log4JMessageParser(XmlDocument document, XmlNamespaceManager namespaceManager)
      : base(document, namespaceManager, Log)
    { }

    public override LogMessageLevel GetLevel() { return MapLevelToEnum(QuerySingleValue("./log4j:event/@level")); }
    public override String GetSource() { return QuerySingleValue("./log4j:event/@logger"); }
    public override String GetThread() { return QuerySingleValue("./log4j:event/@thread"); }
    public override String GetUsername() { return QuerySingleValue("./log4j:event/@username"); }
    public override String GetMessage() { return QuerySingleValue("./log4j:event/log4j:message/text()"); }
    public override String GetException() { return QuerySingleValue("./log4j:event/log4j:exception/text()"); }

    protected override List<Attribute> GetExtendedAttributes()
    {
      Log.Debug("Initializing attributes with hidden event attributes.");

      var attributes = new List<Attribute>();
      var timestamp = QuerySingleValue("./log4j:event/@timestamp");

      if (!String.IsNullOrEmpty(timestamp))
        attributes.Add(new Attribute(Localization.TimestampAttributeLabel.ToLowerInvariant(), timestamp));

      Log.Debug("Attempting to retrieve extended properties from log message.");


      attributes.AddRange(from XmlNode node in QueryMultipleValues("./log4j:event/log4j:properties/log4j:data")
                          let nameAttribute = QuerySingleValue("./@name", node).ToLowerInvariant()
                          let valueAttribute = QuerySingleValue("./@value", node)
                          where !String.IsNullOrEmpty(nameAttribute)
                          select new Attribute(EnsureNamespaceStrippedFromName(nameAttribute), valueAttribute)
                         );
      return attributes;
    }

    private static String EnsureNamespaceStrippedFromName(String attributeName)
    {
      return attributeName.StartsWith("log4j") ? attributeName.Substring(5) : attributeName.ToLower();
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
