using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Infrastructure.Identity;

public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
    {
        var targetScope = GetTargetScope(context);

        if (targetScope != null)
        {
            var scopes = context.User
                .FindAll(claim => claim.Type == "scope")
                .SelectMany(claim => claim.Value.Split(' '));

            if (scopes.Contains(targetScope))
            {
                context.Succeed(requirement);
            }
        }
        else
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private string GetTargetScope(AuthorizationHandlerContext context)
    {
        var httpContext = (HttpContext)context.Resource!;

        var routeEndpoint = httpContext.GetEndpoint();

        var descriptor = routeEndpoint?.Metadata
            .OfType<ControllerActionDescriptor>()
            .SingleOrDefault();

        if (descriptor == null)
        {
            return null;
        }

        var scopeAttribute = (ScopeAttribute)descriptor.MethodInfo.GetCustomAttribute(typeof(ScopeAttribute))
                             ?? (ScopeAttribute)descriptor.ControllerTypeInfo
                                 .GetCustomAttribute(typeof(ScopeAttribute));

        return scopeAttribute?.ScopeName;
    }
}