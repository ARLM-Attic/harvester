using System;
using Harvester.Core.Messages.Parsers.Log4Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

namespace Harvester.Core.Tests.Messages.Parsers.Log4Net
{
  [TestClass]
  public class XmlLayoutParserFactoryTests
  {
    [TestMethod]
    public void CanCreateParserReturnsFalseOnNullMessage()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsFalse(factory.CanCreateParser(null));
    }

    [TestMethod]
    public void CanCreateParserReturnsFalseOnEmptyMessage()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsFalse(factory.CanCreateParser(null));
    }

    [TestMethod]
    public void CanCreateParserReturnsFalseOnWhitespaceMessage()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsFalse(factory.CanCreateParser(String.Empty));
    }

    [TestMethod]
    public void CanCreateParserReturnsFalseWhenMessageDoesNotStartWithOpenTag()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsFalse(factory.CanCreateParser("Stuff Before <log4net:event logger"));
    }

    [TestMethod]
    public void CanCreateParserReturnsFalseWhenMessageDoesNotEndWithCloseTag()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsFalse(factory.CanCreateParser("<log4net:event ></log4net:event> After Stuff"));
    }

    [TestMethod]
    public void CanCreateParserReturnsTrueWhenMessageMatchesLog4NetXmlLayoutElement()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsTrue(factory.CanCreateParser("<log4net:event ></log4net:event>"));
    }

    [TestMethod]
    public void CreateReturnsXmlLayoutParser()
    {
      var factory = new XmlLayoutParserFactory();

      Assert.IsInstanceOfType(factory.Create("<log4net:event ></log4net:event>"), typeof(XmlLayoutParser));
    }
  }
}
