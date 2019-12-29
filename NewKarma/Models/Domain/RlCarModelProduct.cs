using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.Domain
{
    public class RlCarModelProduct
    {
        public int CarId { get; set; }
        public int ProductId { get; set; }
        public Car Car { get; set; }
        public Product Product { get; set; }
    }
}
