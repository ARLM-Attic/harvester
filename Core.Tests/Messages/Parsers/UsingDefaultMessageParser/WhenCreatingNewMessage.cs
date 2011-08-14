﻿using System;
using System.Diagnostics;
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

namespace Harvester.Core.Tests.Messages.Parsers.UsingDefaultMessageParser
{
  public class WhenCreatingNewMessage
  {
    [Fact]
    public void MapSourceToExpectedProperty()
    {
      var parser = new DefaultMessageParser("MySource", "MyMessage");

      Assert.Equal("MySource", parser.GetSource());
    }

    [Fact]
    public void MapMessageToExpectedProperties()
    {
      var parser = new DefaultMessageParser("MySource", "MyMessage");

      Assert.Equal("MyMessage", parser.GetMessage());
      Assert.Equal("MyMessage", parser.GetRawMessage());
    }
    
    [Fact]
    public void UseTraceLogLevel()
    {
      var parser = new DefaultMessageParser("MySource", "MyMessage");

      Assert.Equal(LogMessageLevel.Trace, parser.GetLevel());
    }

    [Fact]
    public void DefaultEmptyProperties()
    {
      var parser = new DefaultMessageParser("MySource", "MyMessage");

      Assert.Equal(String.Empty, parser.GetThread());
      Assert.Equal(String.Empty, parser.GetUsername());
      Assert.Equal(0, parser.GetAttributes().Count());
      Assert.Equal(String.Empty, parser.GetException());
    }
  }
}