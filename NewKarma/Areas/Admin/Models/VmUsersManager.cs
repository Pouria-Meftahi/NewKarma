using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Admin.Models
{
    public class VmUsersManager
    {
        public string Id { get; set; }
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }
        [Display(Name = "تاریخ عضویت")]
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool TowFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public int AccessFaildCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
