using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harvester.Core.Messages.Sources.DbWin;
using Moq;
using Xunit;
using Harvester.Core.Win32;
using Harvester.Core.Win32.Basic;
using Harvester.Core.Win32.Advanced;

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

namespace Harvester.Core.Tests.Messages.Sources.DbWin
{
  public class DbWinMessageProducerTests
  {
    private readonly Mock<IEnqueuer<DbWinMessage>> _enqueuer = new Mock<IEnqueuer<DbWinMessage>>(MockBehavior.Strict);
    private readonly Mock<IAdvancedApi> _windowsAdvancedApi = new Mock<IAdvancedApi>(MockBehavior.Strict);
    private readonly Mock<IBasicApi> _windowsBasicApi = new Mock<IBasicApi>(MockBehavior.Strict);
    private readonly Mock<IWindowsApi> _windowsApi = new Mock<IWindowsApi>(MockBehavior.Strict);
    private readonly DbWinMessageProducer _messageProducer;

    public DbWinMessageProducerTests()
    {
      var securityAttributes = new SecurityAttributes();

      _windowsApi.SetupGet(mock => mock.Advanced).Returns(_windowsAdvancedApi.Object);
      _windowsApi.SetupGet(mock => mock.Basic).Returns(_windowsBasicApi.Object);

      _windowsAdvancedApi.Setup(mock => mock.InitializeSecurityDescriptor()).Returns(new SecurityDescriptor());

      _windowsBasicApi.Setup(mock => mock.CreateLocalEvent(ref securityAttributes, "DBWIN_BUFFER_READY")).Returns(new Handle(IntPtr.Zero));
      _windowsBasicApi.Setup(mock => mock.CreateLocalEvent(ref securityAttributes, "DBWIN_DATA_READY")).Returns(new Handle(IntPtr.Zero));
      _windowsBasicApi.Setup(mock => mock.CreateLocalFileMapping(ref securityAttributes, "DBWIN_BUFFER")).Returns(new Handle(IntPtr.Zero));
      _windowsBasicApi.Setup(mock => mock.MapReadOnlyViewOfFile(It.IsAny<Handle>())).Returns(new Handle(IntPtr.Zero));

      _messageProducer = new DbWinMessageProducer(_enqueuer.Object, _windowsApi.Object);
    }

    [Fact]
    public void StartShouldThrowObjectDisposedExWhenDisposed()
    {
      _messageProducer.Dispose();

      Assert.Throws<ObjectDisposedException>(() => _messageProducer.Start());
    }

    [Fact]
    public void StartShouldThrowInvalidOperationExWhenAlreadyStarted()
    {
      // Ensure Thread doesn't win race to second .Start() call.
      _windowsBasicApi.Setup(mock => mock.SetEvent(It.IsAny<Handle>())).Returns(true);
      _windowsBasicApi.Setup(mock => mock.WaitForSingleObject(It.IsAny<Handle>())).Returns(1);

      _messageProducer.Start();

      var ex = Assert.Throws<InvalidOperationException>(() => _messageProducer.Start());
      Assert.Equal("DbWinMessageProducer has already been started; unable to start DbWinMessageProducer.", ex.Message);
    }

    [Fact]
    public void StopCallIgnoredWhenNotStarted()
    {
      Assert.DoesNotThrow(_messageProducer.Stop);
    }

    [Fact]
    public void StopCallIgnoredWhenDisposed()
    {
      _messageProducer.Dispose();

      Assert.DoesNotThrow(_messageProducer.Stop);
    }

    [Fact]
    public void StopCallIgnoredWhenAlreadyStopped()
    {
      _messageProducer.Stop();
      Assert.DoesNotThrow(_messageProducer.Stop);
    }

    [Fact]
    public void DisposeIgnoresMultipleCalls()
    {
      _messageProducer.Dispose();
      _messageProducer.Dispose();
    }
  }
}
