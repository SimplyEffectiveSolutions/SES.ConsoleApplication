using ConsoleAppFramework;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace SES.ConsoleApplication.Filters;

internal sealed class ReplaceLogFilter(ConsoleAppFilter next, ILogger<Program> logger) : ConsoleAppFilter(next)
{
    public override Task InvokeAsync(ConsoleAppContext context, CancellationToken cancellationToken)
    {
        ConsoleApp.Log = msg => logger.ZLogInformation($"{msg}");
        ConsoleApp.LogError = msg => logger.ZLogError($"{msg}");

        return Next.InvokeAsync(context, cancellationToken);
    }
}
