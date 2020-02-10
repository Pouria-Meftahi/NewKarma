using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Identity.Data
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        public string Description { get; set; }
        public ApplicationRole(string roleName, string roleDescription) : base(roleName)
        {
            Description = roleDescription;
        }
        public virtual List<ApplicationUserRole> Users { get; set; }
        public virtual List<ApplicationRoleClaim> Claims { get; set; }
    }
}
