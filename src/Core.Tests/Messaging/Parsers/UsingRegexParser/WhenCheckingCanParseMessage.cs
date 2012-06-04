﻿using Harvester.Core.Messaging.Parsers;
using Harvester.Core.Processes;
using Moq;
using Xunit;

/* Copyright (c) 2012 CBaxter
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

namespace Harvester.Core.Tests.Messaging.Parsers.UsingRegexParser
{
    public class WhenCheckingCanParseMessage
    {
        private readonly Mock<IRetrieveProcesses> processRetriever = new Mock<IRetrieveProcesses>();
        private readonly IParseMessages messageParser;

        public WhenCheckingCanParseMessage()
        {
            var extendedProperties = new FakeExtendedProperties
                                         {
                                             { "pattern", "ExactMatch" },
                                             { "options", "None" }
                                         };

            messageParser = new RegexParser(processRetriever.Object, extendedProperties);
        }

        [Fact]
        public void ReturnsFalseIfNull()
        {
            Assert.False(messageParser.CanParseMessage(null));
        }

        [Fact]
        public void ReturnsFalseIfDoesNotMatchPattern()
        {
            Assert.False(messageParser.CanParseMessage("NotMatch"));
        }

        [Fact]
        public void ReturnsTrueIfMatchesPattern()
        {
            Assert.True(messageParser.CanParseMessage("ExactMatch"));
        }
    }
}
