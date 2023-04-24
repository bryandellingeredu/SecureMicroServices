using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class CustomProfileService : IProfileService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomProfileService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = context.Subject.GetSubjectId();
        var email = _httpContextAccessor.HttpContext.Items["email"] as string;

        if (!string.IsNullOrEmpty(email))
        {
            context.IssuedClaims.Add(new Claim("email", email));
        }

        var nameClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "name");
        if (nameClaim != null)
        {
            context.IssuedClaims.Add(new Claim("name", nameClaim.Value));
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = !string.IsNullOrEmpty(context.Subject.GetSubjectId());
        return Task.CompletedTask;
    }
}