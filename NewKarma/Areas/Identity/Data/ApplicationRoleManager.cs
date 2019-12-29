using NewKarma.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Identity.Data
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IApplicationRoleManager
    {
        private readonly IRoleStore<ApplicationRole> _store;
        private readonly IEnumerable<IRoleValidator<ApplicationRole>> _roleValidators;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IdentityErrorDescriber _errors;
        private readonly ILogger<RoleManager<ApplicationRole>> _logger;
        private readonly IApplicationUserManager _userManger;
        public ApplicationRoleManager(IRoleStore<ApplicationRole> store, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger,IApplicationUserManager userManager) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _store = store;
            _logger = logger;
            _errors = errors;
            _roleValidators = roleValidators;
            _keyNormalizer = keyNormalizer;
            _userManger = userManager;
        }

        public List<ApplicationRole> GetAllRole()
        {
            return Roles.ToList();
        }

        public List<VmRolesManager> GetAllRolesUserCount()
        {
            return Roles.Select(role =>
            new VmRolesManager
            {
                RoleId = role.Id,
                RoleName = role.Name,
                RoleDescription = role.Description,
                UsersCount = role.Users.Count()
            }).ToList();
        }
        public Task<ApplicationRole> FindClaimInRole(string RoleId)
        {
            return Roles.Include(a => a.Claims).FirstOrDefaultAsync(a => a.Id == RoleId);
        }
        public async Task<IdentityResult> AddOrUpdateClaimsAsync(string RoleId, string RoleClaimType, IList<string> SelectedRoleClaimValues)
        {
            var Role = await FindClaimInRole(RoleId);
            if (Role == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "NotFound",
                    Description = "نقش مورد نظر یافت نشد"
                });
            }
            var CurrentRoleClaimValues = Role.Claims.Where(r => r.ClaimType == RoleClaimType).Select(r => r.ClaimValue).ToList();
            if (CurrentRoleClaimValues == null)
                CurrentRoleClaimValues = new List<string>();

            var NewClaimValuesToAdd = SelectedRoleClaimValues.Except(CurrentRoleClaimValues).ToList();
            foreach (var claim in NewClaimValuesToAdd)
            {
                Role.Claims.Add(new ApplicationRoleClaim
                {
                    RoleId = RoleId,
                    ClaimType = RoleClaimType,
                    ClaimValue = claim,
                });
            }
            var RemoveClaimValues = CurrentRoleClaimValues.Except(SelectedRoleClaimValues).ToList();
            foreach (var claim in RemoveClaimValues)
            {
                var RoleClaim = Role.Claims.SingleOrDefault(a => a.ClaimValue == claim && a.ClaimType == RoleClaimType);
                if (RoleClaim == null)
                    Role.Claims.Remove(RoleClaim);
            }
            return await UpdateAsync(Role);
        }
        public async Task<List<VmUsersManager>> GetUsersInRoleAsync(string RoleId)
        {
           //Hack:Cannot use multiple DbContext instances within a single query execution. Ensure the query uses a single context instance.
            var UserIds = (from r in Roles
                           where (r.Id == RoleId)
                           from u in r.Users
                           select u.UserId);
            return await _userManger.Users.Where(user => UserIds.Contains(user.Id))
                .Select(user => new VmUsersManager
                {
                    Id = user.Id,
                    Name = user.FirstName,
                    Family = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    IsActive = user.Status,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEnd = user.LockoutEnd,
                    RegisterDate = user.RegisterDate,
                    AccessFaildCount = user.AccessFailedCount,
                    TowFactorEnabled = user.TwoFactorEnabled,
                    Roles = user.Roles.Select(u => u.Role.Name),
                }).AsNoTracking().ToListAsync();
        }
    }
}
