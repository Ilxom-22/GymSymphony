using Gymphony.Application.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gymphony.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpPut("update-price")]
    public async ValueTask<IActionResult> UpdateProductPrice([FromBody] UpdateProductPriceCommand updateProductPriceCommand, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(updateProductPriceCommand, cancellationToken);
        
        return Ok(result);
    }
}