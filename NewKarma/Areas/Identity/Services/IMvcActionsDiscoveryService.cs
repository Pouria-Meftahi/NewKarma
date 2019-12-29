using NewKarma.Areas.Admin.Models;
using System.Collections.Generic;

namespace NewKarma.Areas.Identity.Services
{
    public interface IMvcActionsDiscoveryService
    {
        ICollection<VmController> GetAllSecuredControllerActionsWithPolicy(string policyName);
       
    }
}
