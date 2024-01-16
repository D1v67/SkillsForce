using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce.BackgoundJobLogger
{
    public class JobLogger : IJobLogger
    {
        private readonly string _loggerFilePath;
        private const string LOGGER_FILE_PATH = "JobLog\\log.txt";

        public JobLogger()
        {
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _loggerFilePath = Path.Combine(rootDirectory, LOGGER_FILE_PATH);
        }

        public void Log(string message)
        {
            string errorLogMessage = $"{message}";

            try
            {
                if (!File.Exists(_loggerFilePath))
                {
                    File.Create(_loggerFilePath);
                }
                using (StreamWriter writer = File.AppendText(_loggerFilePath))
                {
                    writer.WriteLine(errorLogMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
