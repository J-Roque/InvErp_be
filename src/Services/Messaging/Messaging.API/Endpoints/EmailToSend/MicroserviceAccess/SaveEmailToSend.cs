using Messaging.Application.Dtos.General;
using Messaging.Application.Handlers.EmailsToSend.Command.SaveEmailToSend;

namespace Messaging.API.Endpoints.EmailToSend.MicroserviceAccess;

public record SaveEmailToSendRequest(
    EmailToSendDto Email
);

public class SaveEmailToSend : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/ms/emails-to-send/save", async (SaveEmailToSendRequest request, ISender sender) =>
        {
            var command = request.Adapt<SaveEmailToSendCommand>();
            var result = await sender.Send(command);
            return result;
        })
            .WithName("PrivateSaveEmailToSend")
            .Produces<SaveEmailToSendResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Save Email To Send")
            .WithDescription("Save Email To Send");
    }
}