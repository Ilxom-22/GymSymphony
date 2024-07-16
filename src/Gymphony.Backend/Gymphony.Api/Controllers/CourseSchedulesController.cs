using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class CourseSchedulesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("active/{courseId:guid}")]
    public async ValueTask<IActionResult> GetActiveCourseSchedules([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCourseSchedulesQuery 
        { CourseId = courseId, IsActiveOnly = true }, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{courseId:guid}")]
    public async ValueTask<IActionResult> GetCourseSchedules([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCourseSchedulesQuery
        { CourseId = courseId, IsActiveOnly = false }, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async ValueTask<IActionResult> AddSchedulesAsync([FromBody] CreateCourseScheduleCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{scheduleId:guid}")]
    public async ValueTask<IActionResult> RemoveScheduleAsync([FromRoute] Guid scheduleId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveCourseScheduleCommand { ScheduleId = scheduleId }, cancellationToken);

        return NoContent();
    }
}
