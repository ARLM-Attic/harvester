using Harvester.Core;
using Harvester.Core.Tracing;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace Harvester.Integration.NLog
{
  [Target("Harvester")] 
  public sealed class HarvesterTarget : TargetWithLayout
  {
    private readonly OutputDebugStringWriter _writer;
    private readonly SharedMemoryBuffer _buffer;

    public HarvesterTarget()
    {
      Layout = new Log4JXmlEventLayout();

      _buffer = new SharedMemoryBuffer("Local\\HRVST_DBWIN", 4096);
      _writer = new OutputDebugStringWriter("HarvestDBWinMutex", _buffer);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);

      if (disposing)
        _buffer.Dispose();
    }

    protected override void Write(LogEventInfo logEvent)
    {
      _writer.Write(Layout.Render(logEvent));
    } 
  }
}

