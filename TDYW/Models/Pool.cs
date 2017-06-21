using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace TDYW.Models
{
    public class Pool
    {
        public Pool()
        {
            #region defaults

            //Private = false;
            Name = "Untitled";
            OpenEnrollment = true;
            PicksPerPlayer = 10;
            OversPerPlayer = 3;
            StartDate = DateTime.Now.AddDays(7);
            EndDate = DateTime.Now.AddDays(7).AddYears(1);
            //AllowPluralityVote = false;
            //RequireTwoThirdsVote = false;
            FixedAgeBonus = true;
            FixedAgeBonusMinuend = 100;
            //WeightedAgeBonus = false;
            WeightedAgeBonusFactor = 100;
            //FixedRankBonus = false;
            FixedRankBonusFactor = 100;
            //WeightedRankBonus = false;
            WeightedRankBonusFactor = 100;
            #endregion

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name="Pool Name", GroupName ="Main Details")]
        public string Name { get; set; }

        [MaxLength(255)]
        [Display(Name = "Pool Description", GroupName = "Main Details")]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date", GroupName = "Main Details")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date", GroupName = "Main Details")]
        public DateTime EndDate { get; set; }


        public bool Private { get; set; }

        public bool OpenEnrollment { get; set; }

        public int PicksPerPlayer { get; set; }

        public int OversPerPlayer { get; set; }


        [Display(Name = "Exclude abstentions from vote count?")]
        public bool AllowPluralityVote { get; set; }


        [Display(Name = "Require a two-thirds supermajority?")]
        public bool RequireTwoThirdsVote { get; set; }


        [Display(Name = "Award a fixed age bonus?")]
        public bool FixedAgeBonus { get; set; }

        public int FixedAgeBonusMinuend { get; set; }


        [Display(Name = "Award a weighted age bonus?")]
        public bool WeightedAgeBonus { get; set; }

        public int WeightedAgeBonusFactor { get; set; }



        [Display(Name = "Award a weighted rank bonus?")]
        public bool WeightedRankBonus { get; set; }


        public int WeightedRankBonusFactor { get; set; }

        [Display(Name ="Award a fixed rank bonus?")]
        public bool FixedRankBonus { get; set; }

        public int FixedRankBonusFactor { get; set; }


        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Owner { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();


        public bool GameIsOn()
        {
            return !(GameIsOver() || IsPreGame());
        }

        public bool IsPreGame()
        {
            return DateTime.Now < StartDate;  
        }

        public bool GameIsOver()
        {
            return DateTime.Now >= EndDate;
        }

        public bool UserIsPlaying(string userId)
        {
            return Players.Any(p => p.UserId == userId);
        }
    }
}
