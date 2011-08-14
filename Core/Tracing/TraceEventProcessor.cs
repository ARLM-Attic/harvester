using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Harvester.Core.Messages;

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
  internal class TraceEventProcessor : ITraceEventProcessor
  {
    private readonly IBlockingQueue<TraceEvent> _traceEvents;
    private readonly ILogMessageFactory _logMessageFactory;
    private readonly Thread _processingThread;

    public event EventHandler<TraceEventsProcessedEventArgs> TraceEventsProcessed;

    public TraceEventProcessor()
     : this(new BlockingQueue<TraceEvent>(), new LogMessageFactory())
    { }

    internal TraceEventProcessor(IBlockingQueue<TraceEvent> traceEvents, ILogMessageFactory logMessageFactory)
    {
      Verify.NotNull(traceEvents);
      Verify.NotNull(logMessageFactory);

      _traceEvents = traceEvents;
      _logMessageFactory = logMessageFactory;
      _processingThread = new Thread(ProcessEvents) { Name = "TraceEvent Processor", IsBackground = true };
      _processingThread.Start();
    }

    public void Dispose()
    {
      _traceEvents.Dispose();

      if (_processingThread.IsAlive)
        _processingThread.Join();
    }

    public void ProcessEvent(TraceEvent traceEvent)
    {
      _traceEvents.Enqueue(traceEvent);
    }

    private void ProcessEvents()
    {
      IList<TraceEvent> events;

      while(_traceEvents.TryDequeueAll(out events))
      {
        var defensiveCopy = TraceEventsProcessed;
        if(defensiveCopy != null)
          defensiveCopy.Invoke(this, CreateEventArgs(events));
      }
    }

    private TraceEventsProcessedEventArgs CreateEventArgs(IEnumerable<TraceEvent> events)
    {
      return new TraceEventsProcessedEventArgs(events.Select(_logMessageFactory.Create));
    }
  }
}
