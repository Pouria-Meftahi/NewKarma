using NewKarma.Areas.Admin.Models;
using NewKarma.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NewKarma.Areas.Identity.Data
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IApplicationUserManager
    {
        private readonly IUserStore<ApplicationUser> _store;
        private readonly IOptions<IdentityOptions> _optionsAccessor;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IEnumerable<IUserValidator<ApplicationUser>> _userValidators;
        private readonly IEnumerable<IPasswordValidator<ApplicationUser>> _passwordValidators;
        private readonly ILookupNormalizer _keyNormalizer;
        private readonly IServiceProvider _services;
        private readonly ILogger<ApplicationUserManager> _logger;
        private readonly AppDbContext _context;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<ApplicationUserManager> logger, AppDbContext context) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _logger = logger;
            _store = store;
            _services = services;
            _keyNormalizer = keyNormalizer;
            _optionsAccessor = optionsAccessor;
            _passwordHasher = passwordHasher;
            _passwordValidators = passwordValidators;
            _userValidators = userValidators;
            _context = context;
        }

        public async Task<VmUsersManager> FindUserWithRolesByIdAsync(string UserId)
            => await Users.Where(u => u.Id == UserId)
                          .Select(user => new VmUsersManager { Id = user.Id, Name = user.FirstName, Family = user.LastName, UserName = user.UserName, Email = user.Email, EmailConfirmed = user.EmailConfirmed, IsActive = user.Status, LockoutEnabled = user.LockoutEnabled, LockoutEnd = user.LockoutEnd,RegisterDate = user.RegisterDate, AccessFaildCount = user.AccessFailedCount, TowFactorEnabled = user.TwoFactorEnabled, Roles = user.Roles.Select(u => u.Role.Name), }).FirstOrDefaultAsync();

        public async Task<List<ApplicationUser>> GetAllUserAsync() => await Users.ToListAsync();

        public List<VmUsersManager> GetAllUserWithRole() => Users.Select(user => new VmUsersManager { Id = user.Id, Name = user.FirstName, Family = user.LastName, UserName = user.UserName, Email = user.Email, IsActive = user.Status, RegisterDate = user.RegisterDate, Roles = user.Roles.Select(u => u.Role.Name).ToList(), }).ToList();

        public async Task<List<VmUsersManager>> GetAllUserWithRoleAsync() => await Users.Select(user => new VmUsersManager { Id = user.Id, Name = user.FirstName, Family = user.LastName, UserName = user.UserName, Email = user.Email, IsActive = user.Status, RegisterDate = user.RegisterDate, Roles = user.Roles.Select(u => u.Role.Name), }).ToListAsync();

        public async Task<string> GetFullName(ClaimsPrincipal User)
        {
            var UserInfo = await GetUserAsync(User);
            return UserInfo.FirstName + " " + UserInfo.LastName;
        }

        public async Task<string> GetRoleSingleOrDefualtAsync(ClaimsPrincipal user)
        {
            var userInfo = await GetUserAsync(user);
            return userInfo.Roles.Select(a => a.Role.Name).ToString();
        }

        public string GetUserByIdAsync(string UserId_FK) => UserId_FK == null ? null : Users.Where(a => a.Id == UserId_FK).FirstOrDefault().UserName;

        public ApplicationUser GetUserEmail(string userName) => _context.Users.SingleOrDefault(a => a.UserName == userName);
      
        public VmUsersManager UserInformaiion(string username)
        {
            var user = GetUserEmail(username);
            VmUsersManager userInfo = new VmUsersManager
            {
                Email = user.Email,RegisterDate = user.RegisterDate,Name = user.FirstName,Family = user.LastName,UserName = user.UserName
            };
            return userInfo;
        }
    }
}