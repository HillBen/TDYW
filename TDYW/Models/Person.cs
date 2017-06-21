using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TDYW.Services;

namespace TDYW.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PageId { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        public DateTime? DeathDate { get; set; }

        public DateTime RevisionDate { get; set; }

        [ConcurrencyCheck]
        public DateTime LastQuery { get; set; }

        public ICollection<Pick> Picks { get; set; } = new List<Pick>();

        [NotMapped]
        public int Age
        {
            get
            {
                DateTime LastBreath = DeathDate.HasValue ? (DateTime)DeathDate : DateTime.Now;
                int age = LastBreath.Year - BirthDate.Year;
                if (BirthDate > LastBreath.AddYears(-age)) age--;
                return age;
            }
        }

        [NotMapped]
        public bool IsDead
        {
            get
            {
                return DeathDate.HasValue;
            }
        }
        
        [NotMapped]
        public int HrsInTheGround {
            get
            {
                return (int)(DateTime.Now - (DeathDate ?? DateTime.Now)).TotalHours;
            }
        }
        [NotMapped]
        public int HrsSinceLastCheckUp
        {
            get
            {
                return (int)(DateTime.Now - LastQuery).TotalHours;
            }
        }
    }
}
