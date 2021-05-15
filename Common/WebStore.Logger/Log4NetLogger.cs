﻿using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;
        public Log4NetLogger(string Category, XmlElement Configuration)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            _Log = LogManager.GetLogger(logger_repository.Name, Category);

            log4net.Config.XmlConfigurator.Configure(logger_repository, Configuration);
        }
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel LogLevel) => LogLevel switch
        {
            LogLevel.None => false,
            LogLevel.Trace => _Log.IsDebugEnabled,
            LogLevel.Debug => _Log.IsDebugEnabled,
            LogLevel.Information => _Log.IsInfoEnabled,
            LogLevel.Warning => _Log.IsWarnEnabled,
            LogLevel.Error => _Log.IsErrorEnabled,
            LogLevel.Critical => _Log.IsFatalEnabled,
            _ => throw new ArgumentOutOfRangeException(nameof(LogLevel),LogLevel, null)
        };

        public void Log<TState>(LogLevel Level, EventId Id, TState State, Exception Error, Func<TState, Exception, string> formatter)
        {
            if (formatter is null)
                throw new ArgumentOutOfRangeException(nameof(formatter));

            var log_message = formatter(State, Error);

            if (string.IsNullOrEmpty(log_message) && Error is null) return;

            switch (Level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(log_message);
                    break;
                case LogLevel.Information:
                    _Log.Info(log_message);
                    break;
                case LogLevel.Warning:
                    _Log.Warn(log_message);
                    break;
                case LogLevel.Error:
                    _Log.Error(log_message,Error);
                    break;
                case LogLevel.Critical:
                    _Log.Fatal(log_message,Error);
                    break;
                case LogLevel.None:
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(Level), Level, null);
            } 
        }
    }
}
