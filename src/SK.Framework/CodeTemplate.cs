using Serilog;

namespace SK.Framework;

public static class CodeTemplate
{
    public static void HandleException(Exception exp)
    {
        Log.Error(exp, string.Empty);
    }
}
