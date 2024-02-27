using TestTaskVeeam.KillProcessUtility.Interfaces;

namespace TestTaskVeeam.KillProcessUtility
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]: {message}");
        }
    }
}

