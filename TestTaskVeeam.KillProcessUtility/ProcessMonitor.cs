using TestTaskVeeam.KillProcessUtility.Interfaces;

namespace TestTaskVeeam.KillProcessUtility
{
    public class ProcessMonitor : IProcessMonitor
    {
        private readonly IProcessManager _processManager;
        private readonly ILogger _logger;

        public ProcessMonitor(IProcessManager processManager, ILogger logger)
        {
            _processManager = processManager;
            _logger = logger;
        }

        public void StartMonitoring(string processName, int maxLifetime, int monitoringFrequency)
        {
            _logger.Log($"Monitoring process '{processName}' with a max lifetime of {maxLifetime} minutes, checking every {monitoringFrequency} minutes.");

            while (true)
            {
                if (!_processManager.IsProcessRunning(processName))
                {
                    _logger.Log($"Process '{processName}' is not running. Exiting...");
                    break;
                }

                if (_processManager.IsProcessRunningLongerThan(processName, maxLifetime))
                {
                    _logger.Log($"Process '{processName}' has exceeded the maximum lifetime of {maxLifetime} minutes. Killing process...");
                    _processManager.KillProcess(processName);
                    _logger.Log($"Process '{processName}' killed.");
                    break;
                }

                Thread.Sleep(monitoringFrequency * 60 * 1000);
            }
        }
    }
}

