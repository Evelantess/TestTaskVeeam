namespace TestTaskVeeam.KillProcessUtility.Interfaces
{
    public interface IProcessManager
	{
        bool IsProcessRunning(string processName);
        bool IsProcessRunningLongerThan(string processName, int maxLifetime);
        void KillProcess(string processName);
    }
}

