using BuildingBlocks.Exceptions;
using Grpc.Core;

namespace Security.Application.Wrappers.Exceptions;

public static class HandleWrapperException
{
    public static async Task<T> CallGrpcServiceAsync<T>(AsyncUnaryCall<T> grpcCall)
    {
        try
        {
            return await grpcCall.ResponseAsync;
        }
        catch (RpcException e)
        {
            HandleRpcException(e);
            throw;
        }
    }

    public static void HandleRpcException(RpcException ex)
    {
        throw ex.StatusCode switch
        {
            StatusCode.Internal => new InternalServerException(ex.Message),
            StatusCode.Unauthenticated => new Exception("Unauthenticated"),
            StatusCode.PermissionDenied => new Exception("Permission denied"),
            StatusCode.Unimplemented => new Exception("Unimplemented"),
            StatusCode.Unavailable => new Exception("Service unavailable"),
            _ => new Exception("Unexpected error")
        };
    }
}