/// Logging Library for C#
/// 
/// <author>Jakob Balkovec</author>
/// 
/// <date>July 7, 2023</date>
/// 
/// <version>1.0</version>
///
/// <file>Logger.cs</file>
///
/// <brief>
/// This script contains a logging library implemented through the `g_log` class.
/// It provides functionality for logging messages with different severity levels.
/// The script also includes a filter function for filtering log entries based on log levels.
/// </brief>
/// 
/// <see cref="https://github.com/j-balkovec/Projects/blob/main/Logger/CSLogger/Logger.cs">

using System;
using System.Diagnostics;
using System.Reflection;

namespace Logger
{
    /// <summary>
    /// - Provides a logging mechanism for recording application events, errors, and debugging information.
    /// </summary>
    public class GLog
    {
        /// <constants>
        ///
        /// MaxFiles: int -> The maximum number of log files to keep
        /// MaxSizeOf: int -> The maximum size of a log file in bytes
        ///
        /// </constants>
        private const int MaxFiles = 10;
        private const long MaxSizeOf = 4 * 1024 * 1024;

        ///<remove_all_logging_files>
        ///find {/path/to/wd} -type f -name "*.log" -exec rm { } +
        ///</__remove_all_logging_files>

        ///<severity_levels>
        ///Levels increase with severity in the following manner:
        ///</severity_levels>
        private static readonly string[] Levels = { "DEBUG", "INFO", "WARNING", "ERROR", "CRITICAL" };

        private readonly string FileExtension = ".log";
        private readonly string _filename;
        private List<Func<string, bool>> _filters = new List<Func<string, bool>>();
        private readonly object _lock = new object();

        ///<flags>
        /// _exc_info: bool -> Provides more detailed information about the exception
        /// _traceback: bool -> Provides more detailed information about the traceback
        /// _accurate: bool -> Provides a more detailed date/time format of the log entry
        ///
        ///        - _acurate: bool = true
        ///        {yyyy-mm-dd hh:mm:ss}
        ///    
        ///        
        ///        - _acurate: bool = false
        ///        {hh:mm:ss}
        ///
        /// </flags>

        private readonly bool _exc_info = true;
        private readonly bool _traceback = true;
        private readonly bool _accurate = true;

        /// <summary>
        /// - Initializes a new instance of the <see cref="GLog"/> class.
        /// </summary>
        public GLog()
        {
            string baseFilename = Path.GetFileNameWithoutExtension(path: Assembly.GetEntryAssembly().Location);
            string full_filename = "/Users/jbalkovec/desktop/cpsc/CPSC 3200/P3/P3/logs";
            _filename = Path.Combine(full_filename, $"_{GetUniqueFilename(baseFilename, FileExtension)}");
        }


        /// <summary>
        /// - Generates a unique filename by combining the base filename and an extension.
        /// </summary>
        /// <param name="baseFilename">The base filename.</param>
        /// <param name="extension">The file extension.</param>
        /// <returns>The unique filename.</returns>
        public string GetUniqueFilename(string baseFilename, string extension)
        {
            string filename = $"{baseFilename}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}{extension}";
            return filename;
        }

