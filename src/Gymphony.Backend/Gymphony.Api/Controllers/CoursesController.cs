using Gymphony.Application.Courses.Commands;
using Gymphony.Application.Courses.Models.Dtos;
using Gymphony.Application.Courses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class CoursesController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("public")]
    public async ValueTask<IActionResult> GetPublicCoursesAsync(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPublicCoursesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("all")]
    public async ValueTask<IActionResult> GetAllCourses(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllCoursesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("course-details/{courseId:guid}")]
    public async ValueTask<IActionResult> GetCourseDetails([FromRoute] Guid courseId, CancellationToken cancellaitonToken)
    {
        var result = await mediator.Send(new GetCourseDetailsQuery { CourseId = courseId }, cancellaitonToken);

        return Ok(result);
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateCourseAsync([FromBody] DraftCourseDto course, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCourseCommand { Course = course }, cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    public async ValueTask<IActionResult> UpdateDraftCourseAsync([FromBody] UpdateDraftCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut("publish")]
    public async ValueTask<IActionResult> PublishCourseAsync([FromBody] PublishCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{courseId:guid}")]
    public async ValueTask<IActionResult> DeleteDraftCourseAsync([FromRoute] Guid courseId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveDraftCourseCommand { CourseId = courseId }, cancellationToken);

        return NoContent();
    }
}
