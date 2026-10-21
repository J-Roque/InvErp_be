using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace BuildingBlocks.Behaviors
{
    public partial class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
    {
        private const int PERFORMANCE_THRESHOLD_SECONDS = 10;
        private const string REDACTED = "***";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            MaxDepth = 16,
            // Solo se usa para el texto del log; evita escapar comillas y acentos
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
                typeof(TRequest).Name, typeof(TResponse).Name, Sanitize(request));

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > PERFORMANCE_THRESHOLD_SECONDS)
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                    typeof(TRequest).Name, timeTaken.Seconds);

            logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
            return response;
        }

        /// <summary>
        /// Serializa el request ocultando contraseñas, tokens y secretos, para que no queden en los logs.
        /// </summary>
        private static string Sanitize(TRequest request)
        {
            try
            {
                var node = JsonSerializer.SerializeToNode(request, request.GetType(), SerializerOptions);
                Redact(node);
                return node?.ToJsonString(SerializerOptions) ?? string.Empty;
            }
            catch
            {
                // Si no se puede serializar, no se registra el contenido
                return $"<{typeof(TRequest).Name}: contenido no registrado>";
            }
        }

        private static void Redact(JsonNode? node)
        {
            switch (node)
            {
                case JsonObject obj:
                    foreach (var (key, value) in obj.ToList())
                    {
                        if (SensitiveKeyRegex().IsMatch(key))
                            obj[key] = REDACTED;
                        else if (value is JsonValue jsonValue && jsonValue.TryGetValue<string>(out var text))
                            obj[key] = JwtRegex().Replace(text, REDACTED);
                        else
                            Redact(value);
                    }
                    break;
                case JsonArray array:
                    foreach (var item in array)
                        Redact(item);
                    break;
            }
        }

        // Propiedades cuyo valor nunca debe registrarse
        [GeneratedRegex("password|secret|token|jwt", RegexOptions.IgnoreCase)]
        private static partial Regex SensitiveKeyRegex();

        // JWT incrustados en otros textos (por ejemplo, la URL de restablecimiento dentro del cuerpo de un email)
        [GeneratedRegex(@"eyJ[A-Za-z0-9_-]+\.[A-Za-z0-9_-]+\.[A-Za-z0-9_-]*")]
        private static partial Regex JwtRegex();
    }
}
