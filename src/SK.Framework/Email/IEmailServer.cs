using MailKit.Net.Smtp;
using MimeKit;
using Serilog;

namespace SK.Framework.Email;

public interface IEmailServer
{
    Task<Result<Nothing>> SendAsync(MimeMessage message);
}

public class EmailServer : IEmailServer
{
    private readonly EmailServerData _emailServerData;

    public EmailServer(EmailServerData emailServerData)
    {
        _emailServerData = emailServerData;
    }

    public async Task<Result<Nothing>> SendAsync(MimeMessage message)
    {
        try
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_emailServerData.Host, int.Parse(_emailServerData.Port), false);

                client.Authenticate(_emailServerData.UserName, _emailServerData.Password);

                client.Send(message);
                client.Disconnect(true);
            }

            return Result<Nothing>.True(Nothing.Instance);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error sending email");
            return Result<Nothing>.False(ex);
        }
    }
}

public class EmailServerData
{
    public string Host { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Port { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public bool HasCredentials()
    {
        return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
    }
}

public class EmailConfig
{
    public string RootFolder { get; set; } = string.Empty;

    public EmailTemplate Template { get; set; } = new();

    public class Fake
    {
        public string OutputFolder { get; set; } = string.Empty;

        public bool Enabled { get; set; }
    }

    public Fake FakeServer { get; set; } = new Fake();

    public class EmailTemplate
    {
        public string Folder { get; set; } = string.Empty;

        public string LayoutFolder { get; set; } = string.Empty;
    }

    public string StandardFooter { get; set; } = string.Empty;

    public MailboxAddress? DefaultSender { get; set; }
}