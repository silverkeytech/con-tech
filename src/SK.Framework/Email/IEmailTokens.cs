using MimeKit;

namespace SK.Framework.Email;
public interface IEmailTokens
{
}

public interface IDbEmailSingleSender<T> where T: IEmailTokens
{
    Dictionary<SupportedLanguage, string> Templates { get; }

    SupportedLanguage Lang { get; set; }

    T Tokens { get; set; }

    string? LayoutFile { get; set; }

    MailboxAddress? To { get; set; }
}


public interface IFileEmailMultipleSender<T> where T : IEmailTokens
{
    string TemplateFolder { get; }

    Dictionary<SupportedLanguage, string> Templates { get; }

    SupportedLanguage Lang { get; set; }

    T Tokens { get; set; }

    string? LayoutFile { get; set; }

    List<MailboxAddress> To { get; set; }

    List<MailboxAddress> Bcc { get; set; }
}

public interface IDataEmailSingleSender<T> where T : IEmailTokens
{
    public string SubjectTemplate { get; set; }

    public string BodyTemplate { get; set; }

    SupportedLanguage Lang { get; set; }

    T Tokens { get; set; }

    string? LayoutFile { get; set; }

    MailboxAddress? To { get; set; }
}