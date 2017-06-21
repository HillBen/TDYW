using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace TDYW.Models.PoolViewModels
{
    public class PoolStat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsPublic { get; set; }

        public bool OpenEnrollment { get; set; }

        public ApplicationUser Owner { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

    }
}
