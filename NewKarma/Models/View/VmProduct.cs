using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.View
{
    public class VmProduct
    {
        
        public int ProductId { get; set; }
        [Display(Name = "دسته بندی")]
        public int CatIDFK { get; set; }
        [Display(Name = "برند")]
        public int? BrandIDFK { get; set; }
        [Display(Name = "خودرو")]
        public int[] CarIDFK { get; set; }

        [Display(Name = "خودرو")]
        public string CarName { get; set; }
        [Display(Name = "دسته")]
        public string CategoryName { get; set; }
        [Display(Name = "برند")]
        public string BrandName { get; set; }

        public string UserIdFK { get; set; }
        [Display(Name = "نام قطعه")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "انتشار")]
        public bool Situation { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }

        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
    public class CarList
    {
        public int CarId { get; set; }
        public string CarTitleModel { get; set; }
    }
}
