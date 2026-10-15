using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryLogger.Logger
{
    public class TelemetryFileLogger : IDisposable
    {
        private string _logDirPath;

        private StreamWriter _writer;

        private string _logFilePrefix;

        private string[] _logFileHeaders;

        private readonly Stopwatch _flushTimer = Stopwatch.StartNew();

        private readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(2);

        private long _currentChunkSize;

        private const long MAX_CHUNK_SIZE = 15 * 1024; // 15 KB

        public TelemetryFileLogger(string logFilePrefix, string[] logFileHeaders)
        {
            _logFileHeaders = logFileHeaders;
            _logFilePrefix = logFilePrefix;
        }

        public void StartLogging(string logFolderPath)
        {
            _logDirPath = logFolderPath;
            StartLoggingNewChunk();
        }

        public void Write(BaseLog log)
        {
            var row = log.ToCsvRow() + Environment.NewLine;

            var rowSize = Encoding.UTF8.GetByteCount(row);

            if(rowSize + _currentChunkSize > MAX_CHUNK_SIZE)
            {
                Flush();
                StartLoggingNewChunk();
            }

            _writer.Write(row);
            _currentChunkSize += rowSize;
        }

        public void DelayedFlush()
        {

            if (_flushTimer.Elapsed >= _flushInterval)
            {
                Flush();
            }
        }

        public void Flush()
        {
            if (_writer == null)
                return;

            _writer.Flush();
            _flushTimer.Restart();
        }
        public void Dispose()
        {
            if (_writer == null)
                return;

            _writer.Flush();
            _writer.Dispose();
        }

        private void StartLoggingNewChunk()
        {
            _currentChunkSize = 0;
            var filename = _logFilePrefix + DateTime.UtcNow.ToString("yyyy-MM-ddTHH_mm_ss") + "_log.txt";

            _writer = new StreamWriter(
                _logDirPath + "\\" + filename,
                append: false,
                encoding: new UTF8Encoding(true)
            );

            var headers = string.Join(",", _logFileHeaders) + Environment.NewLine;
            _currentChunkSize += Encoding.UTF8.GetByteCount(headers);
            _writer.Write(headers);
            _writer.Flush();
        }
    }
}
