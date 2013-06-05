﻿using System;
using System.Xml;
using Harvester.Core.Processes;

/* Copyright (c) 2012-2013 CBaxter
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

namespace Harvester.Core.Messaging.Parsers
{
    internal class Log4JParser : XmlMessageParser
    {
        public Log4JParser(IRetrieveProcesses processRetriever, IHaveExtendedProperties extendedProperties)
            : base(processRetriever, "log4j", "http://logging.apache.org/log4j/")
        {
            Verify.NotNull(extendedProperties, "extendedProperties");

            //HACK: NLog now extends standard log4j XML structure with custom NLog namespace (i.e., <nlog:properties /> element). 
            NamespaceManager.AddNamespace("nlog", "http://nlog-project.org/log4j/");
        }

        public override Boolean CanParseMessage(String message)
        {
            return message != null && message.StartsWith("<log4j:event ") && message.EndsWith("</log4j:event>");
        }

        protected override SystemEventLevel GetLevel(XmlDocument document)
        {
            var level = QuerySingleValue(document, "./log4j:event/@level");
            switch (level.ToLowerInvariant())
            {
                case "fatal": return SystemEventLevel.Fatal;
                case "error": return SystemEventLevel.Error;
                case "warn": return SystemEventLevel.Warning;
                case "info": return SystemEventLevel.Information;
                case "debug": return SystemEventLevel.Debug;
                default: return SystemEventLevel.Trace;
            }
        }

        protected override String GetSource(XmlDocument document)
        {
            return QuerySingleValue(document, "./log4j:event/@logger");
        }

        protected override String GetThread(XmlDocument document)
        {
            return QuerySingleValue(document, "./log4j:event/@thread");
        }

        protected override String GetUsername(XmlDocument document)
        {
            return QuerySingleValue(document, "./log4j:event/@username");
        }

        protected override String GetMessage(XmlDocument document)
        {
            return QuerySingleValue(document, "./log4j:event/log4j:message/text()");
        }
    }
}
