using AutoMapper;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Queries;
using Gymphony.Domain.Common.Queries;
using Gymphony.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymphony.Infrastructure.Common.Identity.QueryHandlers;

public class GetAllStaffQueryHandler(IStaffRepository staffRepository, IMapper mapper) : IQueryHandler<GetAllStaffQuery, IEnumerable<StaffDto>>
{
    public async Task<IEnumerable<StaffDto>> Handle(GetAllStaffQuery request, CancellationToken cancellationToken)
    {
        var staff = staffRepository.Get(queryOptions: new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(staff => staff.ProfileImage!)
            .ThenInclude(pi => pi.StorageFile);

        return mapper.Map<IEnumerable<StaffDto>>(staff);
    }
}
