using System;
using Harvester.Core;
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

namespace Core.Tests
{
  [TestClass]
  public class VerifyTests
  {
    [TestMethod]
    [ExpectedException(typeof(VerifyException), "Value cannot be null.")]
    public void NotNullThrowsOnNullValue()
    {
      Verify.NotNull((Object)null);
    }

    [TestMethod]
    public void NotNullIgnoresNonNulLValue()
    {
      Verify.NotNull(new Object());
    }

    [TestMethod]
    [ExpectedException(typeof(VerifyException), "Value cannot be a null, empty or whitespace only.")]
    public void NotWhitespaceThrowsOnNullValue()
    {
      Verify.NotWhitespace(null);
    }

    [TestMethod]
    [ExpectedException(typeof(VerifyException), "Value cannot be a null, empty or whitespace only.")]
    public void NotWhitespaceThrowsOnEmptValue()
    {
      Verify.NotWhitespace(String.Empty);
    }

    [TestMethod]
    [ExpectedException(typeof(VerifyException), "Value cannot be a null, empty or whitespace only.")]
    public void NotWhitespaceThrowsOnWhitespaceValue()
    {
      Verify.NotWhitespace(Environment.NewLine);
    }

    [TestMethod]
    public void NotWhitespaceIgnoresNonNulLEmptyOrWhitespaceValue()
    {
      Verify.NotWhitespace("Not Null");
    }

    [TestMethod]
    [ExpectedException(typeof(VerifyException), "Vallue must be greater than zero.")]
    public void GreaterThanZeroThrowsOnZero()
    {
      Verify.GreaterThanZero(0);
    }

    [TestMethod]
    public void GreaterThanZeroIgnoresPositivevalue()
    {
      Verify.GreaterThanZero(0.001);
    }
  }
}
