using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SK.Framework.MVC;

public static class FlashPageExtensions
{
    /// <summary>
    /// Enable flash message in three different types.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="type"></param>
    /// <param name="message"></param>
    /// <param name="values"></param>
    public static void Flash(this PageModel self, FlashType type, string message, params object[] values)
        => Flash(self, type, "", message, values);

    /// <summary>
    /// Enable flash message in three different types and specific key channel on which it will be displayed
    /// </summary>
    /// <param name="self"></param>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <param name="message"></param>
    /// <param name="values"></param>
    public static void Flash(this PageModel self, FlashType type, string key, string message, params object[] values)
    {
        self.TempData["FlashMessage_" + key] = string.Format(message, values);
        self.TempData["FlashMessageType_" + key] = type.ToString();
    }

    /// <summary>
    /// Enable notice flash message
    /// </summary>
    /// <param name="self"></param>
    /// <param name="message"></param>
    /// <param name="values"></param>
    public static void FlashNotice(this PageModel self, string message, params object[] values)
        => Flash(self, FlashType.Notice, message, values);

    /// <summary>
    /// Enable success flash message
    /// </summary>
    /// <param name="self"></param>
    /// <param name="message"></param>
    /// <param name="values"></param>
    public static void FlashSuccess(this PageModel self, string message, params object[] values)
        => Flash(self, FlashType.Success, message, values);

    /// <summary>
    /// Enable error flash message
    /// </summary>
    /// <param name="self"></param>
    /// <param name="message"></param>
    /// <param name="values"></param>
    public static void FlashError(this PageModel self, string message, params object[] values)
        => Flash(self, FlashType.Error, message, values);

    /// <summary>
    /// Enable info flash message
    /// </summary>
    /// <param name="self"></param>
    /// <param name="message"></param>
    /// <param name="values"></param>
    public static void FlashInfo(this PageModel self, string message, params object[] values)
        => Flash(self, FlashType.Info, message, values);

    /// <summary>
    /// Pass a certain key to the view to signify certain message intention. It's up to the view to decide what string to display
    /// </summary>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <param name="type"></param>
    public static void FlashKey(this PageModel self, string key, FlashType type = FlashType.Notice)
        => Flash(self, type, key, "");
}
