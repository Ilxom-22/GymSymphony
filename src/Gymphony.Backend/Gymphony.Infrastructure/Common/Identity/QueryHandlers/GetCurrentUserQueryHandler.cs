using System.Security.Authentication;
using AutoMapper;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Identity.QueryHandlers;

public class GetCurrentUserQueryHandler(
    IRequestContextProvider requestContextProvider,
    IMapper mapper,
    IUserRepository userRepository,
    IAdminRepository adminRepository)
    : IQueryHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var currentUser = await userRepository
            .Get(user => user.Id == currentUserId)
            .Include(user => user.ProfileImage!)
            .ThenInclude(pi => pi.StorageFile)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AuthenticationException("Unauthorized access!");

        if (currentUser.Role == Role.Admin)
        {
            var admin = await adminRepository
                .Get(user => user.Id == currentUserId, new QueryOptions(QueryTrackingMode.AsNoTracking))
                .Include(user => user.ProfileImage!)
                .ThenInclude(pi => pi.StorageFile)
                .FirstOrDefaultAsync(cancellationToken);

            return mapper.Map<UserDto>(admin);
        }

        return mapper.Map<UserDto>(currentUser);
    }
}