using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace Harvester.Core.Tests
{
  public class VerifyExceptionTests
  {
    [Fact]
    public void CtorUsesExpectedDefaultMessage()
    {
      Assert.Equal("Value is invalid.", new VerifyException().Message);
    }

    [Fact]
    public void CtorUsesCustomMessageWhenProvided()
    {
      Assert.Equal(Localization.VerifyNotNullException, new VerifyException(Localization.VerifyNotNullException).Message);
    }

    [Fact]
    public void CtorUsesCustomMessageWithInnerExceptionWhenProvided()
    {
      var innerEx = new Exception();
      var ex = new VerifyException(Localization.VerifyNotNullException, innerEx);

      Assert.Equal(Localization.VerifyNotNullException, ex.Message);
      Assert.Equal(innerEx, ex.InnerException);
    }

    [Fact]
    public void CanSerializeAndDeserializeException()
    {
      var exToSerializeEx = new VerifyException(Localization.VerifyNotNullException);
      VerifyException deserializedEx;

      var formatter = new BinaryFormatter();

      using (var memoryStream = new MemoryStream())
      {
        formatter.Serialize(memoryStream, exToSerializeEx);

        memoryStream.Seek(0, SeekOrigin.Begin);

        deserializedEx = (VerifyException)formatter.Deserialize(memoryStream);
      }

      Assert.Equal(exToSerializeEx.Message, deserializedEx.Message);
    }
  }
}
