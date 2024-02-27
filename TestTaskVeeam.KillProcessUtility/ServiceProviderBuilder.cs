using Microsoft.Extensions.DependencyInjection;
using TestTaskVeeam.KillProcessUtility.Interfaces;

namespace TestTaskVeeam.KillProcessUtility
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build()
        {
            return new ServiceCollection()
                .AddSingleton<IProcessMonitor, ProcessMonitor>()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddSingleton<IProcessManager>(provider =>
                {
                    if (OperatingSystem.IsWindows())
                    {
                        return new WindowsProcessManager(provider.GetService<ILogger>());
                    }
                    else if (OperatingSystem.IsMacOS())
                    {
                        return new MacOSProcessManager(provider.GetService<ILogger>());
                    }
                    else
                    {
                        throw new PlatformNotSupportedException("This platform is not supported.");
                    }
                })
                .BuildServiceProvider();
        }
    }
}

