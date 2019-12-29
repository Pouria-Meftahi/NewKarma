using NewKarma.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Admin.Models
{
    public class VmDynamicAccessIndex
    {
        public string[] ActionId { get; set; }
        public string RoleId { get; set; }
        public ApplicationRole RoleIncludeRoleClaims { get; set; }
        public ICollection<VmController> SecuredControllerActinos { get; set; }
    }
}
