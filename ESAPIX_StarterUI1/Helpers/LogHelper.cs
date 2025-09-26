using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESAPIX_WPF_Example.Helpers
{
    public static class LogHelper
    {
        public static Microsoft.Extensions.Logging.ILogger GetLogger(bool toConsole = true, bool toFile = true)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug();

            if (toConsole)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console();
            }

            if (toFile)
            {
                var logPath = Path.Combine(AppContext.BaseDirectory, "esapix_log.txt");
                loggerConfiguration = loggerConfiguration.WriteTo.File(
                    logPath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            }

            if (!toConsole && !toFile)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console();
            }

            var serilogLogger = loggerConfiguration.CreateLogger();
            Log.Logger = serilogLogger;

            var loggerFactory = new SerilogLoggerFactory(serilogLogger);
            return loggerFactory.CreateLogger(nameof(LogHelper));
        }
    }
}
