using System;
using Harvester.Core.Messages;
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

namespace Harvester.Core.Tests.Messages
{
  [TestClass]
  public class SequenceTests
  {
    [TestMethod]
    public void GetNextIdShouldReturnInitialIdOnFirstCall()
    {
      var sequence = new Sequence();

      Assert.AreEqual(1U, sequence.GetNextId());
    }

    [TestMethod]
    public void GetNextIdShouldIncrementByOnEachCall()
    {
      var sequence = new Sequence(1);

      Assert.AreEqual(1U, sequence.GetNextId());
      Assert.AreEqual(2U, sequence.GetNextId());
      Assert.AreEqual(3U, sequence.GetNextId());
    }

    [TestMethod]
    public void GetNextIdShouldRollOverOnMaxValue()
    {
      var sequence = new Sequence(UInt32.MaxValue);

      Assert.AreEqual(UInt32.MaxValue, sequence.GetNextId());
      Assert.AreEqual(0U, sequence.GetNextId());
    }
  }
}
