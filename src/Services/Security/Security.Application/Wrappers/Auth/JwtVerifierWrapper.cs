using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Security.Application.Wrappers.Exceptions;
using Utilities.Protos.JwtVerifier;

namespace Security.Application.Wrappers.Auth;

public class JwtVerifierWrapper
{
    private readonly JwtVerifierService.JwtVerifierServiceClient _client;

    public JwtVerifierWrapper(string address)
    {
        // LOCAL
        var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(new HttpClientHandler())
        });

        // PROD
        //var channel = GrpcChannel.ForAddress(address);
        
        _client = new JwtVerifierService.JwtVerifierServiceClient(channel);
    }
    
    public async Task<JwtVerifierResponse> VerifyJwt(JwtVerifierRequest request)
    {
        try
        {
            var grpcCall = _client.VerifyJwtAsync(request);
            return await HandleWrapperException.CallGrpcServiceAsync<JwtVerifierResponse>(grpcCall);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


}