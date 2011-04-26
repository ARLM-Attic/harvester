using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

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

namespace Harvester.Core.Win32.Basic
{
  public sealed class BasicApi : IBasicApi
  {
    public const Int32 ErrorAlreadyExists = 183;
    public const UInt32 SectionMapRead = 0x0004;
    public const UInt32 Infinite = 0xFFFFFFFF;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern Boolean CloseHandle(IntPtr hHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateEvent(ref SecurityAttributes sa, Boolean bManualReset, Boolean bInitialState, String lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateFileMapping(IntPtr hFile, ref SecurityAttributes lpFileMappingAttributes, PageProtection flProtect, UInt32 dwMaximumSizeHigh, UInt32 dwMaximumSizeLow, String lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, UInt32 dwDesiredAccess, UInt32 dwFileOffsetHigh, UInt32 dwFileOffsetLow, UInt32 dwNumberOfBytesToMap);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern Boolean PulseEvent(IntPtr hEvent);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern Boolean SetEvent(IntPtr hEvent); 

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern Boolean UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern Int32 WaitForSingleObject(IntPtr handle, UInt32 milliseconds);

    public Boolean CloseHandle(Handle handle)
    {
      return CloseHandle((IntPtr)handle);
    }

    public Handle CreateLocalEvent(ref SecurityAttributes securityAttributes, String objectName)
    {
      Verify.NotWhitespace(objectName);
      return CreateEvent(ref securityAttributes, "Local", objectName);
    }

    public Handle CreateGlobalEvent(ref SecurityAttributes securityAttributes, String objectName)
    {
      Verify.NotWhitespace(objectName);
      return CreateEvent(ref securityAttributes, "Global", objectName);
    }

    private static Handle CreateEvent(ref SecurityAttributes securityAttributes, String objectNamePrefix, String objectName)
    {
      IntPtr handle = CreateEvent(ref securityAttributes, false, false, String.Format(@"{0}\{1}", objectNamePrefix, objectName));

      if(Marshal.GetLastWin32Error() == ErrorAlreadyExists)
        throw new Win32Exception(ErrorAlreadyExists);

      if (handle == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error());

      return new Handle(handle, CloseHandle);
    }

    public Handle CreateLocalFileMapping(ref SecurityAttributes securityAttributes, String objectName)
    {
      Verify.NotWhitespace(objectName);
      return CreateFileMapping(ref securityAttributes, "Local", objectName);
    }

    public Handle CreateGlobalFileMapping(ref SecurityAttributes securityAttributes, String objectName)
    {
      Verify.NotWhitespace(objectName);
      return CreateFileMapping(ref securityAttributes, "Global", objectName);
    }

    private static Handle CreateFileMapping(ref SecurityAttributes securityAttributes, String objectNamePrefix, String objectName)
    {
      IntPtr handle = CreateFileMapping(new IntPtr(-1), ref securityAttributes, PageProtection.ReadWrite, 0, 4096, String.Format(@"{0}\{1}", objectNamePrefix, objectName));

      if (handle == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error());

      return new Handle(handle, CloseHandle);
    }

    public Handle MapReadOnlyViewOfFile(Handle fileHandle)
    {
      Verify.NotNull(fileHandle);

      IntPtr handle = MapViewOfFile(fileHandle, SectionMapRead, 0, 0, 0);

      if (handle == IntPtr.Zero)
        throw new Win32Exception(Marshal.GetLastWin32Error());

      return new Handle(handle, UnmapViewOfFile);
    }

    public Boolean PulseEvent(Handle eventHandle)
    {
      Verify.NotNull(eventHandle);
      return PulseEvent((IntPtr)eventHandle);
    }

    public Boolean SetEvent(Handle eventHandle)
    {
      Verify.NotNull(eventHandle);
      return SetEvent((IntPtr)eventHandle);
    }

    public Int32 WaitForSingleObject(Handle eventHandle)
    {
      Verify.NotNull(eventHandle);
      return WaitForSingleObject(eventHandle, Infinite);
    }
  }
}
