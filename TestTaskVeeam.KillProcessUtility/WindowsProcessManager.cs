using TestTaskVeeam.KillProcessUtility.Interfaces;

namespace TestTaskVeeam.KillProcessUtility
{
    public class WindowsProcessManager : IProcessManager
    {
        private readonly ILogger _logger;

        public WindowsProcessManager(ILogger logger)
        {
            _logger = logger ?? new ConsoleLogger();
        }

        public bool IsProcessRunning(string processName)
        {
            try
            {
                return System.Diagnostics.Process.GetProcessesByName(processName).Length > 0;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error checking if process '{processName}' is running: {ex.Message}");
                throw new InvalidOperationException($"Error checking if process '{processName}' is running: {ex.Message}");
            }
        }

        public bool IsProcessRunningLongerThan(string processName, int maxLifetime)
        {
            try
            {
                var processes = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    if ((DateTime.Now - process.StartTime).TotalMinutes > maxLifetime)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error checking if process '{processName}' is running longer than {maxLifetime} minutes: {ex.Message}");
                throw new InvalidOperationException($"Error checking if process '{processName}' is running longer than {maxLifetime} minutes: {ex.Message}");
            }
        }

        public void KillProcess(string processName)
        {
            try
            {
                foreach (var process in System.Diagnostics.Process.GetProcessesByName(processName))
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error killing process '{processName}': {ex.Message}");
                throw new InvalidOperationException($"Error killing process '{processName}': {ex.Message}");
            }
        }
    }
}

