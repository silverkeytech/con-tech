using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace SK.Framework.MVC;

public class ApiProblemsDetails : ProblemDetails
{
    public string? RequestPath { get; set; } 

    public object? RequestPayload { get; set; } 

    public IReadOnlyDictionary<string, string> ValidationMessages { get; set; } = new Dictionary<string, string>();

    public string? StackTrace { get; set; }
}

public static class ControllerExtensions
{
    /// <summary>
    /// This method produce an extension to a machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="statusCode"></param>
    /// <param name="title"></param>
    /// <param name="detail"></param>
    /// <param name="exception"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public static ActionResult ApiProblemDetails(this ControllerBase self, HttpStatusCode statusCode,
        string title = "",
        string detail = "",
        IReadOnlyDictionary<string, string>? validationMessages = null,
        ModelStateDictionary? modelState = null,
        Exception? exception = null,
        object? request = null)
    {
        var details = new ApiProblemsDetails
        {
            Title = title,
            Detail = detail,
            RequestPath = self.Request.GetDisplayUrl(),
            Status = (int)statusCode
        };

        if (validationMessages != null)
            details.ValidationMessages = validationMessages;

        if (modelState != null)
        {
            var modelStateMessages = modelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => string.Join(",", kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray())
            );

            if (details.ValidationMessages == null)
                details.ValidationMessages = modelStateMessages;
            else
                details.ValidationMessages = details.ValidationMessages.Union(modelStateMessages).ToDictionary(x => x.Key, x => x.Value);
        }

        if (exception != null)
        {
            if (string.IsNullOrWhiteSpace(details.Detail))
                details.Detail = "Exception Message: " + exception.Message;
            else
                details.Detail += "\n\n Exception Message: " + exception.Message;

            details.StackTrace = exception?.StackTrace ?? string.Empty;
        }

        if (request != null)
            details.RequestPayload = request;

        return self.StatusCode((int)statusCode, details);
    }

    public static Dictionary<string, string> ApiValidationMessages(this Controller self, params (string key, string message)[] messages)
    {
        var dictionary = new Dictionary<string, string>();

        foreach (var m in messages)
            dictionary.Add(m.key, m.message);

        return dictionary;
    }
}
