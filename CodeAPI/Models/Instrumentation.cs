namespace CodeAPI.Models;

using System.Diagnostics;
using CodeAPI.Constants;

/// <summary>
/// It is recommended to use a custom type to hold references for ActivitySource.
/// This avoids possible type collisions with other components in the DI container.
/// </summary>
public class Instrumentation : IDisposable
{
    public Instrumentation()
    {
        ActivitySource = new ActivitySource(OpenTelemetryConstants.ServiceName, OpenTelemetryConstants.ServiceVersion);
    }

    public ActivitySource ActivitySource { get; }

    public void Dispose()
    {
        ActivitySource.Dispose();
    }
}
