using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace WolverineMiddlewareApply;

public class User
{
    public string Id { get; set; }
}

public record UserId(Guid Id);
 
public static class UserIdMiddleWare
{
    public static (UserId, ProblemDetails) Load(ClaimsPrincipal principal, HttpContext context)
    {
        Claim? userIdClaim = principal.FindFirst("sub");
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var id))
        {
            return ( new UserId(id), WolverineContinue.NoProblems);
        }

        Endpoint? endpoint = context.GetEndpoint();
        return endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null 
            ? ( new UserId(Guid.Empty), WolverineContinue.NoProblems) 
            : (new UserId(Guid.Empty), new ProblemDetails { Detail = "Unauthorized", Status = 401});
    }
}