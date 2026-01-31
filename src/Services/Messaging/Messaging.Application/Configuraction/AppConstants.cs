using Messaging.Application.Configuraction.Settings;
using Messaging.Application.Configuration.Settings;
using Microsoft.Extensions.Configuration;

namespace Messaging.Application.Configuraction;

public class AppConstants(IConfiguration configuration)
{
    public string ConnectionString { get; } = configuration.GetConnectionString("Database") ?? string.Empty;
    public JwtSettings Jwt { get; } = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
    public FrontendSettings Frontend { get; } =
    configuration.GetSection("Frontend").Get<FrontendSettings>() ?? new FrontendSettings();

    public BackendSettings Backend { get; } =
        configuration.GetSection("Backend").Get<BackendSettings>() ?? new BackendSettings();

    public MailSettings Mail { get; } =
        configuration.GetSection("Mail").Get<MailSettings>() ?? new MailSettings();
}
