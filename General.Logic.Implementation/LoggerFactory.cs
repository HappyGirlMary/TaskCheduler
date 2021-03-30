using System;
using Core.Interfaces.Logging;
using Core.Logic.Implementations.Logging;

namespace Core.Logic.Implementation
{
    public class LoggerFactory
    {
        public static ILoggerService CreateLogger() => new LoggerService();
    }
}
