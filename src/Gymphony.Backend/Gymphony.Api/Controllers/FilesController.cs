using AutoMapper;
using Gymphony.Application.Common.StorageFiles.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("courses/{courseId:guid}")]
    public async ValueTask<IActionResult> UploadCourseImageAsync(
        [FromForm] IFormFile courseImage,
        [FromRoute] Guid courseId,
        CancellationToken cancellationToken)
    {
        var uploadCourseImageCommand = mapper.Map<UploadCourseImageCommand>(courseImage);
        uploadCourseImageCommand.CourseId = courseId;

        var result = await mediator.Send(uploadCourseImageCommand, cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("courseImages/{courseImageId:guid}")]
    public async ValueTask<IActionResult> DeleteCourseImageAsync(Guid courseImageId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCourseImageCommand { CourseImageId = courseImageId }, cancellationToken);

        return NoContent();
    }
}
