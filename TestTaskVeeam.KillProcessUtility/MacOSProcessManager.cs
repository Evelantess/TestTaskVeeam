using TestTaskVeeam.KillProcessUtility.Interfaces;

namespace TestTaskVeeam.KillProcessUtility
{
    public class MacOSProcessManager : IProcessManager
    {
        private readonly ILogger _logger;

        public MacOSProcessManager(ILogger logger)
        {
            _logger = logger ?? new ConsoleLogger();
        }

        public bool IsProcessRunning(string processName)
        {
            try
            {
                foreach (var process in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        if (process.ProcessName.Equals(processName))
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error checking if process '{processName}' is running: {ex.Message}");
                    }
                }
                return false;
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
                foreach (var process in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        if (process.ProcessName.Equals(processName) && (DateTime.Now - process.StartTime).TotalMinutes > maxLifetime)
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error checking if process '{processName}' is running longer than {maxLifetime} minutes: {ex.Message}");
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
                foreach (var process in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        if (process.ProcessName.Equals(processName))
                        {
                            Mono.Unix.Native.Syscall.kill(process.Id, Mono.Unix.Native.Signum.SIGKILL);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error killing process '{processName}': {ex.Message}");
                    }
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

