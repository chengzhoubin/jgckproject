using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;

namespace JGCK
{
    public static class LogHelper
    {
        private readonly static ILog Logger;
        private readonly static Level PerfomanceLevel;

        static LogHelper()
        {
            PerfomanceLevel = new Level(35000, "PERFORMANCE");
            XmlConfigurator.Configure();
            Logger = LogManager.GetLogger("JGCK.LOG");
            LogManager.GetRepository().LevelMap.Add(PerfomanceLevel);
        }

        private static string GetLogsFolder()
        {
            var empty = String.Empty;
            try
            {
                IAppender[] appenders = LogManager.GetRepository().GetAppenders();
                if (appenders != null)
                {
                    FileAppender fileAppender = appenders.FirstOrDefault(a => a is FileAppender) as FileAppender;
                    if (fileAppender != null)
                    {
                        string file = fileAppender.File;
                        empty = string.Empty;
                        if (file.Contains("\\Logs\\"))
                        {
                            int num = file.IndexOf("\\Logs\\", StringComparison.Ordinal);
                            empty = file.Substring(0, num + 5);
                        }
                    }
                }
            }
            catch
            {
                empty = String.Empty;
            }

            return empty;
        }

        public static void LogCriticalError(string msg, params object[] parameters)
        {
            if (Logger.IsFatalEnabled)
            {
                Logger.FatalFormat(msg, parameters);
            }
        }

        public static void LogCriticalError(string msg, Exception exc)
        {
            if (Logger.IsFatalEnabled)
            {
                Logger.Fatal(msg, exc);
            }
        }

        public static void LogCriticalError(Exception exc)
        {
            if (Logger.IsFatalEnabled)
            {
                Logger.Fatal(exc.ToString());
            }
        }

        public static void LogDebugMessage(string msg, params object[] parameters)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat(msg, parameters);
            }
        }

        public static void LogError(string msg, params object[] parameters)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.ErrorFormat(msg, parameters);
            }
        }

        public static void LogError(string msg, Exception exc)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(msg, exc);
            }
        }

        public static void LogError(Exception exc)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(exc.ToString());
            }
        }

        public static void LogInformation(string msg, params object[] parameters)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.InfoFormat(msg, parameters);
            }
        }

        public static void LogPerfomance(this Type objType, string message, Exception exc)
        {
            Logger.Logger.Log(objType.GetType(), PerfomanceLevel, message, exc);
        }

        public static void LogPerfomance(this Type objType, string message)
        {
            Logger.Logger.Log(objType.GetType(), PerfomanceLevel, message, null);
        }
    }
}
