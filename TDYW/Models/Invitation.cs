using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TDYW.Services;

namespace TDYW.Models
{
    public class Invitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(78, ErrorMessage = "78 character subject line limit. Save it for the body content.")]
        public string Subject { get; set; }

        [MaxLength(255,ErrorMessage = "255 character body content limit. If you have more to say, consider a phone call.")]
        public string Content { get; set; }

        public bool OpenInvite { get; set; }

        public int PoolId { get; set; }

        public Pool Pool { get; set; }

        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();

    }
}
