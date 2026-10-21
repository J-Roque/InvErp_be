using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Security.Application.Wrappers.Exceptions;
using Utilities.Protos.SignFileAndUpdate;

namespace Security.Application.Wrappers.Attachments;

public class SignFileAndUpdateWrapper
{
    private readonly SignFileAndUpdateService.SignFileAndUpdateServiceClient _client;

    public SignFileAndUpdateWrapper(string address)
    {
        // LOCAL
        var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        });

        // PROD
        //var channel = GrpcChannel.ForAddress(address);

        _client = new SignFileAndUpdateService.SignFileAndUpdateServiceClient(channel);
    }

    public async Task<SignFileAndUpdateResponse> SignFileAndUpdate(SignFileAndUpdateRequest request)
    {
        try
        {
            var grpcCall = _client.SignFileAndUpdateAsync(request);
            return await HandleWrapperException.CallGrpcServiceAsync<SignFileAndUpdateResponse>(grpcCall);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}