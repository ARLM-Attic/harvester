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
  public class WhenReadingFragmentedOutputDebugStrings
  {
    private readonly OutputDebugStringReader _reader = new OutputDebugStringReader(64);
    private readonly Random _randomizer = new Random();
    private readonly String _initialFragment;
    private const Int32 ProcessId = 1;

    public WhenReadingFragmentedOutputDebugStrings()
    {
      _initialFragment = _randomizer.NextString(64);

      Assert.False(_reader.GetOutputDebugStrings(new OutputDebugString(ProcessId, _initialFragment)).Any());
    }

    [Fact]
    public void FlushFragmentBeforeNewMessageIfNewMessageHasDifferentProcessId()
    {
      var message = _randomizer.NextString(32);
      var outputDebugStrings = _reader.GetOutputDebugStrings(new OutputDebugString(ProcessId + 1, message)).ToList();

      Assert.Equal(2, outputDebugStrings.Count);
      Assert.Equal(_initialFragment, outputDebugStrings[0].Message);
      Assert.Equal(message, outputDebugStrings[1].Message);
    }

    [Fact]
    public void FlushFragmentOnlyIfNewMessageHasDifferentProcessIdButIsFragment()
    {
      var message = _randomizer.NextString(64);
      var outputDebugStrings = _reader.GetOutputDebugStrings(new OutputDebugString(ProcessId + 1, message)).ToList();

      Assert.Equal(1, outputDebugStrings.Count);
      Assert.Equal(_initialFragment, outputDebugStrings[0].Message);
    }

    [Fact]
    public void FlushFragmentIfExceedsMaxMessageLength()
    {
      var message1 = _randomizer.NextString(896);
      var message2 = _randomizer.NextString(64);

      Assert.False(_reader.GetOutputDebugStrings(new OutputDebugString(ProcessId, message1)).Any());

      var outputDebugStrings = _reader.GetOutputDebugStrings(new OutputDebugString(ProcessId, message2)).ToList();

      Assert.Equal(1, outputDebugStrings.Count);
      Assert.Equal(_initialFragment + message1 + message2, outputDebugStrings[0].Message);
    }

    [Fact]
    public void FlushFragmentIfNewMessageLastFragment()
    {
      var lastFragment = _randomizer.NextString(32);
      var outputDebugStrings = _reader.GetOutputDebugStrings(new OutputDebugString(ProcessId, lastFragment)).ToList();

      Assert.Equal(1, outputDebugStrings.Count);
      Assert.Equal(_initialFragment + lastFragment, outputDebugStrings[0].Message);
    }
  }
}
