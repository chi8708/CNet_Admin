﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNet.Common
{
     public class LogHelper
    {
        private ILog logger;

        public LogHelper(ILog log)
        {
            this.logger = log;
        }

        public void Info(object message)
        {
            this.logger.Info(message);
        }

        public void Info(object message, Exception e)
        {
            this.logger.Info(message, e);
        }

        public void Debug(object message)
        {
            this.logger.Debug(message);
        }

        public void Debug(object message, Exception e)
        {
            this.logger.Debug(message, e);
        }

        public void Warning(object message)
        {
            this.logger.Warn(message);
        }

        public void Warning(object message, Exception e)
        {
            this.logger.Warn(message, e);
        }

        public void Error(object message)
        {
            this.logger.Error(message);
        }

        public void Error(object message, Exception e)
        {
            this.logger.Error(message, e);
        }

        public void Fatal(object message)
        {
            this.logger.Fatal(message);
        }

        public void Fatal(object message, Exception e)
        {
            this.logger.Fatal(message, e);
        }

        /// <summary>
        /// Request日志写入
        /// </summary>
        public static void WrtieRequestLog(LogLevel logLevel, string message)
        {
            Task.Run(() =>
            {
                LogHelper m_Log = LogFactory.GetLogger(LogType.RequestLog);
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        m_Log.Debug(message);
                        break;
                    case LogLevel.Error:
                        m_Log.Error(message);
                        break;
                    case LogLevel.Info:
                        m_Log.Info(message);
                        break;
                    case LogLevel.Warning:
                        m_Log.Warning(message);
                        break;
                }

            });

        }
    }

     /// <summary>
     /// 日志类型
     /// </summary>
     public enum LogType
     {
         GlobalLog,
         RequestLog
     }

     /// <summary>
     /// 日志等级
     /// </summary>
     public enum LogLevel
     {
         Error,
         Debug,
         Warning,
         Info
     }

    /// <summary>
    /// 日志帮助类工厂
    /// </summary>
      public class LogFactory
      {
        private static readonly string repositoryName = LogType.GlobalLog.ToString();
        private static LogHelper logHelperSingleton;
        static LogFactory()
        {
            logHelperSingleton= GetLogger(repositoryName);
        }
        public static LogHelper GetLoggerSingleton()
        {
            logHelperSingleton =logHelperSingleton?? GetLogger(repositoryName);
            return logHelperSingleton;
        }

        public static LogHelper GetLogger(Type type)
        {
            return new LogHelper(LogManager.GetLogger(type));
        }

        public static LogHelper GetLogger(string str)
        {
            return new LogHelper(LogManager.GetLogger(str));
        }

        public static LogHelper GetLogger(LogType logType)
        {
            return new LogHelper(LogManager.GetLogger(logType.ToString(), logType.ToString()));
        }
    }
}
