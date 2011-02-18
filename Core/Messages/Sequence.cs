using System;
using Harvester.Core.Logging;

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

namespace Harvester.Core.Messages
{
  public class Sequence
  {
    private static readonly ILog Log = LogManager.CreateClassLogger();
    private readonly Object _syncLock = new Object();
    private UInt32 _nextId;

    public Sequence()
      : this(1)
    { }

    public Sequence(UInt32 nextId)
    {
      Log.DebugFormat("Initializing sequence: {0}", nextId);

      _nextId = nextId;
    }

    public UInt32 GetNextId()
    {
      lock (_syncLock)
      {
        unchecked
        {
          return _nextId++;
        }
      }
    }
  }
}
