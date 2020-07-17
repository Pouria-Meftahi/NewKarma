using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Models.View
{
    public class VmCategory
    {
        public int CatID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserIDFK { get; set; }

    }
}
