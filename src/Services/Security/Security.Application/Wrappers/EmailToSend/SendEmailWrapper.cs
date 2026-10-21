using Security.Application.Wrappers.Exceptions;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Utilities.Protos.SendEmail;

namespace Security.Application.Wrappers.EmailToSend;

public class SendEmailWrapper
{
    private readonly SendEmailService.SendEmailServiceClient _client;

    public SendEmailWrapper(string address)
    {
        // LOCAL
        // var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        // {
        //     HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        // });

        // PROD
        var channel = GrpcChannel.ForAddress(address);

        _client = new SendEmailService.SendEmailServiceClient(channel);
    }

    public async Task<SendEmailResponse?> SendEmail(SendEmailRequest request)
    {
        try
        {
            var grpcCall = _client.SendEmailAsync(request);
            return await HandleWrapperException.CallGrpcServiceAsync<SendEmailResponse>(grpcCall);
        }
        catch (RpcException)
        {
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}