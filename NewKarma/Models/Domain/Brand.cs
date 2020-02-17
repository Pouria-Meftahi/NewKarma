using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewKarma.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewKarma.Models.Domain
{
    public class Brand
    {
        [Key, Required]
        public int BrandId { get; set; }
        [Required, Display(Name = "عنوان")]
        public string Title { get; set; }

        //Todo Add Migration & Update Database↓
        //public string Description { get; set; }
        //public string Image { get; set; }

        public string UserIDFK { get; set; }
        [ForeignKey(nameof(UserIDFK))]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