        /// <summary>
        /// - Retrieves the line number of a method represented by a delegate.
        /// </summary>
        /// <param name="func">The delegate representing the method.</param>
        /// <returns>The line number of the method's source code.</returns>
        public int GetLineNumber(Delegate func)
        {
            if (func.Method != null)
            {
                MethodInfo method = func.Method;
                StackTrace stackTrace = new StackTrace();

                for (int i = 0; i < stackTrace.FrameCount; i++)
                {
#pragma warning disable CS8600
                    StackFrame frame = stackTrace.GetFrame(i);
#pragma warning restore CS8600
#pragma warning disable CS8602
#pragma warning disable CS8600
                    MethodBase methodBase = frame.GetMethod();
#pragma warning restore CS8600
#pragma warning restore CS8602

                    if (methodBase == method)
                    {
                        return frame.GetFileLineNumber() - 1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// - Adds a filter function to the list of log entry filters.
        /// </summary>
        /// <param name="filterFunc">The filter function to add.</param>
        public void AddFilter(Func<string, bool> filterFunc)
        {
            _filters.Add(filterFunc);
        }

        // <summary>
        /// - Determines whether a log entry should be logged based on the registered filters.
        /// </summary>
        /// <param name="logEntry">The log entry to evaluate.</param>
        /// <returns>True if the log entry should be logged, false otherwise.</returns>
        public bool ShouldLog(string logEntry)
        {
            return _filters.All(filterFunc => filterFunc(logEntry));
        }

        /// <summary>
        /// - Logs an event with a specified condition, message, log level, and an optional delegate for execution.
        /// </summary>
        /// <param name="condition">The condition or context of the log entry.</param>
        /// <param name="message">The log message.</param>
        /// <param name="level">The log level (0 = DEBUG, 1 = INFO, 2 = WARNING, 3 = ERROR, 4 = CRITICAL).</param>
        /// <param name="func">An optional delegate representing a function to log.</param>
        /// <returns>The generated log entry.</returns>
        public string Log(string message, int level, string? condition = null, Delegate? func = null)
        {
            lock (_lock)
            {
                if (level < 0 || level >= Levels.Length)
                {
                    throw new ArgumentException("[invalid log level]");
                }

                string levelStr = Levels[level];
                string conditionStr = condition ?? "[No Condition]";
                string funcStr = func?.Method?.Name ?? "[No Function]";

                string logEntry = $"\n[{GetTimestamp()}]::[{levelStr}] {message} \n {funcStr}(): {conditionStr}";

                if (func != null)
                {
                    if (_exc_info)
                    {
                        try
                        {
                            if (!ShouldLog(logEntry))
                            {
                                return string.Empty;
                            }
                            else
                            {
                                logEntry += $"\n[func definition] => {func.Method.Name}() [line]: {GetLineNumber(func)}";

                                var stopwatch = Stopwatch.StartNew();
                                func.DynamicInvoke();
                                stopwatch.Stop();
                                double executionTime = stopwatch.Elapsed.TotalSeconds;
                                logEntry += $"\n[execution time] => {Math.Round(executionTime)} seconds for {func.Method.Name}";
                            }
                        }
                        catch (Exception ex)
                        {
                            string exceptionStr = $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
                            logEntry += $"\n\n*** ERROR ***\n{exceptionStr}";

                            if (_traceback)
                            {
                                logEntry += $"\n[traceback]: \n{ex.StackTrace}";
                            }
                        }
                    }
                }
                WriteToFile(logEntry);
                return logEntry;
            }
        }

        /// <summary>
        /// - Gets the current timestamp in an accurate or simplified format.
        /// </summary>
        /// <returns>The formatted timestamp.</returns>
        public string GetTimestamp()
        {
            return _accurate ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// - Writes a log entry to the log file.
        /// </summary>
        /// <param name="logEntry">The log entry to write.</param>
        public void WriteToFile(string logEntry)
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(_filename) && new FileInfo(_filename).Length >= MaxSizeOf)
                    {
                        RotateFiles();
                    }

                    using (StreamWriter file = new StreamWriter(_filename, true))
                    {
                        file.WriteLine(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// - Rotates log files by moving the current log to a backup and removing excess backups.
        /// </summary>
        public void RotateFiles()
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                string backupFilename = $"{_filename}_{timestamp}.log";
                File.Move(_filename, backupFilename);

                string[] strings = Directory.GetFiles(path: Path.GetDirectoryName(_filename))
                                            .Where(f => f.StartsWith(_filename))
                                            .OrderByDescending(f => f)
                                            .ToArray();
                string[] existingBackups = strings;

                if (existingBackups.Length >= MaxFiles)
                {
                    for (int i = MaxFiles; i < existingBackups.Length; i++)
                    {
                        File.Delete(existingBackups[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error rotating log files: {ex.Message}");
            }
        }

        /// <summary>
        /// - Filters log entries by log level based on a list of log levels.
        /// </summary>
        /// <param name="logEntry">The log entry to filter.</param>
        /// <param name="levels">The list of log levels to filter by.</param>
        /// <returns>True if the log entry's log level is in the provided list of levels, false otherwise.</returns>
        public static bool FilterByLogLevel(string logEntry, List<string> levels)
        {
            string logLevel = logEntry.Split(new string[] { "::" }, StringSplitOptions.None)[1].Split(']', 2)[0].Trim();
            return levels.Contains(logLevel);
        }
    }
}



