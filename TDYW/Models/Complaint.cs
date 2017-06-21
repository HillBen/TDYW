using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW.Models
{
    public class Complaint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public int PickId { get; set; }

        [ForeignKey("PickId")]
        public Pick Pick { get; set; }

        [MaxLength(50, ErrorMessage = "50 character max. Keep it simeple, like 'death row picks not allowed', or 'no rugby player is famous in america'.")]
        public string Description { get; set; }

        public string DateCreated { get; set; }

        public ICollection<Judgement> Judgements { get; set; } = new List<Judgement>();
    }
}
