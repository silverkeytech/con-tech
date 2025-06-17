namespace SK.Framework.Email;

public class Attachment
{
    public required byte[] Content { get; set; } = new byte[0];

    public required string FileName { get; set; } = string.Empty;

    public required string MediaType { get; set; } = string.Empty; // text, image, application, audio, video,

    public required string MediaSubType { get; set; } = string.Empty; // plain, html, jpeg, png, pdf, zip, etc.
}

public class TemplatedTextEmail
{
    public string TemplateFile { get; set; } = string.Empty;

    public object TemplateData { get; set; } = string.Empty;
}

public class TemplatedTextEmailWithAttachment : TemplatedTextEmail
{
    public Attachment? Attachment { get; set; }
}

public class TemplatedHtmlEmail
{
    public string TemplateFile { get; set; } = string.Empty;

    public object TemplateData { get; set; } = string.Empty;

    public string LayoutFile { get; set; } = string.Empty;
}

public class TemplatedHtmlEmailWithAttachment : TemplatedHtmlEmail
{
    public List<Attachment> Attachments { get; set; } = new();
}

public class EmailTemplateData
{
    public string SubjectTemplate { get; set; } = string.Empty;

    public string BodyTemplate { get; set; } = string.Empty;

    public object Tokens { get; set; } = new();
}

public class EmailTemplate
{
    public string Subject { get; set; } = string.Empty;

    public List<string> Body { get; set; } = new();
}