using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Admin.Models
{
    public class VmRolesManager
    {
        public string RoleId { get; set; }
        [Display(Name = "عنوان نقش")]
        public string RoleName { get; set; }
        //, Required(ErrorMessage = Message.RegMsg)
        [Display(Name = "توضیحات نقش")]
        public string RoleDescription { get; set; }

        [Display(Name = "کاربران")]
        public int UsersCount { get; set; }
        public string RecentRoleName { get; set; }
    }
}
