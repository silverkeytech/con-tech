using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Serilog.Events;
using Serilog;
using SK.Framework.Email;
using Serilog.Enrichers.Span;

namespace SK.Framework;

public record ExtraEmailConfig(bool IsFakeServerEnabled);

public static class StartupHelpers
{
    public static void ConfigureCulture(IServiceCollection services)
    {
        var english = new CultureInfo("en-GB");
        english.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";

        var arabic = new CultureInfo("ar-EG", true);
        arabic.DateTimeFormat = new CultureInfo("en-GB").DateTimeFormat;
        arabic.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

        var supportedCultures = new CultureInfo[]
        {
            english,
            arabic
        };

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(supportedCultures[1]); //arabic
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new CustomRequestCultureProvider(context =>
            {
                var defaultCulture = options.DefaultRequestCulture.Culture;

                var segments = context.Request.Path.Value?.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (segments?.Length > 0)
                {
                    foreach (var c in options.SupportedCultures)
                    {
                        if (c.Name.StartsWith(segments[0]))
                            return Task.FromResult(new ProviderCultureResult(c.Name))!;
                    }
                }

                return Task.FromResult(default(ProviderCultureResult));
            }));

            options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
        });
    }

    public static ExtraEmailConfig ConfigureEmail(IServiceCollection services, IConfiguration config)
    {
        var mailConfig = config.GetSection("Email")!;

        var emailServerData = new EmailServerData()
        {
            Host = mailConfig["EmailHost"]!,
            Port = mailConfig["EmailPort"]!,
            UserName = mailConfig["EmailUsername"]!,
            Password = mailConfig["EmailPassword"]!,
        };

        services.AddSingleton(emailServerData);

        var emailSender = GetDefaultSender(mailConfig);
        services.AddSingleton(emailSender);

        services.AddOptions<EmailConfig>()
            .Configure(options =>
            {
                options.RootFolder = mailConfig["RootFolder"]!;

                options.Template.Folder = mailConfig["Template:Folder"]!;
                options.Template.LayoutFolder = mailConfig["Template:LayoutFolder"]!;

                options.FakeServer.OutputFolder = mailConfig["FakeServer:Folder"]!;

                if (bool.TryParse(mailConfig["FakeServer:Enabled"], out var isEnabled))
                {
                    options.FakeServer.Enabled = isEnabled;
                }
                else
                {
                    options.FakeServer.Enabled = false;
                }

                options.DefaultSender = emailSender;
            })
            .Validate(options =>
            {
                var vals = new string[]
                {
                    options.RootFolder,
                    options.Template.Folder,
                    options.Template.LayoutFolder,
                    options.FakeServer.OutputFolder,
                };

                foreach (var v in vals)
                {
                    if (string.IsNullOrWhiteSpace(v))
                        return false;
                }

                return true;
            });

        if (bool.TryParse(mailConfig["FakeServer:Enabled"], out var isEnabled))
            return new ExtraEmailConfig(IsFakeServerEnabled: isEnabled);
        else
            return new ExtraEmailConfig(IsFakeServerEnabled: false);
    }

    public static MailboxAddress GetDefaultSender(IConfiguration mailConfig)
    {
        string email = mailConfig["DefaultSenderEmail"]!;
        string name = mailConfig["DefaultSenderName"]!;
        return new MailboxAddress(name, email);
    }

    public static void AddDefaultLogger(this IServiceCollection services, string serviceName, string env, Func<LoggerConfiguration, LoggerConfiguration>? func = null, LogEventLevel level = LogEventLevel.Verbose)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
            .Enrich.WithProperty("ServiceName", serviceName)
            .Enrich.WithProperty("Environment", env)
            .Enrich.FromLogContext()
            .Enrich.WithSpan()
#if DEBUG  // Remove support of console logging in production. This is very important and crucial.
            .WriteTo.Console(
                restrictedToMinimumLevel: level)
#endif
            .WriteTo.File(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", $"{serviceName}_log_.txt"),
                restrictedToMinimumLevel: level,
                rollingInterval: RollingInterval.Day);

        if (func is not null)
            loggerConfiguration = func(loggerConfiguration);

        Log.Logger = loggerConfiguration.CreateLogger();

        Log.Information($"{serviceName} Starting with env {env} ...");

        services.AddLogging(logging =>
        {
            logging.AddSerilog(Log.Logger, dispose: true);
        });
    }

}
