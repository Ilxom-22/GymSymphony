using System.Security.Authentication;
using AutoMapper;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;

namespace Gymphony.Infrastructure.Common.Identity.QueryHandlers;

public class GetCurrentUserQueryHandler(
    IRequestContextProvider requestContextProvider,
    IMapper mapper,
    IUserRepository userRepository)
    : IQueryHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = requestContextProvider.GetUserId()
            ?? throw new AuthenticationException("Unauthorized access!");

        var currentUser = await userRepository.GetByIdAsync(currentUserId,
            new QueryOptions(QueryTrackingMode.AsNoTracking),
            cancellationToken)
            ?? throw new AuthenticationException("Unauthorized access!");

        return mapper.Map<UserDto>(currentUser);
    }
}