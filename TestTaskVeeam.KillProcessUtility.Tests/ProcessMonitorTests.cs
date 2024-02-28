using Moq;
using NUnit.Framework;
using TestTaskVeeam.KillProcessUtility.Interfaces;

namespace TestTaskVeeam.KillProcessUtility.Tests
{
    [TestFixture]
    public class ProcessMonitorTests
    {
        private ProcessMonitor _processMonitor;
        private Mock<ILogger> _mockLogger;
        private Mock<IProcessManager> _mockProcessManager;

        [SetUp]
        public void Setup()
        {
            _mockProcessManager = new Mock<IProcessManager>();
            _mockLogger = new Mock<ILogger>();
            _processMonitor = new ProcessMonitor(_mockProcessManager.Object, _mockLogger.Object);
        }

        [Test]
        public void Monitor_SuccessfullyKillsLongRunningProcess()
        {
            // Arrange
            string processName = "TextEdit";
            int maxLifetime = 1; // 1 minute
            _mockProcessManager.Setup(p => p.IsProcessRunning(processName)).Returns(true);
            _mockProcessManager.Setup(p => p.IsProcessRunningLongerThan(processName, maxLifetime)).Returns(true);

            // Act
            _processMonitor.StartMonitoring(processName, maxLifetime, 1);

            // Assert
            _mockProcessManager.Verify(p => p.KillProcess(processName), Times.Once);
        }

        [Test]
        public void Monitor_LogsKillingOfProcess()
        {
            // Arrange
            string processName = "TextEdit";
            int maxLifetime = 1; // 1 minute
            _mockProcessManager.Setup(p => p.IsProcessRunning(processName)).Returns(true);
            _mockProcessManager.Setup(p => p.IsProcessRunningLongerThan(processName, maxLifetime)).Returns(true);

            // Act
            _processMonitor.StartMonitoring(processName, maxLifetime, 1);
          
            // Assert
            _mockLogger.Verify(l => l.Log($"Process '{processName}' has exceeded the maximum lifetime of {maxLifetime} minutes. Killing process..."), Times.Once);
            _mockLogger.Verify(l => l.Log($"Process '{processName}' killed."), Times.Once);
        }

        [Test]
        public void StartMonitoring_ProcessNotRunning_LogsExitMessage()
        {
            // Arrange
            string processName = "MyProcess";
            int maxLifetime = 5;
            int monitoringFrequency = 1;
            _mockProcessManager.Setup(p => p.IsProcessRunning(processName)).Returns(false);

            // Act
            _processMonitor.StartMonitoring(processName, maxLifetime, monitoringFrequency);

            // Assert
            _mockLogger.Verify(l => l.Log($"Process '{processName}' is not running. Exiting..."), Times.Once);
        }

        [Test]
        public void StartMonitoring_ProcessExceedsLifetime_KillsProcessAndLogsMessage()
        {
            // Arrange
            string processName = "MyProcess";
            int maxLifetime = 5;
            int monitoringFrequency = 1;
            _mockProcessManager.Setup(p => p.IsProcessRunning(processName)).Returns(true);
            _mockProcessManager.Setup(p => p.IsProcessRunningLongerThan(processName, maxLifetime)).Returns(true);

            // Act
            _processMonitor.StartMonitoring(processName, maxLifetime, monitoringFrequency);

            // Assert
            _mockProcessManager.Verify(p => p.KillProcess(processName), Times.Once);
            _mockLogger.Verify(l => l.Log($"Process '{processName}' has exceeded the maximum lifetime of {maxLifetime} minutes. Killing process..."), Times.Once);
            _mockLogger.Verify(l => l.Log($"Process '{processName}' killed."), Times.Once);
        }

        [Test]
        public void Monitor_StopsWhenMonitoredProcessExitsBeforeMaximumLifetime()
        {
            // Arrange
            string processName = "TextEdit";
            int maxLifetime = 2; // 2 minutes
            _mockProcessManager.Setup(p => p.IsProcessRunning(processName)).Returns(true);

            // Act
            Task.Run(() =>
            {
                _processMonitor.StartMonitoring(processName, maxLifetime, 1);
            });

            // Simulate process exiting before maximum lifetime
            Thread.Sleep(3000); // Wait for 3 seconds
            _mockProcessManager.Setup(p => p.IsProcessRunning(processName)).Returns(false);

            // Wait for the monitoring to finish
            Thread.Sleep(60000); // Wait for 60 seconds to ensure the monitoring thread finishes

            // Assert
            _mockLogger.Verify(l => l.Log($"Process '{processName}' is not running. Exiting..."), Times.Once);
        }
    }
}

