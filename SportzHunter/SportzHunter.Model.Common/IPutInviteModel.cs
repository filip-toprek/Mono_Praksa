using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Model.Common
{
    public interface IPutInviteModel
    {
        Guid Id { get; set; }
        Guid PlayerId { get; set; }
        Guid TeamId { get; set; }
        bool ResponseSent { get; set; }
        DateTime DateUpdated { get; set; }
        Guid UpdatedBy { get; set; }
    }
}
