using AutoMapper;
using FluentValidation;
using Gymphony.Application.Common.Payments.Commands;
using Gymphony.Application.Common.Payments.Models.Dtos;
using Gymphony.Application.Courses.Services;
using Gymphony.Application.Subscriptions.Commands;
using Gymphony.Domain.Brokers;
using Gymphony.Domain.Common.Commands;
using Gymphony.Domain.Common.Queries;
using Gymphony.Domain.Entities;
using Gymphony.Domain.Enums;
using Gymphony.Persistence.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace Gymphony.Infrastructure.Subscriptions.CommandHandlers;

public class SubscribeForCourseCommandHandler(
    IMapper mapper, IMediator mediator,
    IRequestContextProvider requestContextProvider,
    ICourseScheduleRepository courseScheduleRepository,
    ICourseRepository courseRepository,
    ICourseSubscriptionRepository courseSubscriptionRepository,
    ICourseScheduleEnrollmentRepository courseScheduleEnrollmentRepository,
    ITimeService timeService,
    IValidator<SubscribeForCourseCommand> commandValidator,
    IMemberRepository memberRepository) : ICommandHandler<SubscribeForCourseCommand, CheckoutSessionDto>
{
    public async Task<CheckoutSessionDto> Handle(SubscribeForCourseCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await commandValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ArgumentException(validationResult.Errors[0].ToString());

        var memberId = requestContextProvider.GetUserIdFromClaims()
                       ?? throw new AuthenticationException("Unauthorized access!");

        var member = await memberRepository
                         .Get(member => member.Id == memberId)
                         .FirstOrDefaultAsync(cancellationToken)
                     ?? throw new AuthenticationException("Unauthorized access!");

        var subscription = await courseSubscriptionRepository
            .Get(c => c.MemberId == memberId && c.CourseId == request.CourseId)
            .Include(c => c.LastSubscriptionPeriod)
            .FirstOrDefaultAsync(cancellationToken);

        if (subscription?.LastSubscriptionPeriod?.ExpiryDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException($"You already have an active subscription for this course.");

        var course = await courseRepository.Get(c => c.Id == request.CourseId)
            .Include(c => c.Schedules!)
                .ThenInclude(s => s.Enrollments)
            .Include(c => c.Schedules!)
            .   ThenInclude(s => s.PendingEnrollments)
            .Include(c => c.StripeDetails)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ArgumentException($"Course with id {request.CourseId} does not exist!");

        if (course.Status != ContentStatus.Activated || course.StripeDetails is null)
            throw new ArgumentException($"Course with id {request.CourseId} is not available for subscription!");

        var chosenSchedules = await  ValidateAndGetChosenSchedules(course, request.SchedulesIds, memberId, cancellationToken);

        var createCheckoutSessionCommand = mapper.Map<CreateCheckoutSessionCommand>(request);
        createCheckoutSessionCommand.Member = member;
        createCheckoutSessionCommand.PriceId = course.StripeDetails.PriceId;

        var stripeCheckoutSession = await mediator.Send(createCheckoutSessionCommand, cancellationToken);

        foreach (var schedule in chosenSchedules)
        {
            schedule.PendingEnrollments ??= [];

            schedule.PendingEnrollments.Add(new PendingScheduleEnrollment
            {
                Course = course,
                Member = member,
                StripeSessionId = stripeCheckoutSession.SessionId
            });

            await courseScheduleRepository.UpdateAsync(schedule, cancellationToken: cancellationToken);
        }

        return stripeCheckoutSession;
    }

    private async ValueTask<List<CourseSchedule>> ValidateAndGetChosenSchedules(Course course, ICollection<Guid> scheduleIds, Guid memberId, CancellationToken cancellationToken)
    {
        var chosenSchedules = course.Schedules?
            .Where(s => scheduleIds.Contains(s.Id))
            .ToList();

        if (chosenSchedules?.Count != scheduleIds.Count)
            throw new ArgumentException("Some schedules are not found!");

        foreach (var schedule in chosenSchedules)
        {
            if (schedule.Enrollments?.Count + schedule.PendingEnrollments?.Count >= course.EnrollmentsCountPerWeek)
                throw new ArgumentException($"Course schedule with id {schedule.Id} has reached maximum enrollment.");
        }

        // need to query the personal schedules and check if any of them overlap.
        var personalSchedules = await courseScheduleEnrollmentRepository
            .Get(cs => cs.MemberId == memberId, new QueryOptions(QueryTrackingMode.AsNoTracking))
            .Include(cs => cs.CourseSchedule)
            .Select(cs => cs.CourseSchedule)
            .ToListAsync(cancellationToken);

        if (AreSchedulesOverlapping(chosenSchedules, personalSchedules!))
        {
            throw new ArgumentException("Some of the chosen schedules are overlapping with the existing ones. Please check you personal calendar.");
        }

        return chosenSchedules;
    }

    public bool AreSchedulesOverlapping(List<CourseSchedule> chosenSchedules, List<CourseSchedule> personalSchedules)
    {
        return chosenSchedules.Any(schedule1 =>
           personalSchedules.Any(schedule2 =>
               timeService.IsTimeOverlapping(schedule1, schedule2)));
    }


}
