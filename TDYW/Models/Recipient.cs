using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW.Models
{
    public class Recipient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime? DateSent { get; set; }

        public DateTime? DateJoined { get; set; }

        public int InvitationId { get; set; }

        public Invitation Invitation { get; set; }
    }
}
