using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace SK.Framework;
public class LogEventSink : ILogEventSink
{
    private readonly LogEventLevel? _logLevel;

    public LogEventSink(string filterLogLevel)
    {
        _logLevel = null;
        if (Enum.TryParse(typeof(LogEventLevel), filterLogLevel, true, out var value))
        {
            _logLevel = (LogEventLevel?)value;
        }
    }

    protected Activity? GetCurrentSpan()
    {
        var span = Activity.Current;
        while (span is object && span.Parent is object && span.Source.Name == string.Empty)
            span = span.Parent;
        return span;
    }

    public void Emit(LogEvent logEvent)
    {
        if (_logLevel.HasValue && logEvent.Level >= _logLevel.Value)
        {
            var span = GetCurrentSpan();
            if (span is object && span.Recorded)
            {
                string message = logEvent.RenderMessage();
                var logProperties = logEvent.Properties.Select(x => KeyValuePair.Create(x.Key, (object?)x.Value)).ToList();
                logProperties.Add(KeyValuePair.Create("Level", (object?)logEvent.Level.ToString()));
                if (logEvent.Exception is not null)
                {
                    logProperties.Add(KeyValuePair.Create("exception", (object?)logEvent.Exception));
                    logProperties.Add(KeyValuePair.Create("exception.type", (object?)(logEvent.Exception.GetType().FullName ?? "No Exception Type")));
                    logProperties.Add(KeyValuePair.Create("exception.stacktrace", (object?)(logEvent.Exception.StackTrace ?? "No StackTrace")));
                    logProperties.Add(KeyValuePair.Create("exception.message", (object?)(logEvent.Exception.Message ?? "No Message")));
                }
                span.AddEvent(new ActivityEvent(message, logEvent.Timestamp.ToUniversalTime(), new ActivityTagsCollection(logProperties)));
            }
        }
    }
}