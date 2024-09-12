using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Gymphony.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gymphony.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, ProblemDetails> _exceptionMappings =  new()
    {
        {
            typeof(AuthenticationException), 
            new ProblemDetails { Status = StatusCodes.Status401Unauthorized } 
        },
        { 
            typeof(ArgumentException),
            new ProblemDetails { Status = StatusCodes.Status400BadRequest }
        },
        {
            typeof(InvalidOperationException),
            new ProblemDetails { Status = StatusCodes.Status500InternalServerError }
        },
        {
            typeof(FluentValidation.ValidationException),
            new ProblemDetails { Status = StatusCodes.Status400BadRequest } 
        },
        { 
            typeof(ValidationException),
            new ProblemDetails { Status = StatusCodes.Status400BadRequest } 
        },
        {
            typeof(EntityNotFoundException<>),
            new ProblemDetails { Status = StatusCodes.Status404NotFound }
        },
        {
            typeof(InvalidEntityStateChangeException<>),
            new ProblemDetails { Status = StatusCodes.Status422UnprocessableEntity }
        },
        {
            typeof(AccountNotVerifiedException),
            new ProblemDetails { Status = StatusCodes.Status403Forbidden }
        }
    };

    
    
    public void OnException(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();
        
        var exception = exceptionType.IsGenericType
            ? exceptionType.GetGenericTypeDefinition()
            : exceptionType;
        
        if (_exceptionMappings.TryGetValue(exception, out var problemDetails))
        {
            problemDetails.Detail = context.Exception.Message;
        }
        else
        {
            problemDetails = new ProblemDetails 
            {
                Status = StatusCodes.Status500InternalServerError, 
                Detail = context.Exception.Message 
            };
        }

        context.ExceptionHandled = true;

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }
}