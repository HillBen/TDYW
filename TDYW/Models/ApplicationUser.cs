﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TDYW.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        [Display(Name ="User Name")]
        public string DisplayName { get; set; }

        public ICollection<Pool> Pools { get; set; } = new List<Pool>();

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
