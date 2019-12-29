using System.Security.Claims;

namespace NewKarma.Areas.Identity.Services
{
    public interface ISecurityTrimmingServices
    {
        bool CanCurrentUserAccess(string area, string controller, string action);
        bool CanUserAccess(ClaimsPrincipal user, string area, string controller, string action);
    }
}
