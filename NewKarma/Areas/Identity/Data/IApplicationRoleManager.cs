using NewKarma.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Identity.Data
{
    public interface IApplicationRoleManager
    {
        #region BaseClass
        IQueryable<ApplicationRole> Roles { get; }
        ILookupNormalizer KeyNormalizer { get; set; }
        IdentityErrorDescriber ErrorDescriber { get; set; }
        IList<IRoleValidator<ApplicationRole>> RoleValidators { get; }
        bool SupportsQueryableRoles { get; }
        bool SupportsRoleClaims { get; }
        Task<IdentityResult> CreateAsync(ApplicationRole role);
        Task<IdentityResult> DeleteAsync(ApplicationRole role);
        Task<ApplicationRole> FindByIdAsync(string roleId);
        Task<ApplicationRole> FindByNameAsync(string roleName);
        string NormalizeKey(string key);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> UpdateAsync(ApplicationRole role);
        Task UpdateNormalizedRoleNameAsync(ApplicationRole role);
        Task<string> GetRoleNameAsync(ApplicationRole role);
        Task<IdentityResult> SetRoleNameAsync(ApplicationRole role, string name);
        #endregion

        #region CustomeMethod
        List<ApplicationRole> GetAllRole();
        List<VmRolesManager> GetAllRolesUserCount();
        Task<ApplicationRole> FindClaimInRole(string RoleId);
        Task<IdentityResult> AddOrUpdateClaimsAsync(string RoleId, string RoleClaimType, IList<string> SelectedRoleClaimValues);
        Task<List<VmUsersManager>> GetUsersInRoleAsync(string RoleId);
        #endregion
    }
}
