namespace Microsoft.AspNetCore.Builder;

public static class RouteBuilderExtensions
{
    public static RouteHandlerBuilder ConfigureRoute<TSuccess, TFail>(
        this RouteHandlerBuilder builder,
        int successStatus,
        int failStatus,
        bool requireAuthorization = true,
        bool producesNotFound = false,
        params string[] policyNames)
    {
        if (requireAuthorization)
        {
            builder
                .RequireAuthorization(policyNames)
                .Produces(StatusCodes.Status401Unauthorized);
        }

        if (successStatus < 300)
            builder.Produces<TSuccess>(successStatus);

        if (producesNotFound)
            builder.Produces(StatusCodes.Status404NotFound);

        return builder
            .Produces<TFail>(failStatus)
            .WithOpenApi();
    }
}