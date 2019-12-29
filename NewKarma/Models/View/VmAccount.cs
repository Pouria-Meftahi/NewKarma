using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.View
{
    public class VmAccount
    {
        public class RegisterViewModel
        {
            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [Display(Name = "نام")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [Display(Name = "نام خانوادگی")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [EmailAddress(ErrorMessage = "ایمیل شما نامعتبر است.")]
            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [StringLength(100, ErrorMessage = "{0} باید دارای حداقل {2} کاراکتر و حداکثر دارای {1} کاراکتر باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار کلمه عبور")]
            [Compare("Password", ErrorMessage = "کلمه عبور وارد شده با تکرار کلمه عبور مطابقت ندارد.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "نام کاربری")]
            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            public string UserName { get; set; }
        }
        public class LoginViewModel
        {
            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [Display(Name = "نام کاربری")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور")]
            public string Password { get; set; }

            [Display(Name = "مرا به خاطر بسپار؟")]
            public bool RememberMe { get; set; }
        }

        public class ForgetPasswordViewModel
        {
            [Display(Name = "ایمیل")]
            [EmailAddress(ErrorMessage = "ایمیل وارد شده نامعتبر است.")]
            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            public string Email { get; set; }
        }

        public class ResetPasswordViewModel
        {
            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [EmailAddress(ErrorMessage = "ایمیل شما نامعتبر است.")]
            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
            [StringLength(100, ErrorMessage = "{0} باید دارای حداقل {2} کاراکتر و حداکثر دارای {1} کاراکتر باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "کلمه عبور جدید")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار کلمه عبور جدید")]
            [Compare("Password", ErrorMessage = "تکرار کلمه عبور با کلمه عبور وارد شده مطابقت ندارد.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }
    }
}