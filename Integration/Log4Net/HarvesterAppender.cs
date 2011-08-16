using Harvester.Core;
using Harvester.Core.Tracing;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;

namespace Harvester.Integration.Log4Net
{
  public sealed class HarvesterAppender : AppenderSkeleton
  {
    private readonly OutputDebugStringWriter _writer;
    private readonly SharedMemoryBuffer _buffer;

    public HarvesterAppender()
    {
      Layout = new XmlLayoutSchemaLog4j();

      _buffer = new SharedMemoryBuffer("Local\\HRVST_DBWIN", 4096);
      _writer = new OutputDebugStringWriter("HarvestDBWinMutex", _buffer);
    }

    protected override void OnClose()
    {
      base.OnClose();

      _buffer.Dispose();
    }

    protected override void Append(LoggingEvent loggingEvent)
    {
      _writer.Write(RenderLoggingEvent(loggingEvent));
    }
  }
}
