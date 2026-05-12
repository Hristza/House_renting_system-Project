using System.Security.Claims;
using House_renting_system_Project.Extentions;

namespace House_renting_system_Project.Extentions
{
    public static class UserExtensions
    {
        extension(ClaimsPrincipal user)
        {
            public string? GetId()
                => user.FindFirstValue(ClaimTypes.NameIdentifier);
            public bool IsClient()
                => user.IsInRole(RoleNames.Client);

            public bool IsAgent()
                => user.IsInRole(RoleNames.Agent);
        }
    }
}
