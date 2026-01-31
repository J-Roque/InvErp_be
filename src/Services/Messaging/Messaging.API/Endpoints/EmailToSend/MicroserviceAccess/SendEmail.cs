using Messaging.Application.Dtos.General;
using Messaging.Application.Handlers.EmailsToSend.Command.SendEmail;
//using Utilities.Protos.SendEmail;

namespace Messaging.API.Endpoints.EmailToSend.MicroserviceAccess;

public record SendEmailRequest(EmailToSendDto Email);

public class SendEmail: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ms/emails-to-send/send", async (SendEmailRequest request, ISender sender) =>
            {
                var command = request.Adapt<SendEmailCommand>();
                var result = await sender.Send(command);
                return result;
            })
            .WithName("PrivateSendEmail")
            .Produces<SendEmailResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Send Email")
            .WithDescription("Send Email");
    }
}