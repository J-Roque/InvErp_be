using Auth.Application.Configuration.Settings;
using Microsoft.Extensions.Configuration;

namespace Auth.Application.Configuration;

public sealed class AppConstants(IConfiguration configuration)
{
    public string ConnectionString { get; } = configuration.GetConnectionString("Database") ?? string.Empty;

    public JwtSettings Jwt { get; } = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();

    public FrontendSettings Frontend { get; } =
        configuration.GetSection("Frontend").Get<FrontendSettings>() ?? new FrontendSettings();

    public BackendSettings Backend { get; } =
        configuration.GetSection("Backend").Get<BackendSettings>() ?? new BackendSettings();
}
