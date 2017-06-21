using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TDYW.Models
{
    public class Pick
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        

        public int PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        //public int PoolId { get; set; }

        //[ForeignKey("PoolId")]
        //public Pool Pool { get; set; }
        public int PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        public int Rank { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

        
        public int PotentialValue { get; set; }

        public bool PointsAwarded { get; set; }
    }
}
