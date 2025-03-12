using System.Diagnostics;
using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace SES.ConsoleApplication.Filters;

internal class TimerFilter(ConsoleAppFilter next) : ConsoleAppFilter(next)
{
    public override async Task InvokeAsync(ConsoleAppContext context, CancellationToken cancellationToken)
    {
        if (ConsoleApp.ServiceProvider == null)
        {
            return;
        }

        var logFactory = ConsoleApp.ServiceProvider.GetService<ILoggerFactory>();
        var logger = logFactory.CreateLogger($"Command: {context.CommandName}");

        var startTime = Stopwatch.GetTimestamp();
        logger.ZLogInformation($"Starting...");

        try
        {
            await Next.InvokeAsync(context, cancellationToken);
            logger.ZLogInformation($"Finished successfully, Elapsed: {Stopwatch.GetElapsedTime(startTime)}");
        }
        catch
        {
            logger.ZLogWarning($"Finished abnormally, Elapsed: {Stopwatch.GetElapsedTime(startTime)}");
            throw;
        }
    }
}
