using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SK.Framework.MVC;

public static class RazorFlashExtensions
{
    private static string GetCssClassFor(FlashType type)
    {
        switch (type)
        {
            case FlashType.Error: return "alert-danger";
            case FlashType.Notice: return "";
            case FlashType.Info: return "alert-info";
            case FlashType.Success: return "alert-success";
            default: return "";
        }
    }

    /// <summary>
    /// Show the flash message in general channel or specific channel. The output uses .flashError, .flashNotice and .flashSuccess css class.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static IHtmlContent Flash(this IHtmlHelper self, string? key = null, bool showList = false, bool addBreak = true)
    {
        var tmpData = self.TempData["FlashMessage_" + key];
        var tmpType = self.TempData["FlashMessageType_" + key];

        if (tmpData != null)
        {
            if (showList && tmpData is string)
            {
                var messages = ((string)tmpData).Split(new char[] { '\r' })
                    .Where(p => p.Trim().Length > 0);

                if (messages.Any())
                    tmpData = "<ul>{0}</ul>";

                var list = "";
                foreach (var m in messages)
                {
                    list += $"<li>{m}</li>";
                }

                tmpData = string.Format((string)tmpData, list);
            }

            if (Enum.TryParse(tmpType + "", out FlashType type))
            {
                var cssClass = GetCssClassFor(type);

                if (addBreak)
                    return new HtmlString($"<br/><div class=\"alert {cssClass}\">{tmpData}</div><br/>");
                else
                    return new HtmlString($"<div class=\"alert {cssClass}\">{tmpData}</div>");
            }
            else
            {
                return new HtmlString($"<div class=\"alert\">{tmpData}</div>");
            }
        }
        else
        {
            return new HtmlString("");
        }
    }

    /// <summary>
    /// Interrogate whether a certain flash type exists
    /// </summary>
    /// <param name="self"></param>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IfFlash(this IHtmlHelper self, FlashType type, string? key = null, bool flush = true)
    {
        var tmpData = self.TempData["FlashMessage_" + key];
        var tmpType = self.TempData["FlashMessageType_" + key];

        if (!flush)
        {
            //reinsert so the data so it's not missing because of value checking
            self.TempData["FlashMessage_" + key] = tmpData;
            self.TempData["FlashMessageType_" + key] = tmpType;
        }

        if (tmpData != null && Enum.TryParse(tmpType + "", out FlashType tp))
        {
            return tp == type;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Show the flash message in general channel or specific channel. The output uses .flashError, .flashNotice and .flashSuccess css class. If key is found, show message.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static IHtmlContent Flash(this IHtmlHelper self, string key, string message, FlashType type)
    {
        var tmpData = self.TempData["FlashMessage_" + key];//ignore
        var flush = self.TempData["FlashMessageType_" + key]; //ignore

        if (tmpData != null)
        {
            var cssClass = GetCssClassFor(type);
            return new HtmlString($"<div class=\"alert {cssClass}\">{message}</div>");
        }
        else
        {
            return new HtmlString("");
        }
    }

    /// <summary>
    /// Show the flass message in the general or specific channel. The output is depending on how the FlashKey format is set up (.flashError, flasjNotice and .flashSuccess)
    /// </summary>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static IHtmlContent Flash(this IHtmlHelper self, string key, string message)
    {
        var tmpData = self.TempData["FlashMessage_" + key];//ignore
        var tmpType = self.TempData["FlashMessageType_" + key]; //ignore

        if (tmpData != null && Enum.TryParse(tmpType + "", out FlashType type))
        {
            var cssClass = GetCssClassFor(type);
            return new HtmlString($"<div class=\"alert {cssClass}\">{message}</div>");
        }

        return new HtmlString("");
    }

    public static IHtmlContent FlashFormat(this IHtmlHelper self, string message, FlashType type, string extraCssClass = "")
    {
        var cssClass = GetCssClassFor(type);
        if (!string.IsNullOrWhiteSpace(extraCssClass))
            return new HtmlString($"<div class=\"alert {cssClass} {extraCssClass}\">{message}</div>");

        return new HtmlString($"<div class=\"alert {cssClass}\">{message}</div>");
    }

    /// <summary>
    /// Format message based on a condition. Otherwise do not show.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IHtmlContent FlashFormat(this IHtmlHelper self, bool condition, string message, FlashType type)
    {
        if (condition)
        {
            var cssClass = GetCssClassFor(type);
            return new HtmlString($"<div class=\"alert {cssClass}\">{message}</div>");
        }
        else
        {
            return new HtmlString("");
        }
    }
}
