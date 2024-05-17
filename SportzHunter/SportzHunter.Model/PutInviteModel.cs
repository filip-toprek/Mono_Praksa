using SportzHunter.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportzHunter.Model
{
    public class PutInviteModel : IPutInviteModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid PlayerId { get; set; }
        [Required]
        public Guid TeamId { get; set; }
        [Required]
        public bool ResponseSent { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid UpdatedBy { get; set;}
    }
}
