using SportzHunter.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Model
{
    public class DeleteInviteModel : IDeleteInviteModel
    {
        public Guid Id { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
