using MimeKit;
using MimeKit.Text;

namespace SK.Framework.Email;

public static class CreateMessage
{
    public static MimeMessage Text(MailboxAddress sender, List<MailboxAddress> targets, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(sender);
        foreach (var target in targets)
            message.To.Add(target);

        message.Subject = subject;

        message.Body = new TextPart(TextFormat.Plain)
        {
            Text = body
        };

        return message;
    }

    public static MimeMessage Html(MailboxAddress sender, List<MailboxAddress> targets, List<MailboxAddress> bcc, string subject, string textBody, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(sender);
        foreach (var target in targets)
            message.To.Add(target);

        foreach (var b in bcc)
            message.Bcc.Add(b);

        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = textBody,
            HtmlBody = htmlBody
        };

        message.Body = builder.ToMessageBody();

        return message;
    }

    public static MimeMessage HtmlWithAttachments(MailboxAddress sender, List<MailboxAddress> targets, string subject, string textBody, string htmlBody, IReadOnlyList<Attachment> attachments)
    {
        var message = new MimeMessage();
        message.From.Add(sender);
        foreach (var target in targets)
            message.To.Add(target);

        message.Subject = subject;

        var builder = new BodyBuilder
        {
            TextBody = textBody,
            HtmlBody = htmlBody
        };

        foreach (var a in attachments)
        {
            builder.Attachments.Add(a.FileName, a.Content, new ContentType(a.MediaType, a.MediaSubType));
        }

        message.Body = builder.ToMessageBody();

        return message;
    }
}