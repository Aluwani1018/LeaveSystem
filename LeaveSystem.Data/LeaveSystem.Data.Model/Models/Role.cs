using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace LeaveSystem.Data.Model
{
    public class Role : IdentityRole
    {
        public Role(string roleName) : base(roleName)
        {

        }
        public Role()
        {

        }

        public Role(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Users { get; set; }
        public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; }
    }

}
