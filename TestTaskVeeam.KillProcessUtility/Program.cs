using Microsoft.Extensions.DependencyInjection;
using TestTaskVeeam.KillProcessUtility;
using TestTaskVeeam.KillProcessUtility.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Process Monitor Utility!");
        Console.WriteLine("This utility monitors a specified process and terminates it if it runs longer than the specified duration.");

        if (args.Length != 3)
        {
            Console.WriteLine("\nUsage:");
            Console.WriteLine("You should provide the following parameters: <processName> <maxLifetimeInMinutes> <monitoringFrequencyInMinutes>");
            Console.WriteLine(" - processName: The name of the process to monitor.");
            Console.WriteLine(" - maxLifetimeInMinutes: The maximum lifetime allowed for the process in minutes.");
            Console.WriteLine(" - monitoringFrequencyInMinutes: The frequency of monitoring the process in minutes.");
            return;
        }

        if (!int.TryParse(args[1], out int maxLifetime) || !int.TryParse(args[2], out int monitoringFrequency))
        {
            Console.WriteLine("Invalid arguments. MaxLifetimeInMinutes and MonitoringFrequencyInMinutes must be integers.");
            return;
        }

        var serviceProvider = ServiceProviderBuilder.Build();

        // Resolve and run ProcessMonitor
        var processMonitor = serviceProvider.GetService<IProcessMonitor>();
        processMonitor.StartMonitoring(args[0], maxLifetime, monitoringFrequency);
    }
}

