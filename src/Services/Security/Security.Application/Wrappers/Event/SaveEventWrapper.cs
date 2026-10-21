using Security.Application.Wrappers.Exceptions;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Utilities.Protos.SaveEvent;

namespace Security.Application.Wrappers.Event;

public class SaveEventWrapper
{
    private readonly SaveEventService.SaveEventServiceClient _client;
    
    public SaveEventWrapper(string address)
    {
        // LOCAL
        // var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        // {
        //     HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        // });

        // PROD
        var channel = GrpcChannel.ForAddress(address);

        _client = new SaveEventService.SaveEventServiceClient(channel);
    }
    
    public async Task<SaveEventResponse?> SaveEvent(SaveEventRequest request)
    {
        try
        {
            var grpcCall = _client.SaveEventAsync(request);
            return await HandleWrapperException.CallGrpcServiceAsync<SaveEventResponse>(grpcCall);
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