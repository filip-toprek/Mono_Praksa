using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Model.Common
{
    public interface IDeleteInviteModel
    {
        Guid Id { get; set; }
        DateTime DateUpdated { get; set; }
        Guid UpdatedBy { get; set; }
    }
}
