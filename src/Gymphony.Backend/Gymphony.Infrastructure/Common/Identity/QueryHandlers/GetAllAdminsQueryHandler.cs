using AutoMapper;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Common.Identity.QueryHandlers;

public class GetAllAdminsQueryHandler(IMapper mapper,
    IAdminRepository adminRepository,
    IRequestContextProvider requestContextProvider) : IQueryHandler<GetAllAdminsQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetAllAdminsQuery request, CancellationToken cancellationToken)
    {
        var adminId = requestContextProvider.GetUserIdFromClaims()
            ?? throw new AuthenticationException("Unauthorized access!");

        var admins = await adminRepository.Get(admin => admin.Id != adminId, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(admin => admin.ProfileImage!)
            .ThenInclude(pi => pi.StorageFile)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<UserDto>>(admins);
    }
}
