using Microsoft.AspNetCore.Html;
using System.Text;

namespace SK.Framework.MVC;

public static class Fmt2
{
    public static IHtmlContent Mode(FormMode mode)
        => (mode == FormMode.Edit) ? new HtmlString("Update") : new HtmlString("Create");

    public static IHtmlContent Email(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new HtmlString("");

        var link = $"<a href=\"mailto:{email}\">{email}</a>";
        return new HtmlString(link);
    }

    public static IHtmlContent Link(string url, bool isRelative = false, bool isNewWindow = false)
    {
        if (string.IsNullOrWhiteSpace(url))
            return new HtmlString("");

        var newWindowAttr = "";

        if (isNewWindow)
            newWindowAttr = " target=\"new\" ";

        if (isRelative || url.Contains("http"))
        {
            var link = $"<a href=\"{url}\"{newWindowAttr}>{url}</a>";
            return new HtmlString(link);
        }

        var link2 = $"<a href=\"http://{url}\"{newWindowAttr}>{url}</a>";
        return new HtmlString(link2);
    }

    public static IHtmlContent Date(DateTime? dateTime)
    {
        if (!dateTime.HasValue)
            return new HtmlString("");
        else
        {
            var date = DateWhenCulture(dateTime.Value);
            var result = new HtmlString(date);

            return result;
        }
    }

    public static string DateWhenCulture(DateTime date)
    {
        var lang = Env.CurrentCultureCode.ToSupportedLang();

        if (lang == SupportedLanguage.English || lang == SupportedLanguage.German)
            return date.ToString("D");
        else if (lang == SupportedLanguage.Arabic)
        {
            string arabicDate = "";

            if (date.DayOfWeek == DayOfWeek.Saturday)
                arabicDate = arabicDate + "السبت";

            if (date.DayOfWeek == DayOfWeek.Sunday)
                arabicDate = arabicDate + "الاحد";

            if (date.DayOfWeek == DayOfWeek.Monday)
                arabicDate = arabicDate + "الاثنين";

            if (date.DayOfWeek == DayOfWeek.Tuesday)
                arabicDate = arabicDate + "الثلاثاء";

            if (date.DayOfWeek == DayOfWeek.Wednesday)
                arabicDate = arabicDate + "الاربعاء";

            if (date.DayOfWeek == DayOfWeek.Thursday)
                arabicDate = arabicDate + "الخميس";

            if (date.DayOfWeek == DayOfWeek.Friday)
                arabicDate = arabicDate + "الجمعة";

            arabicDate = arabicDate + ", " + date.ToString("dd") + " ";

            if (date.Month == 1)
                arabicDate = arabicDate + "يناير";

            if (date.Month == 2)
                arabicDate = arabicDate + "فبراير";

            if (date.Month == 3)
                arabicDate = arabicDate + "مارس";

            if (date.Month == 4)
                arabicDate = arabicDate + "ابريل";

            if (date.Month == 5)
                arabicDate = arabicDate + "مايو";

            if (date.Month == 6)
                arabicDate = arabicDate + "يونيو";

            if (date.Month == 7)
                arabicDate = arabicDate + "يوليو";

            if (date.Month == 8)
                arabicDate = arabicDate + "اغسطس";

            if (date.Month == 9)
                arabicDate = arabicDate + "سبتمبر";

            if (date.Month == 10)
                arabicDate = arabicDate + "اكتوبر";

            if (date.Month == 11)
                arabicDate = arabicDate + "نوفمبر";

            if (date.Month == 12)
                arabicDate = arabicDate + "ديسمبر";

            arabicDate = arabicDate + " ," + date.ToString("yyyy");

            return arabicDate;
        }
        return date.ToString("D");
    }

    public static IHtmlContent ArabicNumber(double? number)
    {
        if (!number.HasValue)
            return new HtmlString("");

        if (Env.IsArabic())
            return new HtmlString(ConvertToEasternArabicNumerals(number!.Value.ToString()));

        return new HtmlString($"{number}");
    }

    public static string ConvertToEasternArabicNumerals(string input)
    {
        var utf8Encoder = new UTF8Encoding();
        var utf8Decoder = utf8Encoder.GetDecoder();
        var convertedChars = new StringBuilder();
        var convertedChar = new char[1];
        var bytes = new byte[] { 217, 160 };
        var inputCharArray = input.ToCharArray();
        foreach (var c in inputCharArray)
        {
            if (char.IsDigit(c))
            {
                bytes[1] = Convert.ToByte(160 + char.GetNumericValue(c));
                utf8Decoder.GetChars(bytes, 0, 2, convertedChar, 0);
                convertedChars.Append(convertedChar[0]);
            }
            else
                convertedChars.Append(c);
        }

        return convertedChars.ToString();
    }

    /// <summary>
    /// Use directly on Razor
    /// </summary>
    /// <param name="arabic"></param>
    /// <param name="english"></param>
    /// <param name="isDefaultArabicWhenEnglishIsNull"></param>
    /// <returns></returns>
    public static IHtmlContent WhenCulture(string? arabic, string? english, bool isDefaultArabicWhenEnglishIsNull = false, bool isDefaultEnglishWhenArabicIsNull = false)
    {
        var lang = Env.CurrentCultureCode.ToSupportedLang();

        if (lang == SupportedLanguage.English || lang == SupportedLanguage.German)
        {
            if (isDefaultArabicWhenEnglishIsNull)
                return new HtmlString(string.IsNullOrEmpty(english) ? arabic : english);
            else
                return new HtmlString(english ?? string.Empty);
        }
        else if (lang == SupportedLanguage.Arabic)
        {
            if (isDefaultEnglishWhenArabicIsNull)
                return new HtmlString(string.IsNullOrEmpty(arabic) ? english : arabic);
            else
                return new HtmlString(arabic ?? string.Empty);
        }

        return new HtmlString(string.Empty);
    }

    /// <summary>
    /// Use when the return goes to another method
    /// </summary>
    /// <param name="arabic"></param>
    /// <param name="english"></param>
    /// <returns></returns>
    public static string WhenCulture2(string? arabic, string? english)
    {
        var lang = Env.CurrentCultureCode.ToSupportedLang();

        return WhenCulture3(lang, arabic, english);
    }

    static string WhenCulture3(SupportedLanguage lang, string? arabic, string? english)
    {
        if (lang == SupportedLanguage.English || lang == SupportedLanguage.German)
            return english ?? string.Empty;
        else if (lang == SupportedLanguage.Arabic)
            return arabic ?? string.Empty;

        return string.Empty;
    }

    public static string YesOrNo(bool? value)
    {
        var lang = Env.CurrentCultureCode.ToSupportedLang();
        if(lang == SupportedLanguage.Arabic)
        {
            if (value == true)
                return "نعم";
            else if (value == false)
                return "لا";
            else
                return "";
        }
        else
        {
            if (value == true)
                return "Yes";
            else if (value == false)
                return "No";
            else
                return "";
        }
    }
}
