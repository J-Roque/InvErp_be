using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Messaging.API.Interceptors;

public class ErrorHandlingInterceptor: Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new RpcException(new Status(StatusCode.Internal, $"Unexpected error ocurred: {e.Message}"));
        }
    }
}