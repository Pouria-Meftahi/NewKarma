using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace NewKarma.Areas.Admin.Models
{
    public class VmController
    {
        public IList<Attribute> ControllerAttributes { get; set; }
        public string ControllerDisplayName { get; set; }
        public string ControllerId => $"{AreaName}:{ControllerName}";
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public IList<VmAction> MvcActinos { get; set; } = new List<VmAction>(); 
    }
}
