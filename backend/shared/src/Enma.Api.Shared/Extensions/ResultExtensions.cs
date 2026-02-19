using Enma.Common.Errors;
using Enma.Common.Errors.Types;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enma.Api.Shared.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess 
            ? new StatusCodeResult(204) 
            : ToActionResultCore(result.Errors);
    }
    
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess 
            ? new OkObjectResult(result.Value) 
            : ToActionResultCore(result.Errors);
    }
    
    private static IActionResult ToActionResultCore(IReadOnlyList<IError> errors)
    {
        var err = errors.FirstOrDefault();

        return err switch
        {
            null => new StatusCodeResult(StatusCodes.Status500InternalServerError),
            
            ConflictError conflictError       => BuildProblem(StatusCodes.Status409Conflict, conflictError),
            ForbiddenError forbiddenError     => BuildProblem(StatusCodes.Status403Forbidden, forbiddenError),
            NotFoundError notFoundError       => BuildProblem(StatusCodes.Status404NotFound, notFoundError),
            ValidationError validationError   => BuildProblem(StatusCodes.Status400BadRequest, validationError),

            Error error => BuildProblem(StatusCodes.Status500InternalServerError, error),
            _ => throw new ArgumentOutOfRangeException(nameof(err))
        };
    }
    
    private static ObjectResult BuildProblem(int statusCode, IError error)
    {
        var applicationError = error as ApplicationError;

        var pd = new ProblemDetails
        {
            Status = statusCode,
            Title = applicationError?.Category ?? "error",
            Detail = error.Message,
            Type = applicationError?.Code
        };
        
        return new ObjectResult(pd) { StatusCode = statusCode };
    }
}