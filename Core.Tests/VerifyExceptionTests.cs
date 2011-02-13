using Harvester.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
  public class VerifyExceptionTests
  {
    [TestMethod]
    public void CtorUsesExpectedDefaultMessage()
    {
      Assert.AreEqual("Value is invalid.", new VerifyException().Message);
    }

    [TestMethod]
    public void CtorUsesCustomMessageWhenProvided()
    {
      Assert.AreEqual(Localization.VerifyNotNullException, new VerifyException(Localization.VerifyNotNullException).Message);
    }

    [TestMethod]
    public void CtorUsesCustomMessageWithInnerExceptionWhenProvided()
    {
      var innerEx = new Exception();
      var ex = new VerifyException(Localization.VerifyNotNullException, innerEx);

      Assert.AreEqual(Localization.VerifyNotNullException, ex.Message);
      Assert.AreEqual(innerEx, ex.InnerException);
    }

    [TestMethod]
    public void CanSerializeAndDeserializeException()
    {
      var exToSerializeEx = new VerifyException(Localization.VerifyNotNullException);
      var deserializedEx = (VerifyException) null;

      var formatter = new BinaryFormatter();

      using (var memoryStream = new MemoryStream())
      {
        formatter.Serialize(memoryStream, exToSerializeEx);

        memoryStream.Seek(0, SeekOrigin.Begin);

        deserializedEx = (VerifyException)formatter.Deserialize(memoryStream);
      }

      Assert.AreEqual(exToSerializeEx.Message, deserializedEx.Message);
    }
  }
}
