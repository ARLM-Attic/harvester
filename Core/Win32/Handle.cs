using System;

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

namespace Harvester.Core.Win32
{
  public sealed class Handle : IDisposable
  {
    private readonly IntPtr _handle;
    private volatile Boolean _disposed;
    private readonly Object _syncLock = new Object();
    private readonly Func<IntPtr, Boolean> _disposeAction;

    public static readonly Handle Null = new Handle(IntPtr.Zero);

    public Handle(IntPtr handle)
      : this(handle, ptr => true, true)
    { }

    public Handle(IntPtr handle, Func<IntPtr, Boolean> disposeAction)
      : this(handle, disposeAction, false)
    { }

    private Handle(IntPtr handle, Func<IntPtr, Boolean> disposeAction, Boolean disposed)
    {
      Verify.NotNull(disposeAction);

      _handle = handle;
      _disposed = disposed;
      _disposeAction = disposeAction;
    }

    public void Dispose()
    {
      lock (_syncLock)
      {
        if (_disposed)
          return;

        _disposed = true;
      }

      if (_handle != IntPtr.Zero)
        _disposeAction.Invoke(_handle);

      GC.SuppressFinalize(this);
    }

    public static implicit operator IntPtr(Handle handle)
    {
      Verify.NotNull(handle);

      return handle._handle;
    }

    public static Handle operator +(Handle lhs, Int32 rhs)
    {
      Verify.NotNull(lhs);

      return new Handle(new IntPtr(lhs._handle.ToInt32() + rhs));
    }
  }
}
