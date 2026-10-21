using Security.Application.Wrappers.Exceptions;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Utilities.Protos.SaveEmailToSend;

namespace Security.Application.Wrappers.EmailToSend;

public class SaveEmailToSendWrapper
{
    private readonly SaveEmailToSendService.SaveEmailToSendServiceClient _client;

    public SaveEmailToSendWrapper(string address)
    {
        // LOCAL
        var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        });

        // PROD
        //var channel = GrpcChannel.ForAddress(address);

        _client = new SaveEmailToSendService.SaveEmailToSendServiceClient(channel);
    }

    public async Task<SaveEmailToSendResponse?> SaveEmailToSend(SaveEmailToSendRequest request)
    {
        try
        {
            var grpcCall = _client.SaveEmailToSendAsync(request);
            return await HandleWrapperException.CallGrpcServiceAsync<SaveEmailToSendResponse>(grpcCall);
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