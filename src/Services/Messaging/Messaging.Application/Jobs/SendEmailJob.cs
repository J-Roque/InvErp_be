using Mail.Services;
using Messaging.Application.Utilities;
using Quartz;

namespace Messaging.Application.Jobs;

[DisallowConcurrentExecution]
public class SendEmailJob(
    ILogger<SendEmailJob> logger,
    IApplicationDbContext dbContext,
    MailSenderUtility mailSenderUtility) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // Se obtienen todos los correos a notificar
            var emails = await dbContext.EmailsToSend
                .Where(x => x.IsSent == false)
                .ToListAsync();

            if (emails.Count == 0)
            {
                logger.LogInformation("No hay correos para enviar");
                return;
            }

            // Id de emails procesados correctamente
            var successEmailIds = new List<long>();
            var errorEmailIds = new List<long>();

            logger.LogInformation($"Enviando nuevo lote de {emails.Count} emails a las {DateTime.UtcNow}");

            // Se envían los correos
            foreach (var toSend in emails)
            {
                try
                {
                    var htmlBody = await MailService.GetTemplateData(toSend.Template, toSend.Data);
                    await mailSenderUtility.Instance.SendEmailAsync(
                        to: toSend.Recipients,
                        subject: toSend.Subject,
                        htmlBody: htmlBody
                    );

                    successEmailIds.Add(toSend.Id);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error enviando email a {Recipients}: {Message}", 
                        toSend.Recipients, e.Message);
                    errorEmailIds.Add(toSend.Id);
                }
            }

            if (successEmailIds.Count > 0)
            {
                // Se actualizan los correos enviados
                await dbContext.EmailsToSend
                    .Where(x => successEmailIds.Contains(x.Id))
                    .ExecuteUpdateAsync(
                        updates
                            => updates.SetProperty(e => e.IsSent, true)
                                .SetProperty(e => e.SentDate, DateTime.UtcNow)
                    );
            }

            // Se indica cuantos fueron exitos, cuantos fallaron y la hora
            logger.LogInformation($"Emails enviados: {successEmailIds.Count}");
            logger.LogInformation($"Emails fallidos: {errorEmailIds.Count}");
            logger.LogInformation($"Fin de envío de emails a {DateTime.UtcNow}");

            await Task.CompletedTask;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error en SendEmailJob: {e.Message}");
        }
    }
}