using System;
using System.Diagnostics;
using System.Threading;

namespace Harvester.Core.Tracing
{
  public class OutputDebugStringWriter
  {
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

      using (var mutex = new Mutex(false, _mutexName, out createdNew))
      {
        if (createdNew || !mutex.WaitOne(Timeout))
          return false;

        try
        {
          //TODO: Split messages over 2046 bytes.
          _buffer.Write(new OutputDebugString(_processId, message));
        }
        finally
        {
          mutex.ReleaseMutex();
        }

        return true;
      }
    }
  }
}
