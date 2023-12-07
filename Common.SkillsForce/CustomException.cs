using Common.SkillsForce.AppLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SkillsForce
{
    public class CustomException : Exception
    {
        private readonly Logger _logger;
        public CustomException(string message, Exception exception) : base(message, exception)
        {
            _logger = new Logger();
        }

        public void Log()
        {
            string fullMessage = "--------------------------------------------------";
            fullMessage += Environment.NewLine + $"Timestamp: {DateTime.Now}";
            fullMessage += Environment.NewLine + $"Exception Type: {this.GetType().FullName}";
            fullMessage += Environment.NewLine + $"Message: {this.Message}";
            fullMessage += Environment.NewLine + $"Inner Exception: {this.InnerException}";
            fullMessage += Environment.NewLine + $"Stack Trace: {this.StackTrace}";
            fullMessage += Environment.NewLine + "--------------------------------------------------";
            _logger.Log(fullMessage);
        }
    }
}
