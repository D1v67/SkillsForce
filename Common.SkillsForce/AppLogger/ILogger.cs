using System;

namespace Common.SkillsForce.AppLogger
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(Exception exception);
    }
}
