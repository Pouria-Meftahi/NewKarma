using NewKarma.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
namespace NewKarma.Areas.Identity.Services
{
    public class SecurityTrimmingServices : ISecurityTrimmingServices
    {
        //private readonly HttpContext _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMvcActionsDiscoveryService _mvcActionsDiscoveryService;
        public SecurityTrimmingServices(IHttpContextAccessor httpContextAccessor, IMvcActionsDiscoveryService mvcActionsDiscoveryService)
        {
            _httpContextAccessor = httpContextAccessor;
            //_httpContext = _httpContextAccessor.HttpContext;
            _mvcActionsDiscoveryService = mvcActionsDiscoveryService;
        }
        public bool CanCurrentUserAccess(string area, string controller, string action)
        {
            return _httpContextAccessor.HttpContext != null && CanUserAccess(_httpContextAccessor.HttpContext.User, area, controller, action);
        }
        public bool CanUserAccess(ClaimsPrincipal user, string area, string controller, string action)
        {
            var currentClaimValue = $"{area}:{controller}:{action}";
            var securedControllerActions = _mvcActionsDiscoveryService.GetAllSecuredControllerActionsWithPolicy(ConstantPolicies.DynamicPermission);
            if (!securedControllerActions.SelectMany(x => x.MvcActinos).Any(x => x.ActionId == currentClaimValue))
            {
                throw new KeyNotFoundException($@"The `secured` area={area}/controller={controller}/action={action} with `ConstantPolicies.DynamicPermission` policy not found. Please check you have entered the area/controller/action names correctly and also it's decorated with the correct security policy.");
            }
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            return user.HasClaim(claim => claim.Type == ConstantPolicies.DynamicPermissionClaimType && claim.Value == currentClaimValue);
        }
    }
}