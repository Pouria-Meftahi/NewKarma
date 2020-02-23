using NewKarma.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKarma.Models.Domain
{
    public class Category
    {
        [Key, Required]
        public int CatId { get; set; }
        [Required, Display(Name = "Title")]
        public string Title { get; set; }
        //[Display(Name = "Icon")]
        //public string Icon { get; set; }
        //[Required, Display(Name = "Description")]
        //public string Description { get; set; }

        public string UserIDFK { get; set; }
        [ForeignKey(nameof(UserIDFK)), NotMapped]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
