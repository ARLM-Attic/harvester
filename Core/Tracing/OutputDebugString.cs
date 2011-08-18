using System;
using System.Text;

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

namespace Harvester.Core.Tracing
{
  public class OutputDebugString
  {
    private readonly Int32 _processId;
    private readonly String _message;
    private readonly Byte[] _rawData;

    public Int32 ProcessId { get { return _processId; } }
    public String Message { get { return _message; } }
    private Byte[] Raw { get { return _rawData; } }

    public OutputDebugString(Byte[] rawData)
    {
      _rawData = rawData ?? new Byte[0];
      _processId = GetProcessId(_rawData);
      _message = GetMessage(_rawData);
    }

    public OutputDebugString(Int32 processId, String message)
    {
      _processId = processId;
      _message = message ?? String.Empty;
      _rawData = GetRawData(_processId, _message);
    }

    private static Int32 GetProcessId(Byte[] rawData)
    {
      return rawData.Length < sizeof(Int32) ? 0 : BitConverter.ToInt32(rawData, 0);
    }

    private static String GetMessage(Byte[] buffer)
    {
      Int32 index = sizeof(Int32);

      while (index < buffer.Length && buffer[index] != 0)
        index++;

      index--;

      return index >= sizeof(Int32) ? Encoding.ASCII.GetString(buffer, sizeof(Int32), index - sizeof(Int32) + 1) : String.Empty;
    }

    private static Byte[] GetRawData(Int32 processId, String message)
    {
      var messageBytes = Encoding.ASCII.GetBytes(message);
      var processIdBytes = BitConverter.GetBytes(processId);
      var result = new Byte[processIdBytes.Length + messageBytes.Length + 1];

      Buffer.BlockCopy(processIdBytes, 0, result, 0, processIdBytes.Length);

      if (messageBytes.Length > 0)
        Buffer.BlockCopy(messageBytes, 0, result, processIdBytes.Length, messageBytes.Length);

      return result;
    }

    public static Int32 GetMaxCharCount(Int32 byteCount)
    {
      Verify.GreaterThanZero(byteCount);

      return Math.Max(0, byteCount - sizeof(Int32) - 1);
    }

    public static implicit operator Byte[](OutputDebugString outputDebugString)
    {
      return outputDebugString == null ? new Byte[0] : outputDebugString.Raw;
    }

    public static OutputDebugString operator +(OutputDebugString lhs, OutputDebugString rhs)
    {
      if (lhs == null)
        return rhs;

      if (rhs == null)
        return lhs;

      if(lhs.ProcessId != rhs.ProcessId)
        throw new InvalidOperationException(Localization.OutputDebugStringProcessMismatch);

      return new OutputDebugString(lhs.ProcessId, lhs.Message + rhs.Message);
    }
  }
}
