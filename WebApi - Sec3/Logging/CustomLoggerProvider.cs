using System.Collections.Concurrent;

namespace WebApi___Sec3.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
    readonly CustomLoggerProviderConfig   _loggerconfig;
    readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfig config)
        {
            _loggerconfig = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggerconfig));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
