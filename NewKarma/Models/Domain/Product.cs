using NewKarma.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.Domain
{
    public class Product
    {
        [Key, Required]
        public int ProductId { get; set; }
        [Required, MaxLength(300), Display(Name = "عنوان")]
        public string Title { get; set; }
        [Required, Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Required, Display(Name = "تصویر")]
        public string Img { get; set; }
        [Required, Display(Name = "وضعیت")]
        public bool Situation { get; set; }
        [Display(Name = "تاریخ")]
        public DateTime CreatedDate { get; set; }


        /// <summary>
        /// Rel
        /// </summary>
        #region Rels
        public int CatIDFK { get; set; }
        [ForeignKey(nameof(CatIDFK)), Display(Name = "دسته بندی")]
        public Category Category { get; set; }
        
        public int BrandIDFK { get; set; }
        [ForeignKey(nameof(BrandIDFK)), Display(Name = "برند")]
        public Brand Brand { get; set; }

        public string UserIDFK { get; set; }
        [ForeignKey(nameof(UserIDFK))]
        public virtual ApplicationUser ApplicationUser { get; set; }
        

        public List<RlCarModelProduct> RlCarModelProduct { get; set; }

        #endregion

    }
}