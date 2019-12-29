using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.View
{
    public class VmBrand
    {
        public int brandId { get; set; }
        public string Title { get; set; }
        public int UserIDFK { get; set; }
    }
}
