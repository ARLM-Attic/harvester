﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Harvester.Core.Messaging.Sources;
using Harvester.Core.Messaging.Sources.NamedPipe;
using Moq;
using Xunit;
using Xunit.Extensions;

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

namespace Harvester.Core.Tests.Messaging.Sources.NamedPipe.UsingPipeMessageWriter
{
    public class WhenWritingMessage : IDisposable
    {
        private readonly Mock<MessageBuffer> messageBuffer = new Mock<MessageBuffer>("BufferName");
        private readonly ManualResetEvent setupComplete = new ManualResetEvent(false);
        private readonly ManualResetEvent testComplete = new ManualResetEvent(false);
        private readonly PipeMessageWriter messageWriter;
        private readonly String mutexName;

        public WhenWritingMessage()
        {
            mutexName = "Harvester: " + Guid.NewGuid();
            messageWriter = new PipeMessageWriter(mutexName, messageBuffer.Object);
            messageBuffer.Object.Timeout = TimeSpan.FromMilliseconds(25);
        }

        public void Dispose()
        {
            setupComplete.Dispose();
            testComplete.Dispose();
        }

        [Fact]
        public void DoNotWriteMessageIfMutexCreated()
        {
            messageWriter.Write("My Message");

            messageBuffer.Verify(mock => mock.Write(It.IsAny<Byte[]>()), Times.Never());
        }

        [Fact]
        public void DoNotWriteMessageIfMutexWaitTimeout()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          using (new Mutex(true, mutexName))
                                          {
                                              setupComplete.Set();
                                              testComplete.WaitOne();
                                          }
                                      });

            setupComplete.WaitOne();

            messageWriter.Write("My Message");

            messageBuffer.Verify(mock => mock.Write(It.IsAny<Byte[]>()), Times.Never());

            testComplete.Set();
        }

        [Fact]
        public void AlwaysWriteMessageWithProcessIdPreamble()
        {
            using (new Mutex(false, mutexName))
            using (var process = Process.GetCurrentProcess())
            {
                var processId = process.Id;

                messageWriter.Write("My First Message");
                messageWriter.Write("My Second Message");

                messageBuffer.Verify(mock => mock.Write(It.Is<Byte[]>(buffer => BitConverter.ToInt32(buffer, 0) == processId)), Times.Exactly(2));
            }
        }

        [Theory, InlineData(null), InlineData(""), InlineData("Small Message")]
        public void DoNotWriteOutNullTerminatingByte(String message)
        {
            using (new Mutex(false, mutexName))
            {
                var chunks = new Queue<String>();

                messageBuffer.Setup(mock => mock.Write(It.IsAny<Byte[]>())).Callback((Byte[] buffer) => chunks.Enqueue(new PipeMessage("Source", buffer).Message));

                messageWriter.Write(message);

                Assert.Equal(1, chunks.Count);
                Assert.False(chunks.Dequeue().Any(c => c == 0));
            }
        }

        [Fact]
        public void WriteLargeMessagesAsSingleChunk()
        {
            using (new Mutex(false, mutexName))
            {
                var message = String.Empty.PadLeft(1048576);

                messageWriter.Write(message);

                messageBuffer.Verify(mock => mock.Write(It.IsAny<Byte[]>()), Times.Once());
            }
        }
    }
}
