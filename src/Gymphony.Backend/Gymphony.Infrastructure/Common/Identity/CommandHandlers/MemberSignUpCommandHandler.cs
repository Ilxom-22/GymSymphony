using AutoMapper;
using Gymphony.Application.Common.EventBus.Brokers;
using Gymphony.Application.Common.Identity.Commands;
using Gymphony.Application.Common.Identity.Events;
using Gymphony.Application.Common.Identity.Models.Dtos;
using Gymphony.Application.Common.Identity.Services;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using Member = Gymphony.Domain.Entities.Member;

namespace Gymphony.Infrastructure.Common.Identity.CommandHandlers;

public class MemberSignUpCommandHandler(
    IMemberRepository memberRepository,
    IMapper mapper,
    IPasswordHasherService passwordHasherService,
    IEventBusBroker eventBusBroker)
    : ICommandHandler<MemberSignUpCommand, UserDto>
{
    public async Task<UserDto> Handle(MemberSignUpCommand request, CancellationToken cancellationToken)
    {
        var memberData = mapper.Map<Member>(request.SignUpDetails);
        memberData.AuthDataHash = passwordHasherService.HashPassword(request.SignUpDetails.AuthData);
        memberData.AuthenticationProvider = request.AuthProvider;
        memberData.Status = request.AuthProvider == Provider.EmailPassword
            ? AccountStatus.Unverified
            : AccountStatus.Active;

        var member = await memberRepository
            .CreateAsync(memberData, cancellationToken: cancellationToken);

        if (member.AuthenticationProvider == Provider.EmailPassword)
            await eventBusBroker.PublishLocalAsync(new MemberCreatedEvent { Member = member });

        return mapper.Map<UserDto>(member);
    }
}