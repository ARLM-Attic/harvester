using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NLog;

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
  public sealed class OutputDebugStringWriter
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
    private readonly String _mutexName;
    private readonly Int32 _processId;
    private readonly IBuffer _buffer;
    
    public OutputDebugStringWriter(String mutexName, IBuffer buffer)
    {
      Verify.NotWhitespace(mutexName);
      Verify.NotNull(buffer);

      using (var process = Process.GetCurrentProcess())
        _processId = process.Id;

      _mutexName = mutexName;
      _buffer = buffer;
    }

    public Boolean Write(String message)
    {
      Boolean createdNew;

      Log.Debug("Creating mutex: {0}", _mutexName);

      using (var mutex = new Mutex(false, _mutexName, out createdNew))
      {
        Log.Debug("Mutex created as new: {0}", createdNew);

        if (createdNew || !mutex.WaitOne(Timeout))
          return false;

        try
        {
          Log.Debug("Writing OutputDebugString message.");

          var position = 0;
          var maxBlockSize = OutputDebugString.GetMaxCharCount(_buffer.Capacity);

          Verify.GreaterThanZero(maxBlockSize);

          while (position < message.Length)
          {
            var length = Math.Min(maxBlockSize, message.Length - position);

            _buffer.Write(new OutputDebugString(_processId, message.Substring(position, length)));

            position += length;
          }
        }
        finally
        {
          Log.Debug("Releasing mutex.");

          mutex.ReleaseMutex();
        }

        return true;
      }
    }
  }
}
