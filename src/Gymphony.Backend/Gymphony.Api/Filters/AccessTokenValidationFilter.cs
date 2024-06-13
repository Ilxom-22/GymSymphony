using System.Security.Authentication;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gymphony.Api.Filters;

public class AccessTokenValidationFilter(
    IRequestContextProvider requestContextProvider,
    IAccessTokenRepository accessTokenRepository,
    IUserRepository userRepository)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var isAuthorized = context.ActionDescriptor.EndpointMetadata
            .Any(endpointMetadata => endpointMetadata is AuthorizeAttribute);
        
        var actionName = context.ActionDescriptor.RouteValues["action"];

        if (isAuthorized)
        {
            var userId = requestContextProvider.GetUserId();

            if (userId is null)
                throw new AuthenticationException("Unauthorized access!");

            _ = await accessTokenRepository.GetByUserIdAsync(
                (Guid)userId,
                new QueryOptions(QueryTrackingMode.AsNoTracking))
                ?? throw new AuthenticationException("Unauthorized access!");

            var user = await userRepository.GetByIdAsync((Guid)userId,
                new QueryOptions(QueryTrackingMode.AsNoTracking)) 
                       ?? throw new AuthenticationException("Unauthorized access!");

            if (actionName is "GetCurrentUser" or "LogOut")
            {
                await next();
                return;
            }
            
            if (user.Status == AccountStatus.Unverified)
                throw new AuthenticationException("Verify your email address please!");
        }

        await next();
    }
}