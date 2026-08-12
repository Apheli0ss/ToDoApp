using System.Security.Claims;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.API.Middleware;

public class UserSyncMiddleware
{
    private readonly RequestDelegate _next;

    public UserSyncMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var usernameClaim = context.User.FindFirst("preferred_username")?.Value;
            var emailClaim = context.User.FindFirst(ClaimTypes.Email)?.Value ?? "no-email@test.com";

            if (!string.IsNullOrEmpty(usernameClaim))
            {
                // Check if user exists in local DB
                var user = await userRepository.GetByUsernameAsync(usernameClaim, CancellationToken.None);

                if (user == null)
                {
                    // Create local user
                    user = new User
                    {
                        Username = usernameClaim,
                        Email = emailClaim,
                        PasswordHash = "KEYCLOAK_MANAGED",
                        CreatedAt = DateTime.UtcNow
                    };
                    await userRepository.AddAsync(user, CancellationToken.None);
                    await userRepository.SaveChangesAsync(CancellationToken.None);
                }

                // Add the local integer ID to claims so GetUserId() works
                var claimsIdentity = context.User.Identity as ClaimsIdentity;
                
                // Keep the string NameIdentifier around if needed, but we must provide an integer NameIdentifier for TodoController
                // To avoid multiple NameIdentifier claims breaking int.Parse, we remove existing ones
                var existingNameIds = claimsIdentity?.FindAll(ClaimTypes.NameIdentifier).ToList();
                if (existingNameIds != null)
                {
                    foreach(var claim in existingNameIds)
                    {
                        claimsIdentity?.RemoveClaim(claim);
                    }
                }
                
                claimsIdentity?.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            }
        }

        await _next(context);
    }
}
