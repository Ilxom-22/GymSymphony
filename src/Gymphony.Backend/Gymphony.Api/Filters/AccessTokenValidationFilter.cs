using System.Security.Authentication;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gymphony.Api.Filters;

public class AccessTokenValidationFilter(
    IRequestContextProvider requestContextProvider,
    IAccessTokenRepository accessTokenRepository)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var isAuthorized = context.ActionDescriptor.EndpointMetadata
            .Any(endpointMetadata => endpointMetadata is AuthorizeAttribute);

        if (isAuthorized)
        {
            var accessTokenId = requestContextProvider.GetUserId();

            if (accessTokenId is null)
                throw new AuthenticationException("Unauthorized access!");

            _ = await accessTokenRepository.GetByUserIdAsync(
                (Guid)accessTokenId,
                new QueryOptions(QueryTrackingMode.AsNoTracking));
        }

        await next();
    }
}