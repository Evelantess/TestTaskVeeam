namespace TestTaskVeeam.KillProcessUtility.Interfaces
{
    public interface IProcessMonitor
	{
        void StartMonitoring(string processName, int maxLifetime, int monitoringFrequency);
    }
}

