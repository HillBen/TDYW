using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW.Models
{
    public class Judgement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public int ComplaintId { get; set; }

        [ForeignKey("ComplaintId")]
        public Complaint Complaint { get; set; }


        public bool Agreed { get; set; }

        public string DateCreated { get; set; }

    }
}
