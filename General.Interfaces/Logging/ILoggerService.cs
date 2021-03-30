using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces.Logging
{
    public interface ILoggerService
    {
        void Informatiom(string info);

        void Debug(string debugInfo);

        void Error(string errorInfo);
    }
}
