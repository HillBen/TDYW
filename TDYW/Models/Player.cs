using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace TDYW.Models
{
    public class Player
    {
        //public Player()
        //{
        //    HidePicksBeforeStart = false;
        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[MaxLength(50)]
        //public string Name { get; set; }

        //public bool HidePicksBeforeStart { get; set; }

        //[EmailAddress]
        //public string Email { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PoolId")]
        public Pool Pool { get; set; }

        public int PoolId { get; set; }

        public ICollection<Pick> Picks { get; set; } = new List<Pick>();

        public ICollection<Judgement> Judgements { get; set; } = new List<Judgement>();

        public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

        [NotMapped]
        public int Points
        {
            get
            {
                return Picks.Where(p => p.PointsAwarded).Sum(s => s.PotentialValue);
            }
        }
    }
}
