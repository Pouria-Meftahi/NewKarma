using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewKarma.Models.Domain
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        [Required,Display(Name = "نام خودرو")]
        public string CarTitle { get; set; }
        [Display(Name = "مدل خودرو")]
        public string CarModel { get; set; }
        public List<RlCarModelProduct> RlCarModelProduct { get; set; }

    }
}