using System.Text;

namespace Auth.API.Security;

public static class JwtSecretValidator
{
    // HS256 necesita una clave de al menos 256 bits
    private const int MinimumSecretBytes = 32;

    // Marcadores de ejemplo que nunca deben usarse para firmar tokens reales
    private static readonly string[] PlaceholderFragments = ["your-super-secret", "change-me", "changeme"];

    /// <summary>
    /// Impide arrancar fuera de Development con una clave JWT vacía, corta o de ejemplo.
    /// En Development solo avisa, para no bloquear el trabajo local.
    /// </summary>
    public static void ValidateJwtSecret(this WebApplication app)
    {
        var secret = app.Configuration["Jwt:Secret"];
        var problem = GetProblem(secret);
        if (problem == null) return;

        if (app.Environment.IsDevelopment())
        {
            app.Logger.LogWarning("Jwt:Secret no es seguro ({Problem}). Configura una clave real antes de desplegar.", problem);
            return;
        }

        throw new InvalidOperationException(
            $"Jwt:Secret no es seguro ({problem}). Configúralo con una clave aleatoria de al menos " +
            $"{MinimumSecretBytes} bytes, por ejemplo con la variable de entorno Jwt__Secret.");
    }

    private static string? GetProblem(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
            return "está vacío";

        if (PlaceholderFragments.Any(p => secret.Contains(p, StringComparison.OrdinalIgnoreCase)))
            return "es un valor de ejemplo";

        if (Encoding.UTF8.GetByteCount(secret) < MinimumSecretBytes)
            return $"tiene menos de {MinimumSecretBytes} bytes";

        return null;
    }
}
