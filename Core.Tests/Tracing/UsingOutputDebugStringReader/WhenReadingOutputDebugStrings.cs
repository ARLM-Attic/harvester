using System;
using System.Linq;
using Harvester.Core.Extensions;
using Harvester.Core.Tracing;
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

namespace Harvester.Core.Tests.Tracing.UsingOutputDebugStringReader
{
  public class WhenReadingOutputDebugStrings
  {
    private readonly OutputDebugStringReader _reader = new OutputDebugStringReader(64);
    private readonly Random _randomizer = new Random();
   
    [Fact]
    public void NoMessageReadIfOutputDebugStringMessageNull()
    {
      var outputDebugStrings = _reader.GetOutputDebugStrings(null);

      Assert.True(!outputDebugStrings.Any());
    }

    [Fact]
    public void NoMessageReadIfOutputDebugStringMessageEmptyOrWhitespace()
    {
      var outputDebugStrings = _reader.GetOutputDebugStrings(new Byte[0]);

      Assert.True(!outputDebugStrings.Any());
    }

    [Fact]
    public void MessageReadIfOutputDebugStringIsNotFragment()
    {
      var message = _randomizer.NextString(32);

      Assert.Equal(message, _reader.GetOutputDebugStrings(new OutputDebugString(1, message)).Single().Message);
    }

    [Fact]
    public void MessageIsNotBufferedIfOutputDebugStringIsNotFragment()
    {
      var message1 = _randomizer.NextString(32);
      var message2 = _randomizer.NextString(32);

      Assert.Equal(message1, _reader.GetOutputDebugStrings(new OutputDebugString(1, message1)).Single().Message);
      Assert.Equal(message2, _reader.GetOutputDebugStrings(new OutputDebugString(1, message2)).Single().Message);
    }
  }
}
