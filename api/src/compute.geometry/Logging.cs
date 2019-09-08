using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Json;

namespace Define.Api
{
    static class Logging
    {
        static bool _enabled = false;
        public static void Init()
        {
            if (_enabled)
                return;

            var path = Path.Combine(Path.GetTempPath(), "Compute", "Logs", "log-geometry-.txt"); // log-20180925.txt, etc.
            var limit = 10;

            var logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#endif
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Source", "geometry")
                .WriteTo.Console(outputTemplate: "{Timestamp:o} {Level:w3}: {Source} {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.File(new JsonFormatter(), path, rollingInterval: RollingInterval.Day, retainedFileCountLimit: limit);

            Log.Logger = logger.CreateLogger();

            Log.Debug("Logging to {LogPath}", Path.GetDirectoryName(path));

            _enabled = true;
        }
    }
}
