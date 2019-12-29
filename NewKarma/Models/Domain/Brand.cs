using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewKarma.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.Domain
{
    public class Brand
    {
        [Key,Required]
        public int BrandId { get; set; }
        [Required,Display(Name ="عنوان")]
        public string Title { get; set; }

        public string UserIDFK { get; set; }
        [ForeignKey(nameof(UserIDFK))]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}
