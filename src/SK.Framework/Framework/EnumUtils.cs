using System.ComponentModel;

namespace SK.Framework;

public static class EnumUtils
{
    public static string GetDescription(System.Enum value)
    {
        var fi = value.GetType().GetField(value.ToString())!;
        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }
}
